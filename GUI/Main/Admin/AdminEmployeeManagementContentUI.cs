using BLL.Admin;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Guna.UI2.WinForms;
using GUI;
using GUI.Helpers;

using Control = System.Windows.Forms.Control;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;
using UserControl = System.Windows.Forms.UserControl;

namespace GUI.Admin
{
    public partial class AdminEmployeeManagementContentUI : UserControl
    {
        private readonly AdminBLL _adminBLL = AdminBLL.Instance;
        private List<UserDTO> _fullEmployeeList;
        private string _selectedEmployeeId = null;
        private bool _isAddingNew = false;
        private bool _isUpdatingSearchBoxProgrammatically = false;
        
        // Sử dụng static để giữ trạng thái khi chuyển tab
        private static Dictionary<string, bool> _hiddenEmployees = new Dictionary<string, bool>(); // Lưu trạng thái ẩn/hiện cho từng nhân viên

        private GUI.VoskVoiceRecognizer recognizer;
        private bool isListening = false;

        private class Language
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        public AdminEmployeeManagementContentUI()
        {
            InitializeComponent();
            // Subscribe to data change events
            GlobalEventBus.EmployeeDataChanged += OnEmployeeDataChanged;
            this.Disposed += (s, e) =>
            {
                GlobalEventBus.EmployeeDataChanged -= OnEmployeeDataChanged;
            };
        }

        private void OnEmployeeDataChanged()
        {
            // Refresh employee list when employee data changes
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshData));
            }
            else
            {
                RefreshData();
            }
        }

        public void RefreshData()
        {
            // Refresh employee list and maintain current selection if possible
            string previousSelectedId = _selectedEmployeeId;
            LoadFullEmployeeList();
            
            // Restore selection if it still exists
            if (!string.IsNullOrEmpty(previousSelectedId))
            {
                var employee = _fullEmployeeList?.FirstOrDefault(e => e.Company_UserId == previousSelectedId);
                if (employee != null)
                {
                    DisplayEmployeeDetails(previousSelectedId);
                }
            }
        }

        private void AdminEmployeeManagementContentUI_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
            LoadFullEmployeeList();
            SetDetailPanelEnabled(false, false);

            //SetupLanguageSelector();
            InitializeVoiceSearch();

            txtSearch.Text = "Tìm kiếm nhân viên...";
            txtSearch.ForeColor = Color.Gray;
            ApplyPermissions();
            
            // Khởi tạo icon và tooltip cho nút Ẩn/Hiện
            btnHide.Image = Properties.Resources.eye_closed;
            toolTip.SetToolTip(btnHide, "Ẩn thông tin phụ của nhân viên đang chọn");
        }

        private void ApplyPermissions()
        {
            // Vô hiệu hóa nút thêm, sửa, xóa nếu không có quyền quản lý nhân viên (chỉ Admin)
            if (!PermissionHelper.CanManageEmployees())
            {
                btnAdd.Enabled = false;
                btnAdd.Visible = false;
                btnEdit.Enabled = false;
                btnEdit.Visible = false;
                btnDelete.Enabled = false;
                btnDelete.Visible = false;
                btnHide.Enabled = false;
                btnHide.Visible = false;
            }
        }

        #region TÍCH HỢP VOICE SEARCH


        private void InitializeVoiceSearch()
        {
            recognizer?.Dispose();
            try
            {
                string modelFolderName = "vn";
                string sourceModelPath = Path.Combine(Application.StartupPath, "Models", modelFolderName);
                EnsureVoskNativeOnPath();
                string safeModelPath = EnsureSafeModelPath(sourceModelPath, modelFolderName);

                recognizer = new GUI.VoskVoiceRecognizer(safeModelPath);
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                picMicrophoneIcon.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo bộ nhận dạng giọng nói Vosk:\n\n" + ex.Message,
                                "Lỗi Voice Search", MessageBoxButtons.OK, MessageBoxIcon.Error);
                picMicrophoneIcon.Enabled = false;
                recognizer = null;
            }
        }

        private void EnsureVoskNativeOnPath()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string nativeInOutput = Path.Combine(baseDir, "runtimes", "win-x64", "native");
                string candidate = Directory.Exists(nativeInOutput) ? nativeInOutput
                    : Path.GetFullPath(Path.Combine(baseDir, "..", "..", "packages", "Vosk.0.3.38", "runtimes", "win-x64", "native"));
                if (Directory.Exists(candidate))
                {
                    string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
                    if (!path.Split(';').Any(p => string.Equals(p.TrimEnd('\\'), candidate.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)))
                    {
                        Environment.SetEnvironmentVariable("PATH", candidate + ";" + path, EnvironmentVariableTarget.Process);
                    }
                }
            }
            catch { }
        }

        private string EnsureSafeModelPath(string sourceModelPath, string languageFolder)
        {
            try
            {
                if (!Directory.Exists(sourceModelPath) || !File.Exists(Path.Combine(sourceModelPath, "am", "final.mdl")))
                {
                    throw new DirectoryNotFoundException("Thiếu dữ liệu model Vosk.");
                }
                bool hasNonAscii = sourceModelPath.Any(c => c > 127);
                if (!hasNonAscii) return sourceModelPath;
                string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EcoGreen", "VoskModels", languageFolder);
                if (!Directory.Exists(appDir))
                {
                    Directory.CreateDirectory(appDir);
                    CopyDirectoryRecursive(sourceModelPath, appDir);
                }
                return appDir;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chuẩn bị model Vosk: " + ex.Message, "Voice Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void CopyDirectoryRecursive(string sourceDir, string targetDir)
        {
            foreach (string dir in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(Path.Combine(targetDir, dir.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar)));
            }
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                File.Copy(file, Path.Combine(targetDir, file.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar)), true);
            }
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isListening && recognizer != null)
            {
                recognizer.Stop();
                isListening = false;
                picMicrophoneIcon.BackColor = Color.Transparent;
            }
            InitializeVoiceSearch();
        }

        private void picMicrophoneIcon_Click(object sender, EventArgs e)
        {
            if (recognizer == null || !picMicrophoneIcon.Enabled) return;
            isListening = !isListening;
            if (isListening)
            {
                recognizer.Start();
                picMicrophoneIcon.BackColor = Color.LightCoral;

                _isUpdatingSearchBoxProgrammatically = true;
                try
                {
                    txtSearch.ForeColor = Color.Black;
                    txtSearch.Text = "Đang lắng nghe...";
                }
                finally
                {
                    _isUpdatingSearchBoxProgrammatically = false;
                }
            }
            else
            {
                recognizer.Stop();
                picMicrophoneIcon.BackColor = Color.Transparent;
                if (txtSearch.Text == "Đang lắng nghe...")
                {
                    _isUpdatingSearchBoxProgrammatically = true;
                    try
                    {
                        txtSearch.Text = "";
                        txtSearch_Leave(sender, e); // Gọi Leave để đặt lại placeholder
                    }
                    finally
                    {
                        _isUpdatingSearchBoxProgrammatically = false;
                    }
                }
            }
        }

        private void Recognizer_SpeechRecognized(object sender, string recognizedText)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _isUpdatingSearchBoxProgrammatically = true;
                try
                {
                    txtSearch.ForeColor = Color.Black;
                    txtSearch.Text = recognizedText;
                }
                finally
                {
                    _isUpdatingSearchBoxProgrammatically = false;
                }

                // Kích hoạt tìm kiếm thủ công sau khi đã cập nhật text
                txtSearch_TextChanged(txtSearch, EventArgs.Empty);

                if (isListening && recognizer != null)
                {
                    recognizer.Stop();
                    isListening = false;
                    picMicrophoneIcon.BackColor = Color.Transparent;
                }
            });
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            recognizer?.Dispose();
            base.OnHandleDestroyed(e);
        }
        #endregion

        #region Data Loading & Display
        private void LoadComboBoxData()
        {
            cmbPosition.DataSource = _adminBLL.GetAllRoles();
            cmbPosition.DisplayMember = "RoleName";
            cmbPosition.ValueMember = "RoleId";

            cmbDepartment.DataSource = _adminBLL.GetAllDepartments();
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "DepartmentId";
        }

        private void LoadFullEmployeeList()
        {
            _fullEmployeeList = _adminBLL.GetAllUsers();
            DisplayEmployees(_fullEmployeeList);
        }

        private void DisplayEmployees(List<UserDTO> employeesToDisplay)
        {
            flowLayoutPanelUsers.Controls.Clear();
            if (employeesToDisplay == null) return;
            foreach (var user in employeesToDisplay)
            {
                var item = new ucUserItem();
                item.SetData(user.FullName, $"{user.RoleName} ({user.Company_UserId})");
                item.Tag = user.Company_UserId;
                
                // Áp dụng trạng thái ẩn/hiện cho từng nhân viên
                bool isHidden = _hiddenEmployees.ContainsKey(user.Company_UserId) && _hiddenEmployees[user.Company_UserId];
                item.SetHidden(isHidden);

                item.Click += UserItem_Click;
                if (_selectedEmployeeId != null && user.Company_UserId == _selectedEmployeeId)
                {
                    item.SetSelected(true);
                }
                flowLayoutPanelUsers.Controls.Add(item);
            }
        }

        private void UserItem_Click(object sender, EventArgs e)
        {
            var clickedItem = sender as ucUserItem;
            if (clickedItem == null) return;
            _isAddingNew = false;
            _selectedEmployeeId = clickedItem.Tag.ToString();
            foreach (Control control in flowLayoutPanelUsers.Controls)
            {
                if (control is ucUserItem item)
                {
                    item.SetSelected(item.Tag.ToString() == _selectedEmployeeId);
                }
            }
            DisplayEmployeeDetails(_selectedEmployeeId);
            
            // Cập nhật icon và tooltip của nút Ẩn/Hiện dựa trên trạng thái của nhân viên đang chọn
            bool isHidden = _hiddenEmployees.ContainsKey(_selectedEmployeeId) && _hiddenEmployees[_selectedEmployeeId];
            if (isHidden)
            {
                btnHide.Image = Properties.Resources.eye_open;
                toolTip.SetToolTip(btnHide, "Hiện thông tin");
            }
            else
            {
                btnHide.Image = Properties.Resources.eye_closed;
                toolTip.SetToolTip(btnHide, "Ẩn thông tin");
            }
            
            // Cập nhật trạng thái các nút thêm, sửa, xóa dựa trên quyền và trạng thái ẩn/hiện
            if (PermissionHelper.CanManageEmployees())
            {
                if (isHidden)
                {
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
        }

        private void DisplayEmployeeDetails(string employeeId)
        {
            UserDTO user = _adminBLL.GetUserDetails(employeeId);
            if (user == null) { MessageBox.Show("Không tìm thấy thông tin."); return; }

            SetDetailPanelEnabled(true, false);

            // Kiểm tra xem nhân viên này có đang bị ẩn không
            bool isHidden = _hiddenEmployees.ContainsKey(employeeId) && _hiddenEmployees[employeeId];

            if (isHidden)
            {
                // Nếu đang ẩn: chỉ hiển thị mã số và họ tên, tất cả các trường khác để trống và ẩn
                txtUserID.Text = user.Company_UserId;
                txtFullName.Text = user.FullName;
                
                // Tất cả các trường khác để trống
                txtPhoneNumber.Text = "";
                txtEmail.Text = "";
                txtHometown.Text = "";
                txtNationality.Text = "";
                txtCurrentAddress.Text = "";
                
                // Ẩn các trường: ngày sinh, phòng ban, giới tính, chức vụ
                lblDateOfBirth.Visible = false;
                dtpDateOfBirth.Visible = false;
                lblDepartment.Visible = false;
                cmbDepartment.Visible = false;
                lblGender.Visible = false;
                cmbGender.Visible = false;
                lblPosition.Visible = false;
                cmbPosition.Visible = false;
                
                // Vô hiệu hóa các nút thêm, sửa, xóa
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                // Nếu không ẩn: hiển thị đầy đủ thông tin
                txtUserID.Text = user.Company_UserId;
                txtFullName.Text = user.FullName;
                dtpDateOfBirth.Value = user.DateOfBirth ?? DateTime.Now.AddYears(-22);
                txtPhoneNumber.Text = user.Phone;
                txtEmail.Text = user.Email;
                cmbGender.SelectedItem = user.Gender;
                txtHometown.Text = user.Address;
                txtNationality.Text = "Việt Nam";
                txtCurrentAddress.Text = user.Address;
                cmbDepartment.SelectedValue = user.DepartmentId;
                cmbPosition.SelectedValue = user.RoleId;
                
                // Hiện lại các trường đã ẩn
                lblDateOfBirth.Visible = true;
                dtpDateOfBirth.Visible = true;
                lblDepartment.Visible = true;
                cmbDepartment.Visible = true;
                lblGender.Visible = true;
                cmbGender.Visible = true;
                lblPosition.Visible = true;
                cmbPosition.Visible = true;
                
                // Kích hoạt lại các nút thêm, sửa, xóa (nếu có quyền)
                if (PermissionHelper.CanManageEmployees())
                {
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }

            ValidateInputs();
        }
        #endregion

        #region UI Event Handlers
        private void btnAdd_Click(object sender, EventArgs e)
        {
            _isAddingNew = true;
            _selectedEmployeeId = null;
            foreach (Control control in flowLayoutPanelUsers.Controls)
            {
                if (control is ucUserItem item) item.SetSelected(false);
            }
            ClearDetailForm();
            SetDetailPanelEnabled(true, true);
            
            // Hiện lại tất cả các trường khi thêm mới
            lblDateOfBirth.Visible = true;
            dtpDateOfBirth.Visible = true;
            lblDepartment.Visible = true;
            cmbDepartment.Visible = true;
            lblGender.Visible = true;
            cmbGender.Visible = true;
            lblPosition.Visible = true;
            cmbPosition.Visible = true;
            
            // Kích hoạt các nút
            if (PermissionHelper.CanManageEmployees())
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            
            txtUserID.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedEmployeeId))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa thông tin.", "Chưa chọn nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _isAddingNew = false;
            SetDetailPanelEnabled(true, true);
            txtFullName.Focus();
            txtFullName.SelectAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedEmployeeId))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa.", "Chưa chọn nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn XÓA VĨNH VIỄN tài khoản của nhân viên '{txtFullName.Text}'?\nThao tác này không thể hoàn tác.", "Xác nhận xóa vĩnh viễn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                // Gọi phương thức DeleteUser mới trong BLL
                bool success = _adminBLL.DeleteUser(_selectedEmployeeId, out string errorMessage);
                if (success)
                {
                    MessageBox.Show("Xóa nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _selectedEmployeeId = null;
                    LoadFullEmployeeList();
                    ClearDetailForm();
                    SetDetailPanelEnabled(false, false);
                    // Thông báo các tab khác cập nhật
                    GlobalEventBus.RaiseEmployeeDataChanged();
                }
                else
                {
                    MessageBox.Show($"Xóa thất bại: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtUserID.Text) || string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || cmbPosition.SelectedValue == null || cmbDepartment.SelectedValue == null)
            {
                MessageBox.Show("Mã nhân viên, Họ tên, Email, Phòng ban và Chức vụ là các trường bắt buộc.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng email
            string email = txtEmail.Text.Trim();
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Email không đúng định dạng. Vui lòng nhập email hợp lệ (ví dụ: user@example.com).", "Email không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // Kiểm tra định dạng số điện thoại (nếu có nhập)
            string phone = txtPhoneNumber.Text.Trim();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!phone.All(char.IsDigit) || phone.Length < 10)
                {
                    MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại có ít nhất 10 chữ số.", "Số điện thoại không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhoneNumber.Focus();
                    return;
                }
            }

            // Kiểm tra độ dài mã nhân viên
            if (txtUserID.Text.Trim().Length < 3)
            {
                MessageBox.Show("Mã nhân viên phải có ít nhất 3 ký tự.", "Mã nhân viên không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserID.Focus();
                return;
            }

            // Kiểm tra độ dài họ tên
            if (txtFullName.Text.Trim().Length < 2)
            {
                MessageBox.Show("Họ tên phải có ít nhất 2 ký tự.", "Họ tên không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            var user = new UserDTO
            {
                Company_UserId = txtUserID.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Email = email,
                Phone = phone,
                DateOfBirth = dtpDateOfBirth.Value,
                Gender = cmbGender.SelectedItem?.ToString(),
                Address = txtCurrentAddress.Text.Trim(),
                RoleId = cmbPosition.SelectedValue.ToString(),
                DepartmentId = cmbDepartment.SelectedValue.ToString(),
                IsActive = true
            };

            bool success;
            string errorMessage;
            string currentId;

            if (_isAddingNew)
            {
                currentId = user.Company_UserId;
                success = _adminBLL.AddNewUser(user, out errorMessage);
            }
            else
            {
                user.Company_UserId = _selectedEmployeeId;
                currentId = _selectedEmployeeId;
                success = _adminBLL.UpdateUser(user, out errorMessage);
            }

            if (success)
            {
                MessageBox.Show(_isAddingNew ? "Thêm nhân viên thành công!" : "Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshAndSelectUser(currentId);
                // Thông báo các tab khác cập nhật
                GlobalEventBus.RaiseEmployeeDataChanged();
            }
            else
            {
                // Thông báo lỗi trả về từ BLL sẽ được hiển thị ở đây, giải quyết vấn đề crash.
                MessageBox.Show($"Thao tác thất bại: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _isAddingNew = false;
            if (!string.IsNullOrEmpty(_selectedEmployeeId))
            {
                DisplayEmployeeDetails(_selectedEmployeeId);
            }
            else
            {
                ClearDetailForm();
                SetDetailPanelEnabled(false, false);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // [FIX] Nếu text được thay đổi bởi code, không thực hiện tìm kiếm.
            if (_isUpdatingSearchBoxProgrammatically)
            {
                return;
            }

            string searchTerm = txtSearch.Text.ToLower().Trim();
            if (string.IsNullOrEmpty(searchTerm) || searchTerm == "tìm kiếm nhân viên...")
            {
                DisplayEmployees(_fullEmployeeList);
                return;
            }
            var filteredList = _fullEmployeeList
                .Where(u => (u.Company_UserId != null && u.Company_UserId.ToLower().Contains(searchTerm)) ||
                            (u.FullName != null && u.FullName.ToLower().Contains(searchTerm)))
                .ToList();
            DisplayEmployees(filteredList);
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm nhân viên...") { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                _isUpdatingSearchBoxProgrammatically = true;
                try
                {
                    txtSearch.Text = "Tìm kiếm nhân viên...";
                    txtSearch.ForeColor = Color.Gray;
                }
                finally
                {
                    _isUpdatingSearchBoxProgrammatically = false;
                }
            }
        }


        private void txtEmail_TextChanged(object sender, EventArgs e) => ValidateInputs();
        private void txtPhoneNumber_TextChanged(object sender, EventArgs e) => ValidateInputs();
        #endregion

        #region Helper Methods
        private void RefreshAndSelectUser(string userIdToSelect)
        {
            _selectedEmployeeId = userIdToSelect;
            LoadFullEmployeeList();
            DisplayEmployeeDetails(userIdToSelect);
            SetDetailPanelEnabled(true, false);
        }

        private void ClearDetailForm()
        {
            txtUserID.Text = "";
            txtFullName.Clear();
            dtpDateOfBirth.Value = DateTime.Now.AddYears(-22);
            txtPhoneNumber.Clear();
            txtEmail.Clear();
            cmbGender.SelectedIndex = 0;
            txtHometown.Clear();
            txtNationality.Text = "Việt Nam";
            txtCurrentAddress.Clear();
            if (cmbDepartment.Items.Count > 0) cmbDepartment.SelectedIndex = 0;
            if (cmbPosition.Items.Count > 0) cmbPosition.SelectedIndex = 0;
        }

        private void SetDetailPanelEnabled(bool isVisible, bool isEditable)
        {
            panelDetails.Visible = isVisible;

            var inputControls = tableLayoutPanelDetails.Controls.OfType<Control>()
                .Where(c => c is TextBox || c is ComboBox || c is DateTimePicker || c is Guna2TextBox || c is Guna2ComboBox || c is Guna2DateTimePicker);

            foreach (var control in inputControls)
            {
                if (control is TextBox text)
                {
                    text.ReadOnly = !isEditable;
                }
                else if (control is Guna2TextBox gunaText)
                {
                    gunaText.ReadOnly = !isEditable;
                }
                else
                {
                    control.Enabled = isEditable;
                }
            }

            btnSave.Enabled = isEditable;
            btnCancel.Enabled = isEditable;

            txtUserID.ReadOnly = !_isAddingNew;
            txtUserID.Enabled = isEditable || _isAddingNew;
            txtNationality.ReadOnly = true;
            txtNationality.Enabled = true;
        }

        private void ValidateInputs()
        {
            string email = txtEmail.Text;
            bool isEmailValid = string.IsNullOrWhiteSpace(email) || IsValidEmail(email);
            txtEmail.FillColor = isEmailValid ? Color.White : Color.MistyRose;
            string phone = txtPhoneNumber.Text;
            bool isPhoneValid = string.IsNullOrWhiteSpace(phone) || (phone.All(char.IsDigit) && phone.Length >= 10);
            txtPhoneNumber.FillColor = isPhoneValid ? Color.White : Color.MistyRose;
        }

        private bool IsValidEmail(string email)
        {
            try { var addr = new System.Net.Mail.MailAddress(email); return addr.Address == email; }
            catch { return false; }
        }
        #endregion

        private void splitContainerMain_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void panelDetails_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblUserID_Click(object sender, EventArgs e)
        {

        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            // Chỉ admin mới có thể ẩn/hiện
            if (!PermissionHelper.CanManageEmployees())
            {
                MessageBox.Show("Chỉ quản trị viên mới có quyền ẩn/hiện thông tin nhân viên.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chỉ ẩn/hiện nhân viên đang được chọn
            if (string.IsNullOrEmpty(_selectedEmployeeId))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để ẩn/hiện thông tin.", "Chưa chọn nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Toggle trạng thái ẩn/hiện cho nhân viên đang chọn
            bool currentState = _hiddenEmployees.ContainsKey(_selectedEmployeeId) && _hiddenEmployees[_selectedEmployeeId];
            _hiddenEmployees[_selectedEmployeeId] = !currentState;
            
            // Cập nhật lại danh sách để áp dụng trạng thái mới
            DisplayEmployees(_fullEmployeeList);
            
            // Cập nhật lại phần chi tiết
            DisplayEmployeeDetails(_selectedEmployeeId);
            
            // Cập nhật icon và tooltip dựa trên trạng thái mới
            bool newState = _hiddenEmployees[_selectedEmployeeId];
            if (newState)
            {
                btnHide.Image = Properties.Resources.eye_open;
                toolTip.SetToolTip(btnHide, "Hiện thông tin phụ");
            }
            else
            {
                btnHide.Image = Properties.Resources.eye_closed;
                toolTip.SetToolTip(btnHide, "Ẩn thông tin phụ");
            }
        }

        private void cmbLanguage_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}