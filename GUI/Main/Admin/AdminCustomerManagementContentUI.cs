using BLL.Admin;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
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
    public partial class AdminCustomerManagementContentUI : System.Windows.Forms.UserControl
    {
        private readonly AdminBLL _adminBLL = AdminBLL.Instance;
        private List<CustomerDTO> _fullCustomerList;
        private string _selectedCustomerId = null;
        private bool _isAddingNew = false;

        private GUI.VoskVoiceRecognizer recognizer;
        private bool isListening = false;

        // [FIX] Khai báo biến cờ để kiểm soát việc cập nhật text box một cách an toàn
        private bool _isUpdatingSearchBoxProgrammatically = false;
        
        // Sử dụng static để giữ trạng thái khi chuyển tab
        private static Dictionary<string, bool> _hiddenCustomers = new Dictionary<string, bool>(); // Lưu trạng thái ẩn/hiện cho từng khách hàng



        public AdminCustomerManagementContentUI()
        {
            InitializeComponent();
            // Subscribe to data change events
            GlobalEventBus.CustomerDataChanged += OnCustomerDataChanged;
            GlobalEventBus.OrderDataChanged += OnOrderDataChanged;
            this.Disposed += (s, e) =>
            {
                GlobalEventBus.CustomerDataChanged -= OnCustomerDataChanged;
                GlobalEventBus.OrderDataChanged -= OnOrderDataChanged;
            };
        }

        private void OnCustomerDataChanged()
        {
            // Refresh customer list when customer data changes
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshData));
            }
            else
            {
                RefreshData();
            }
        }

        private void OnOrderDataChanged()
        {
            // Refresh customer list when order data changes (orders might reference customers)
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
            // Refresh customer list and maintain current selection if possible
            string previousSelectedId = _selectedCustomerId;
            LoadFullCustomerList();
            
            // Restore selection if it still exists
            if (!string.IsNullOrEmpty(previousSelectedId))
            {
                var customer = _fullCustomerList?.FirstOrDefault(c => c.CustomerId == previousSelectedId);
                if (customer != null)
                {
                    DisplayCustomerDetails(previousSelectedId);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unsubscribe event handlers và clear flowLayoutPanelCustomers trước để tránh lỗi khi dispose
                try
                {
                    if (flowLayoutPanelCustomers != null && !flowLayoutPanelCustomers.IsDisposed && flowLayoutPanelCustomers.IsHandleCreated)
                    {
                        // Unsubscribe event handlers từ các ucCustomerItem trước khi clear
                        foreach (Control control in flowLayoutPanelCustomers.Controls)
                        {
                            if (control is ucCustomerItem item)
                            {
                                item.Click -= CustomerItem_Click;
                                item.Dispose();
                            }
                        }
                        flowLayoutPanelCustomers.Controls.Clear();
                    }
                }
                catch { }
                
                // Stop và dispose recognizer trước
                try
                {
                    if (isListening && recognizer != null)
                    {
                        recognizer.Stop();
                        isListening = false;
                    }
                    recognizer?.Dispose();
                    recognizer = null;
                }
                catch { }
                
                // Dispose components
                try
                {
                    if (components != null)
                    {
                        components.Dispose();
                        components = null;
                    }
                }
                catch { }
            }
            base.Dispose(disposing);
        }

        private void AdminCustomerManagementContentUI_Load(object sender, EventArgs e)
        {
            // Khôi phục tất cả các control về trạng thái hiển thị ban đầu
            RestoreAllControlsVisibility();
            
            LoadFullCustomerList();
            SetDetailPanelEnabled(false, false);

            InitializeVoiceSearch();
            txtSearch.Text = "Tìm kiếm khách hàng...";
            txtSearch.ForeColor = Color.Gray;
            ApplyPermissions();
            
            // Khởi tạo icon và tooltip cho nút Ẩn/Hiện
            if (btnHide != null)
            {
                btnHide.Image = Properties.Resources.eye_closed;
                toolTip.SetToolTip(btnHide, "Ẩn thông tin phụ của khách hàng đang chọn");
            }
            
            // Nếu có khách hàng đang được chọn và đang bị ẩn, hiển thị lại với trạng thái ẩn
            if (!string.IsNullOrEmpty(_selectedCustomerId))
            {
                bool isHidden = _hiddenCustomers.ContainsKey(_selectedCustomerId) && _hiddenCustomers[_selectedCustomerId];
                if (isHidden)
                {
                    DisplayCustomerDetails(_selectedCustomerId);
                }
            }
        }
        
        private void RestoreAllControlsVisibility()
        {
            // Khôi phục tất cả các label và control về trạng thái hiển thị
            if (lblCustomerName != null) lblCustomerName.Visible = true;
            if (txtCustomerName != null) txtCustomerName.Visible = true;
            if (lblRepName != null) lblRepName.Visible = true;
            if (txtRepName != null) txtRepName.Visible = true;
            if (lblRepEmail != null) lblRepEmail.Visible = true;
            if (txtRepEmail != null) txtRepEmail.Visible = true;
            if (lblRepPhone != null) lblRepPhone.Visible = true;
            if (txtRepPhone != null) txtRepPhone.Visible = true;
            if (lblAddress != null) lblAddress.Visible = true;
            if (txtAddress != null) txtAddress.Visible = true;
        }

        private void ApplyPermissions()
        {
            // Vô hiệu hóa nút thêm, sửa, xóa nếu không có quyền quản lý
            if (!PermissionHelper.CanManageData())
            {
                btnAdd.Enabled = false;
                btnAdd.Visible = false;
                btnEdit.Enabled = false;
                btnEdit.Visible = false;
                btnDelete.Enabled = false;
                btnDelete.Visible = false;
                if (btnHide != null)
                {
                    btnHide.Enabled = false;
                    btnHide.Visible = false;
                }
            }
        }

        #region Data Loading & Display
        private void LoadFullCustomerList()
        {
            _fullCustomerList = _adminBLL.GetAllCustomers();
            DisplayCustomers(_fullCustomerList);
        }

        private void DisplayCustomers(List<CustomerDTO> customersToDisplay)
        {
            flowLayoutPanelCustomers.Controls.Clear();
            if (customersToDisplay == null) return;

            foreach (var customer in customersToDisplay)
            {
                var item = new ucCustomerItem();
                item.SetData(customer.CustomerId, customer.CustomerName, customer.RepName, customer.RepPhone);
                item.Tag = customer.CustomerId;
                
                // Áp dụng trạng thái ẩn/hiện cho từng khách hàng
                bool isHidden = _hiddenCustomers.ContainsKey(customer.CustomerId) && _hiddenCustomers[customer.CustomerId];
                item.SetHidden(isHidden);

                item.Click += CustomerItem_Click;
                if (_selectedCustomerId != null && customer.CustomerId == _selectedCustomerId)
                {
                    item.SetSelected(true);
                }
                flowLayoutPanelCustomers.Controls.Add(item);
            }
        }

        private void CustomerItem_Click(object sender, EventArgs e)
        {
            var clickedItem = sender as ucCustomerItem;
            if (clickedItem == null) return;

            _isAddingNew = false;
            _selectedCustomerId = clickedItem.Tag.ToString();

            foreach (Control control in flowLayoutPanelCustomers.Controls)
            {
                if (control is ucCustomerItem item)
                {
                    item.SetSelected(item.Tag.ToString() == _selectedCustomerId);
                }
            }
            DisplayCustomerDetails(_selectedCustomerId);
            
            // Cập nhật icon và tooltip của nút Ẩn/Hiện dựa trên trạng thái của khách hàng đang chọn
            if (btnHide != null)
            {
                bool isHidden = _hiddenCustomers.ContainsKey(_selectedCustomerId) && _hiddenCustomers[_selectedCustomerId];
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
                if (PermissionHelper.CanManageData())
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
        }

        private void DisplayCustomerDetails(string customerId)
        {
            CustomerDTO customer = _adminBLL.GetCustomerDetails(customerId);
            if (customer == null)
            {
                MessageBox.Show("Không tìm thấy thông tin khách hàng.");
                ClearDetailForm();
                return;
            }

            SetDetailPanelEnabled(true, false);

            // Kiểm tra xem khách hàng này có đang bị ẩn không
            bool isHidden = _hiddenCustomers.ContainsKey(customerId) && _hiddenCustomers[customerId];

            if (isHidden)
            {
                // Nếu đang ẩn: chỉ hiển thị mã khách hàng, tất cả các trường khác để trống và ẩn
                txtCustomerId.Text = customer.CustomerId;
                txtCustomerName.Text = "";
                txtRepName.Text = "";
                txtRepEmail.Text = "";
                txtRepPhone.Text = "";
                txtAddress.Text = "";
                
                // Ẩn các label và control
                if (lblCustomerName != null) lblCustomerName.Visible = false;
                if (txtCustomerName != null) txtCustomerName.Visible = false;
                if (lblRepName != null) lblRepName.Visible = false;
                if (txtRepName != null) txtRepName.Visible = false;
                if (lblRepEmail != null) lblRepEmail.Visible = false;
                if (txtRepEmail != null) txtRepEmail.Visible = false;
                if (lblRepPhone != null) lblRepPhone.Visible = false;
                if (txtRepPhone != null) txtRepPhone.Visible = false;
                if (lblAddress != null) lblAddress.Visible = false;
                if (txtAddress != null) txtAddress.Visible = false;
                
                // Vô hiệu hóa các nút thêm, sửa, xóa
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                // Nếu không ẩn: hiển thị đầy đủ thông tin
                txtCustomerId.Text = customer.CustomerId;
                txtCustomerName.Text = customer.CustomerName;
                txtRepName.Text = customer.RepName;
                txtRepEmail.Text = customer.RepEmail;
                txtRepPhone.Text = customer.RepPhone;
                txtAddress.Text = customer.Address;
                
                // Hiện lại các label và control
                if (lblCustomerName != null) lblCustomerName.Visible = true;
                if (txtCustomerName != null) txtCustomerName.Visible = true;
                if (lblRepName != null) lblRepName.Visible = true;
                if (txtRepName != null) txtRepName.Visible = true;
                if (lblRepEmail != null) lblRepEmail.Visible = true;
                if (txtRepEmail != null) txtRepEmail.Visible = true;
                if (lblRepPhone != null) lblRepPhone.Visible = true;
                if (txtRepPhone != null) txtRepPhone.Visible = true;
                if (lblAddress != null) lblAddress.Visible = true;
                if (txtAddress != null) txtAddress.Visible = true;
                
                // Kích hoạt lại các nút thêm, sửa, xóa (nếu có quyền)
                if (PermissionHelper.CanManageData())
                {
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
        }
        #endregion

        #region UI Event Handlers
        private void btnAdd_Click(object sender, EventArgs e)
        {
            _isAddingNew = true;
            _selectedCustomerId = null;

            foreach (Control control in flowLayoutPanelCustomers.Controls)
            {
                if (control is ucCustomerItem item) item.SetSelected(false);
            }

            ClearDetailForm();
            SetDetailPanelEnabled(true, true);
            
            // Hiện lại tất cả các trường khi thêm mới
            if (lblCustomerName != null) lblCustomerName.Visible = true;
            if (txtCustomerName != null) txtCustomerName.Visible = true;
            if (lblRepName != null) lblRepName.Visible = true;
            if (txtRepName != null) txtRepName.Visible = true;
            if (lblRepEmail != null) lblRepEmail.Visible = true;
            if (txtRepEmail != null) txtRepEmail.Visible = true;
            if (lblRepPhone != null) lblRepPhone.Visible = true;
            if (txtRepPhone != null) txtRepPhone.Visible = true;
            if (lblAddress != null) lblAddress.Visible = true;
            if (txtAddress != null) txtAddress.Visible = true;
            
            // Kích hoạt các nút
            if (PermissionHelper.CanManageData())
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            
            // Tự sinh mã khách hàng và không cho sửa
            try
            {
                string nextId = _adminBLL.GetNextCustomerId();
                txtCustomerId.Text = nextId;
                txtCustomerId.ReadOnly = true;
            }
            catch { }
            txtCustomerName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedCustomerId))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.", "Chưa chọn khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _isAddingNew = false;
            SetDetailPanelEnabled(true, true);
            txtCustomerName.Focus();
            txtCustomerName.SelectAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedCustomerId))
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa.", "Chưa chọn khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{txtCustomerName.Text}'?\nThao tác này không thể hoàn tác và sẽ xóa vĩnh viễn khách hàng.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                bool success = _adminBLL.DeleteCustomer(_selectedCustomerId, out string errorMessage);
                if (success)
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _selectedCustomerId = null;
                    LoadFullCustomerList();
                    ClearDetailForm();
                    SetDetailPanelEnabled(false, false);
                    // Thông báo các tab khác cập nhật
                    GlobalEventBus.RaiseCustomerDataChanged();
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
            if (string.IsNullOrWhiteSpace(txtCustomerId.Text) || string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Mã khách hàng và Tên doanh nghiệp là bắt buộc.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng email (nếu có nhập)
            string repEmail = txtRepEmail.Text.Trim();
            if (!string.IsNullOrWhiteSpace(repEmail) && !IsValidContactEmail(repEmail))
            {
                MessageBox.Show("Email đại diện không đúng định dạng. Vui lòng nhập email hợp lệ (ví dụ: user@example.com).", "Email không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRepEmail.Focus();
                return;
            }

            // Kiểm tra định dạng số điện thoại (nếu có nhập)
            string repPhone = txtRepPhone.Text.Trim();
            if (!string.IsNullOrWhiteSpace(repPhone))
            {
                if (!IsValidContactPhone(repPhone))
                {
                    MessageBox.Show("Số điện thoại đại diện không hợp lệ. Vui lòng nhập số điện thoại bắt đầu bằng 0 hoặc +84 và có từ 9-11 chữ số.", "Số điện thoại không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRepPhone.Focus();
                    return;
                }
            }

            // Kiểm tra độ dài tên doanh nghiệp
            if (txtCustomerName.Text.Trim().Length < 2)
            {
                MessageBox.Show("Tên doanh nghiệp phải có ít nhất 2 ký tự.", "Tên doanh nghiệp không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerName.Focus();
                return;
            }

            var customer = new CustomerDTO
            {
                CustomerId = txtCustomerId.Text.Trim(),
                CustomerName = txtCustomerName.Text.Trim(),
                RepName = txtRepName.Text.Trim(),
                RepEmail = repEmail,
                RepPhone = repPhone,
                Address = txtAddress.Text.Trim()
            };

            bool success;
            string errorMessage;
            string currentId = customer.CustomerId;

            if (_isAddingNew)
            {
                success = _adminBLL.AddNewCustomer(customer, out errorMessage);
            }
            else
            {
                customer.CustomerId = _selectedCustomerId;
                currentId = _selectedCustomerId;
                success = _adminBLL.UpdateCustomer(customer, out errorMessage);
            }

            if (success)
            {
                MessageBox.Show(_isAddingNew ? "Thêm khách hàng thành công!" : "Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshAndSelectCustomer(currentId);
                // Thông báo các tab khác cập nhật
                GlobalEventBus.RaiseCustomerDataChanged();
            }
            else
            {
                MessageBox.Show($"Thao tác thất bại: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _isAddingNew = false;
            if (!string.IsNullOrEmpty(_selectedCustomerId))
            {
                DisplayCustomerDetails(_selectedCustomerId);
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
            if (string.IsNullOrEmpty(searchTerm) || searchTerm == "tìm kiếm khách hàng...")
            {
                DisplayCustomers(_fullCustomerList);
                return;
            }

            var filteredList = _fullCustomerList
                .Where(c => (c.CustomerId != null && c.CustomerId.ToLower().Contains(searchTerm)) ||
                            (c.CustomerName != null && c.CustomerName.ToLower().Contains(searchTerm)) ||
                            (c.RepName != null && c.RepName.ToLower().Contains(searchTerm)))
                .ToList();
            DisplayCustomers(filteredList);
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm khách hàng...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // [FIX] Sử dụng cờ để thay đổi text một cách an toàn
                _isUpdatingSearchBoxProgrammatically = true;
                try
                {
                    txtSearch.Text = "Tìm kiếm khách hàng...";
                    txtSearch.ForeColor = Color.Gray;
                }
                finally
                {
                    _isUpdatingSearchBoxProgrammatically = false;
                }
            }
        }
        #endregion

        #region Helper Methods
        private void RefreshAndSelectCustomer(string customerIdToSelect)
        {
            _selectedCustomerId = customerIdToSelect;
            LoadFullCustomerList();
            DisplayCustomerDetails(customerIdToSelect);
        }

        private void ClearDetailForm()
        {
            txtCustomerId.Clear();
            txtCustomerName.Clear();
            txtRepName.Clear();
            txtRepEmail.Clear();
            txtRepPhone.Clear();
            txtAddress.Clear();
        }

        private static readonly Regex EmailRegex = new Regex(
            @"^(?!\.)(?!.*\.\.)[A-Za-z0-9._%+\-]+@(?:[A-Za-z0-9-]+\.)+[A-Za-z]{2,}$",
            RegexOptions.Compiled);

        private bool IsValidContactEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string trimmed = email.Trim();
            if (trimmed.Length > 254) return false;
            return EmailRegex.IsMatch(trimmed);
        }

        private bool IsValidContactPhone(string phone)
        {
            string normalized = phone.Replace(" ", "");
            if (normalized.StartsWith("+84"))
            {
                normalized = "0" + normalized.Substring(3);
            }
            return Regex.IsMatch(normalized, @"^0\d{8,10}$");
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

            txtCustomerId.ReadOnly = !_isAddingNew;
            txtCustomerId.Enabled = isEditable || _isAddingNew;
        }
        #endregion

        private void panelLeftContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        #region TÍCH HỢP VOICE SEARCH: Toàn bộ logic nhận dạng giọng nói


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

        private void picMicrophoneIcon_Click(object sender, EventArgs e)
        {
            if (recognizer == null || !picMicrophoneIcon.Enabled) return;
            isListening = !isListening;
            if (isListening)
            {
                recognizer.Start();
                picMicrophoneIcon.BackColor = Color.LightCoral;

                // [FIX] Sử dụng cờ để thay đổi text một cách an toàn
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
                    // [FIX] Sử dụng cờ để thay đổi text một cách an toàn
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
                // [FIX] Sử dụng cờ để thay đổi text một cách an toàn
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

                // [FIX] Kích hoạt tìm kiếm thủ công sau khi đã cập nhật text
                txtSearch_TextChanged(txtSearch, EventArgs.Empty);

                if (isListening && recognizer != null)
                {
                    recognizer.Stop();
                    isListening = false;
                    picMicrophoneIcon.BackColor = Color.Transparent;
                }
            });
        }
        #endregion

        private void btnHide_Click(object sender, EventArgs e)
        {
            // Chỉ Admin và Phòng Kinh Doanh mới có thể ẩn/hiện
            if (!PermissionHelper.CanManageData())
            {
                MessageBox.Show("Chỉ quản trị viên và phòng kinh doanh mới có quyền ẩn/hiện thông tin khách hàng.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chỉ ẩn/hiện khách hàng đang được chọn
            if (string.IsNullOrEmpty(_selectedCustomerId))
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để ẩn/hiện thông tin.", "Chưa chọn khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Toggle trạng thái ẩn/hiện cho khách hàng đang chọn
            bool currentState = _hiddenCustomers.ContainsKey(_selectedCustomerId) && _hiddenCustomers[_selectedCustomerId];
            _hiddenCustomers[_selectedCustomerId] = !currentState;
            
            // Cập nhật lại danh sách để áp dụng trạng thái mới
            DisplayCustomers(_fullCustomerList);
            
            // Cập nhật lại phần chi tiết
            DisplayCustomerDetails(_selectedCustomerId);
            
            // Cập nhật icon và tooltip dựa trên trạng thái mới
            bool newState = _hiddenCustomers[_selectedCustomerId];
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

        private void btnHide_Click_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanelDetails_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}