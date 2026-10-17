using BLL.Admin;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using ComboBox = System.Windows.Forms.ComboBox;
using Control = System.Windows.Forms.Control;
using TextBox = System.Windows.Forms.TextBox;
using UserControl = System.Windows.Forms.UserControl;
using Guna.UI2.WinForms;
using GUI;
using GUI.Helpers;

namespace GUI.Admin
{
    public partial class AdminOrderManagementContentUI : UserControl
    {
        private readonly AdminBLL _adminBLL = AdminBLL.Instance;
        private List<OrderDTO> _fullOrderList;
        private string _selectedOrderId = null;
        private bool _isAddingNew = false;

        private GUI.VoskVoiceRecognizer recognizer;
        private bool isListening = false;
        private bool _isUpdatingSearchBoxProgrammatically = false;

        // Sử dụng static để giữ trạng thái khi chuyển tab
        private static Dictionary<string, bool> _hiddenOrders = new Dictionary<string, bool>(); // Lưu trạng thái ẩn/hiện cho từng đơn hàng

        private class Language
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        public AdminOrderManagementContentUI()
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
            // Refresh order list when customer data changes (to update customer names)
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
            // Refresh order list when order data changes
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
            // Refresh order list and maintain current selection if possible
            string previousSelectedId = _selectedOrderId;
            LoadFullOrderList();
            
            // Restore selection if it still exists
            if (!string.IsNullOrEmpty(previousSelectedId))
            {
                var order = _fullOrderList?.FirstOrDefault(o => o.OrderId == previousSelectedId);
                if (order != null)
                {
                    DisplayOrderDetails(previousSelectedId);
                }
            }
        }

        private void AdminOrderManagementContentUI_Load(object sender, EventArgs e)
        {
            // Khôi phục tất cả các control về trạng thái hiển thị ban đầu
            RestoreAllControlsVisibility();

            LoadFullOrderList();
            SetDetailPanelEnabled(false, false);
            InitializeVoiceSearch();
            txtSearch.Text = "Tìm kiếm đơn hàng...";
            txtSearch.ForeColor = Color.Gray;

            // Cấu hình DateTimePicker để có thể hiển thị trống
            dtpSampleStartDate.Format = DateTimePickerFormat.Custom;
            dtpSampleStartDate.CustomFormat = "dd/MM/yyyy";
            dtpExpectedReturnDate.Format = DateTimePickerFormat.Custom;
            dtpExpectedReturnDate.CustomFormat = "dd/MM/yyyy";

            // Trạng thái đơn hàng
            InitializeStatusCombo();

            // Cho phép click để hiển thị lịch nếu đang trống
            dtpSampleStartDate.ValueChanged += (s, ev) => { dtpSampleStartDate.CustomFormat = "dd/MM/yyyy"; };
            dtpSampleStartDate.DropDown += (s, ev) => { dtpSampleStartDate.CustomFormat = "dd/MM/yyyy"; };
            dtpExpectedReturnDate.ValueChanged += (s, ev) => { dtpExpectedReturnDate.CustomFormat = "dd/MM/yyyy"; };
            dtpExpectedReturnDate.DropDown += (s, ev) => { dtpExpectedReturnDate.CustomFormat = "dd/MM/yyyy"; };

            ApplyPermissions();

            // Khởi tạo icon và tooltip cho nút Ẩn/Hiện
            if (btnHide != null)
            {
                btnHide.Image = Properties.Resources.eye_closed;
                toolTip.SetToolTip(btnHide, "Ẩn thông tin phụ của đơn hàng đang chọn");
            }

            // Nếu có đơn hàng đang được chọn và đang bị ẩn, hiển thị lại với trạng thái ẩn
            if (!string.IsNullOrEmpty(_selectedOrderId))
            {
                bool isHidden = _hiddenOrders.ContainsKey(_selectedOrderId) && _hiddenOrders[_selectedOrderId];
                if (isHidden)
                {
                    DisplayOrderDetails(_selectedOrderId);
                }
            }
        }

        private void RestoreAllControlsVisibility()
        {
            // Khôi phục tất cả các label và control về trạng thái hiển thị
            if (lblSampleStartDate != null) lblSampleStartDate.Visible = true;
            if (dtpSampleStartDate != null) dtpSampleStartDate.Visible = true;
            if (lblExpectedReturnDate != null) lblExpectedReturnDate.Visible = true;
            if (dtpExpectedReturnDate != null) dtpExpectedReturnDate.Visible = true;
            if (lblStatus != null) lblStatus.Visible = true;
            if (cmbStatus != null) cmbStatus.Visible = true;
            if (lblPersonInCharge != null) lblPersonInCharge.Visible = true;
            if (txtPersonInCharge != null) txtPersonInCharge.Visible = true;
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
        private void LoadFullOrderList()
        {
            try
            {
                _fullOrderList = _adminBLL.GetAllOrders();
                DisplayOrders(_fullOrderList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _fullOrderList = new List<OrderDTO>();
            }
        }

        private void DisplayOrders(List<OrderDTO> ordersToDisplay)
        {
            flowLayoutPanelOrders.Controls.Clear();
            if (ordersToDisplay == null) return;

            foreach (var order in ordersToDisplay)
            {
                var item = new ucOrderItem();
                item.SetData(order.OrderId, order.CustomerId, order.CustomerName, order.CustomerContactName);
                item.Tag = order.OrderId;

                // Áp dụng trạng thái ẩn/hiện cho từng đơn hàng
                bool isHidden = _hiddenOrders.ContainsKey(order.OrderId) && _hiddenOrders[order.OrderId];
                item.SetHidden(isHidden);

                item.Click += OrderItem_Click;
                if (_selectedOrderId != null && order.OrderId == _selectedOrderId)
                {
                    item.SetSelected(true);
                }
                flowLayoutPanelOrders.Controls.Add(item);
            }
        }

        private void OrderItem_Click(object sender, EventArgs e)
        {
            var clickedItem = sender as ucOrderItem;
            if (clickedItem == null) return;

            _isAddingNew = false;
            _selectedOrderId = clickedItem.Tag.ToString();

            foreach (Control control in flowLayoutPanelOrders.Controls)
            {
                if (control is ucOrderItem item)
                {
                    item.SetSelected(item.Tag.ToString() == _selectedOrderId);
                }
            }
            DisplayOrderDetails(_selectedOrderId);

            // Cập nhật icon và tooltip của nút Ẩn/Hiện dựa trên trạng thái của đơn hàng đang chọn
            if (btnHide != null)
            {
                bool isHidden = _hiddenOrders.ContainsKey(_selectedOrderId) && _hiddenOrders[_selectedOrderId];
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

        private void DisplayOrderDetails(string orderId)
        {
            OrderDTO order = null;
            try
            {
                order = _adminBLL.GetOrderDetails(orderId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (order == null)
            {
                MessageBox.Show("Không tìm thấy thông tin chi tiết cho đơn hàng này.");
                ClearDetailForm();
                SetDetailPanelEnabled(false, false);
                return;
            }

            SetDetailPanelEnabled(true, false);

            // Kiểm tra xem đơn hàng này có đang bị ẩn không
            bool isHidden = _hiddenOrders.ContainsKey(orderId) && _hiddenOrders[orderId];

            if (isHidden)
            {
                // Nếu đang ẩn: chỉ hiển thị mã đơn và mã khách, tất cả các trường khác để trống và ẩn
                txtOrderId.Text = order.OrderId;
                txtCustomerId.Text = order.CustomerId;

                // Tất cả các trường khác để trống
                dtpSampleStartDate.CustomFormat = " ";
                dtpExpectedReturnDate.CustomFormat = " ";
                cmbStatus.SelectedIndex = -1;
                txtPersonInCharge.Text = "";

                // Ẩn các label và control
                if (lblSampleStartDate != null) lblSampleStartDate.Visible = false;
                if (dtpSampleStartDate != null) dtpSampleStartDate.Visible = false;
                if (lblExpectedReturnDate != null) lblExpectedReturnDate.Visible = false;
                if (dtpExpectedReturnDate != null) dtpExpectedReturnDate.Visible = false;
                if (lblStatus != null) lblStatus.Visible = false;
                if (cmbStatus != null) cmbStatus.Visible = false;
                if (lblPersonInCharge != null) lblPersonInCharge.Visible = false;
                if (txtPersonInCharge != null) txtPersonInCharge.Visible = false;

                // Vô hiệu hóa các nút thêm, sửa, xóa
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                // Nếu không ẩn: hiển thị đầy đủ thông tin
                txtOrderId.Text = order.OrderId;
                txtCustomerId.Text = order.CustomerId;

                // Nếu ngày tháng hợp lệ, hiển thị nó. Nếu không, hiển thị trống.
                if (order.SampleStartDate > DateTime.MinValue.AddYears(1))
                {
                    dtpSampleStartDate.CustomFormat = "dd/MM/yyyy";
                    dtpSampleStartDate.Value = order.SampleStartDate;
                }
                else
                {
                    dtpSampleStartDate.CustomFormat = " ";
                }

                if (order.ExpectedReturnDate > DateTime.MinValue.AddYears(1))
                {
                    dtpExpectedReturnDate.CustomFormat = "dd/MM/yyyy";
                    dtpExpectedReturnDate.Value = order.ExpectedReturnDate;
                }
                else
                {
                    dtpExpectedReturnDate.CustomFormat = " ";
                }

                // order.Status đã là tiếng Việt từ BLL
                SelectStatusInCombo(order.Status);
                txtPersonInCharge.Text = order.PersonInCharge;

                // Hiện lại các label và control
                if (lblSampleStartDate != null) lblSampleStartDate.Visible = true;
                if (dtpSampleStartDate != null) dtpSampleStartDate.Visible = true;
                if (lblExpectedReturnDate != null) lblExpectedReturnDate.Visible = true;
                if (dtpExpectedReturnDate != null) dtpExpectedReturnDate.Visible = true;
                if (lblStatus != null) lblStatus.Visible = true;
                if (cmbStatus != null) cmbStatus.Visible = true;
                if (lblPersonInCharge != null) lblPersonInCharge.Visible = true;
                if (txtPersonInCharge != null) txtPersonInCharge.Visible = true;

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
            _selectedOrderId = null;
            foreach (Control control in flowLayoutPanelOrders.Controls)
            {
                if (control is ucOrderItem item) item.SetSelected(false);
            }
            ClearDetailForm();
            SetDetailPanelEnabled(true, true);

            // Hiện lại tất cả các trường khi thêm mới
            if (lblSampleStartDate != null) lblSampleStartDate.Visible = true;
            if (dtpSampleStartDate != null) dtpSampleStartDate.Visible = true;
            if (lblExpectedReturnDate != null) lblExpectedReturnDate.Visible = true;
            if (dtpExpectedReturnDate != null) dtpExpectedReturnDate.Visible = true;
            if (lblStatus != null) lblStatus.Visible = true;
            if (cmbStatus != null) cmbStatus.Visible = true;
            if (lblPersonInCharge != null) lblPersonInCharge.Visible = true;
            if (txtPersonInCharge != null) txtPersonInCharge.Visible = true;

            // Kích hoạt các nút
            if (PermissionHelper.CanManageData())
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }

            // Sinh mã hợp đồng tự động và khóa không cho sửa
            try
            {
                txtOrderId.Text = _adminBLL.GetNextContractId();
                txtOrderId.ReadOnly = true;
            }
            catch { txtOrderId.ReadOnly = true; }
            SelectStatusInCombo("Đang thực hiện"); // mặc định
            dtpSampleStartDate.CustomFormat = " "; // không nhập ở đây
            dtpExpectedReturnDate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedOrderId))
            {
                MessageBox.Show("Hãy chọn một hợp đồng để sửa.");
                return;
            }
            _isAddingNew = false;
            SetDetailPanelEnabled(true, true);
            txtOrderId.ReadOnly = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedOrderId))
            {
                MessageBox.Show("Hãy chọn một hợp đồng để xóa.");
                return;
            }
            var confirm = MessageBox.Show($"Bạn chắc chắn muốn xóa hợp đồng '{_selectedOrderId}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            string error;
            if (_adminBLL.DeleteOrder(_selectedOrderId, out error))
            {
                MessageBox.Show("Đã xóa hợp đồng.");
                _selectedOrderId = null;
                ClearDetailForm();
                SetDetailPanelEnabled(false, false);
                LoadFullOrderList();
            }
            else
            {
                MessageBox.Show("Xóa thất bại: " + error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool hasExpectedDate = !string.IsNullOrWhiteSpace(dtpExpectedReturnDate.Text) && dtpExpectedReturnDate.Text.Trim() != "";
            if (hasExpectedDate)
            {
                dtpExpectedReturnDate.CustomFormat = "dd/MM/yyyy";
            }

            var dto = new OrderDTO
            {
                OrderId = txtOrderId.Text?.Trim(),
                CustomerId = txtCustomerId.Text?.Trim(),
                // SampleStartDate không nhập từ form này
                ExpectedReturnDate = hasExpectedDate ? dtpExpectedReturnDate.Value : DateTime.MinValue,
                Status = cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : null,
                // PersonInCharge: chấp nhận nhập mã NV (Company_UserId) hoặc để trống
                PersonInCharge = string.IsNullOrWhiteSpace(txtPersonInCharge.Text) ? null : txtPersonInCharge.Text.Trim()
            };

            string error;
            if (_isAddingNew)
            {
                if (_adminBLL.AddNewOrder(dto, out error))
                {
                    MessageBox.Show("Đã thêm hợp đồng mới.");
                    _isAddingNew = false;
                    RefreshAndSelectOrder(dto.OrderId);
                    SetDetailPanelEnabled(true, false);
                    // Thông báo UI cập nhật
                    GlobalEventBus.RaiseContractDataChanged();
                    GlobalEventBus.RaiseOrderDataChanged();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại: " + error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(dto.OrderId))
                {
                    MessageBox.Show("Thiếu mã hợp đồng.");
                    return;
                }
                if (_adminBLL.UpdateOrder(dto, out error))
                {
                    MessageBox.Show("Đã cập nhật hợp đồng.");
                    RefreshAndSelectOrder(dto.OrderId);
                    SetDetailPanelEnabled(true, false);
                    // Thông báo UI cập nhật
                    GlobalEventBus.RaiseContractDataChanged();
                    GlobalEventBus.RaiseOrderDataChanged();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại: " + error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _isAddingNew = false;
            if (!string.IsNullOrEmpty(_selectedOrderId))
            {
                DisplayOrderDetails(_selectedOrderId);
            }
            else
            {
                ClearDetailForm();
                SetDetailPanelEnabled(false, false);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingSearchBoxProgrammatically) return;

            string searchTerm = txtSearch.Text.ToLower().Trim();
            if (string.IsNullOrEmpty(searchTerm) || searchTerm == "tìm kiếm đơn hàng...")
            {
                DisplayOrders(_fullOrderList);
                return;
            }

            var filteredList = _fullOrderList
                .Where(o => (o.OrderId != null && o.OrderId.ToLower().Contains(searchTerm)) ||
                            (o.CustomerId != null && o.CustomerId.ToLower().Contains(searchTerm)) ||
                            (!string.IsNullOrEmpty(o.CustomerName) && o.CustomerName.ToLower().Contains(searchTerm)) ||
                            (!string.IsNullOrEmpty(o.CustomerContactName) && o.CustomerContactName.ToLower().Contains(searchTerm)))
                .ToList();
            DisplayOrders(filteredList);
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm đơn hàng...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                _isUpdatingSearchBoxProgrammatically = true;
                try
                {
                    txtSearch.Text = "Tìm kiếm đơn hàng...";
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
        private void RefreshAndSelectOrder(string orderIdToSelect)
        {
            _selectedOrderId = orderIdToSelect;
            LoadFullOrderList();
            DisplayOrderDetails(orderIdToSelect);
        }

        private void ClearDetailForm()
        {
            txtOrderId.Clear();
            txtCustomerId.Clear();
            dtpSampleStartDate.CustomFormat = " ";
            dtpExpectedReturnDate.CustomFormat = " ";
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = -1;
            txtPersonInCharge.Clear();
        }

        private void SetDetailPanelEnabled(bool isVisible, bool isEditable)
        {
            panelDetails.Visible = isVisible;

            var inputControls = tableLayoutPanelDetails.Controls.OfType<Control>()
                .Where(c => c is TextBox || c is ComboBox || c is DateTimePicker || c is Guna2TextBox || c is Guna2ComboBox || c is Guna2DateTimePicker);

            foreach (var control in inputControls)
            {
                if (control is TextBox textBox)
                {
                    textBox.ReadOnly = !isEditable;
                }
                else if (control is Guna2TextBox gunaTextBox)
                {
                    gunaTextBox.ReadOnly = !isEditable;
                }
                else
                {
                    control.Enabled = isEditable;
                }
            }

            btnSave.Enabled = isEditable;
            btnCancel.Enabled = isEditable;
        }
        #endregion

        #region TÍCH HỢP VOICE SEARCH
        private void InitializeStatusCombo()
        {
            var statuses = new List<string>
            {
                "Đang thực hiện",
                "Đã hoàn thành",
                "Đã hủy",
                "Đã lên kế hoạch"
            };
            cmbStatus.DataSource = statuses;
            cmbStatus.SelectedIndex = -1;
        }

        private void SelectStatusInCombo(string statusText)
        {
            if (string.IsNullOrWhiteSpace(statusText))
            {
                cmbStatus.SelectedIndex = -1;
                return;
            }
            int idx = cmbStatus.FindStringExact(statusText);
            cmbStatus.SelectedIndex = idx >= 0 ? idx : -1;
        }

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
                        txtSearch_Leave(sender, e);
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

        private void lblOrderId_Click(object sender, EventArgs e)
        {

        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            // Chỉ Admin và Phòng Kinh Doanh mới có thể ẩn/hiện
            if (!PermissionHelper.CanManageData())
            {
                MessageBox.Show("Chỉ quản trị viên và phòng kinh doanh mới có quyền ẩn/hiện thông tin đơn hàng.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chỉ ẩn/hiện đơn hàng đang được chọn
            if (string.IsNullOrEmpty(_selectedOrderId))
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để ẩn/hiện thông tin.", "Chưa chọn đơn hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Toggle trạng thái ẩn/hiện cho đơn hàng đang chọn
            bool currentState = _hiddenOrders.ContainsKey(_selectedOrderId) && _hiddenOrders[_selectedOrderId];
            _hiddenOrders[_selectedOrderId] = !currentState;

            // Cập nhật lại danh sách để áp dụng trạng thái mới
            DisplayOrders(_fullOrderList);

            // Cập nhật lại phần chi tiết
            DisplayOrderDetails(_selectedOrderId);

            // Cập nhật icon và tooltip dựa trên trạng thái mới
            bool newState = _hiddenOrders[_selectedOrderId];
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
    }
}