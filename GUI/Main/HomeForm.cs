using BLL.User;
using BLL.Utils;
using DTO;
using DTO.User;
using GUI.Authentication;
using GUI;
using GUI.Main.User;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Collections.Generic; // Added for List
using System.Linq; // Added for Where and ToList

namespace GUI.Main
{
    public partial class HomeForm : Form
    {
        private UserDTO _currentUser;
        private ChangePasswordAnd2FASettings _changePasswordAnd2FAControl;
        private OverdueNotificationService _overdueService;
        private UserControl _activeControl; // control đang phủ lên Dashboard
        private string _currentLanguage = "vi-VN";// Ngôn ngữ mặc định



        public HomeForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            _overdueService = new OverdueNotificationService();
            ApplyLanguage(_currentLanguage);
            this.Load += HomeForm_Load; // Đảm bảo update số liệu dashboard sau khi form load
			GlobalEventBus.ContractDataChanged += OnContractDataChanged;
			this.FormClosed += (s, e) => { GlobalEventBus.ContractDataChanged -= OnContractDataChanged; };
            this.FormClosing += HomeForm_FormClosing; // Dispose _activeControl trước khi form đóng
        }

        private void HomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose _activeControl trước khi form đóng để tránh lỗi
            if (_activeControl != null)
            {
                try
                {
                    panelContent.Controls.Remove(_activeControl);
                    _activeControl.Dispose();
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không crash ứng dụng
                    System.Diagnostics.Debug.WriteLine($"Error disposing control on form closing: {ex.Message}");
                }
                finally
                {
                    _activeControl = null;
                }
            }
            
            // _overdueService không cần dispose vì không implement IDisposable
        }

        public HomeForm(UserDTO user) : this()
        {
            _currentUser = user;
            
            // Lưu user vào UserSession để PermissionHelper có thể sử dụng
            if (user != null)
            {
                UserSession.Instance.Login(user);
                Debug.WriteLine($"HomeForm: UserSession set - Department: {user.DepartmentName}, Role: {user.RoleName}");
            }
            
            // Kiểm tra hợp đồng quá hạn
            this.Load += (s, e) => {
                Debug.WriteLine("HomeForm_Load: Bắt đầu kiểm tra hợp đồng quá hạn...");
               
                Task.Run(() => {
                    try
                    {
                        Debug.WriteLine("Task.Run: Đang gọi CheckOverdueContracts()...");
                        CheckOverdueContracts();
                        Debug.WriteLine("Task.Run: Đã hoàn thành CheckOverdueContracts()");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"❌ Lỗi khi kiểm tra hợp đồng quá hạn: {ex.Message}");
                        Debug.WriteLine($"   StackTrace: {ex.StackTrace}");
                        Console.WriteLine($"❌ Lỗi khi kiểm tra hợp đồng quá hạn: {ex.Message}");
                        Console.WriteLine($"   StackTrace: {ex.StackTrace}");
                    }
                });
            };
            Debug.WriteLine($"HomeForm: User received: {_currentUser?.Company_Username}");
            DisplayUserInfo();
            UpdateDashboardCardsByCurrentMonth();
        }

        private void DisplayUserInfo()
        {
            if (lblUserInfo == null)
            {
                Debug.WriteLine("LỖI");
                return;
            }

            if (_currentUser != null)
            {
                string infoText = $"{_currentUser.FullName} ";
                lblUserInfo.Text = infoText;
                Debug.WriteLine($"HomeForm: Đang hiển thị thông tin người dùng: {infoText}");
            }
            else
            {
                lblUserInfo.Text = "Khách";
                Debug.WriteLine("HomeForm: Đang hiển thị 'Khách' vì _currentUser là null.");
            }
        }

        private void lblUserInfo_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("lblUserInfo đã được nhấp.");
        }


        private void btnAbout_Click(object sender, EventArgs e)
        {
            LoadContent(new About());
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            // Ẩn/loại bỏ control động để lộ Dashboard gốc (được đặt trong Designer)
            if (_activeControl != null)
            {
                try
                {
                    panelContent.Controls.Remove(_activeControl);
                    _activeControl.Dispose();
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không crash ứng dụng
                    System.Diagnostics.Debug.WriteLine($"Error disposing control on home click: {ex.Message}");
                }
                finally
                {
                    _activeControl = null;
                }
            }
        }

        private void LoadContent(UserControl uc)
        {
            // Không xóa Dashboard gốc; chỉ thêm control mới đè lên
            if (_activeControl != null)
            {
                try
                {
                    panelContent.Controls.Remove(_activeControl);
                    _activeControl.Dispose();
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không crash ứng dụng
                    System.Diagnostics.Debug.WriteLine($"Error disposing control: {ex.Message}");
                }
                finally
                {
                    _activeControl = null;
                }
            }
            uc.Dock = DockStyle.Fill;
            panelContent.Controls.Add(uc);
            uc.BringToFront();
            _activeControl = uc;
        }
        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Không có thông tin người dùng. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            LoadContent(new Profile(_currentUser));
        }
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Không thể đổi mật khẩu. Vui lòng đăng nhập để thực hiện chức năng này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _changePasswordAnd2FAControl = new ChangePasswordAnd2FASettings(_currentUser);

            _changePasswordAnd2FAControl.PasswordChangedSuccessfully += ChangePasswordAnd2FAControl_PasswordChangedSuccessfully;

            LoadContent(_changePasswordAnd2FAControl);
        }

        private void ChangePasswordAnd2FAControl_PasswordChangedSuccessfully(object sender, EventArgs e)
        {
            MessageBox.Show("Mật khẩu đã được đổi thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //_changePasswordAnd2FAControl.Hide();
            // LoadContent(new DashboardUserControl());
            // this.panelContent.Controls.Remove(_changePasswordAnd2FAControl);
            // _changePasswordAnd2FAControl.Dispose();
            // _changePasswordAnd2FAControl = null;
        }


        private void btnNotification_Click(object sender, EventArgs e)
        {
            LoadContent(new Notification());
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            LoadContent(new Statistics());
        }

        /// <summary>
        /// Kiểm tra và gửi thông báo hợp đồng quá hạn
        /// </summary>
        public void CheckOverdueContracts()
        {
            try
            {
                Debug.WriteLine("CheckOverdueContracts: Bắt đầu kiểm tra...");
                if (_overdueService == null)
                {
                    Debug.WriteLine("❌ _overdueService là null!");
                    Console.WriteLine("❌ _overdueService là null!");
                    return;
                }
                
                Debug.WriteLine("CheckOverdueContracts: Gọi CheckAndSendOverdueNotifications()...");
                _overdueService.CheckAndSendOverdueNotifications();
                Debug.WriteLine("CheckOverdueContracts: Đã hoàn thành CheckAndSendOverdueNotifications()");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Lỗi khi kiểm tra hợp đồng quá hạn: {ex.Message}");
                Debug.WriteLine($"   StackTrace: {ex.StackTrace}");
                Console.WriteLine($"❌ Lỗi khi kiểm tra hợp đồng quá hạn: {ex.Message}");
                Console.WriteLine($"   StackTrace: {ex.StackTrace}");
            }
        }

        private void UpdateDashboardCardsByCurrentMonth()
        {
            var contractBll = new BLL.User.ContractBLL();
            var allContracts = contractBll.GetAllContracts() ?? new List<ContractDTO>();
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var thisMonthContracts = allContracts
                .Where(c =>
                {
                    DateTime referenceDate = c.PlanDueDate ?? c.SignDate;
                    return referenceDate >= startOfMonth && referenceDate < endOfMonth;
                })
                .ToList();

            int countSuccess = thisMonthContracts.Count(c => IsCompletedStatus(c.Status));
            int countOverdue = thisMonthContracts.Count(c => IsOverdueContract(c, now));
            int countInProgress = thisMonthContracts.Count(c => !IsCompletedStatus(c.Status) && !IsOverdueContract(c, now));

            label11.Text = countSuccess.ToString();      // thành công
            label7.Text = countInProgress.ToString();    // đang thực hiện
            label8.Text = countOverdue.ToString();       // quá hạn
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            UpdateDashboardCardsByCurrentMonth();
            LoadRenewalPrediction();
        }

		private void OnContractDataChanged()
		{
			// Làm mới card dashboard khi dữ liệu hợp đồng thay đổi - ensure UI thread
			if (this.InvokeRequired)
			{
				this.Invoke(new Action(() => {
					UpdateDashboardCardsByCurrentMonth();
					LoadRenewalPrediction();
				}));
			}
			else
			{
				UpdateDashboardCardsByCurrentMonth();
				LoadRenewalPrediction();
			}
        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private bool IsCompletedStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return false;
            var normalized = status.Trim().ToLowerInvariant();
            return normalized.Contains("done") ||
                   normalized.Contains("success") ||
                   normalized.Contains("thành công") ||
                   normalized.Contains("completed") ||
                   normalized.Contains("hoàn tất") ||
                   normalized.Contains("đã hoàn thành");
        }

        private bool IsOverdueContract(ContractDTO contract, DateTime now)
        {
            if (contract == null || IsCompletedStatus(contract.Status)) return false;
            return contract.PlanDueDate.HasValue && contract.PlanDueDate.Value.Date < now.Date;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void ApplyLanguage(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            ResourceManager rm = new ResourceManager("GUI.Resources.Lang", typeof(HomeForm).Assembly);      
            btnHome.Text = rm.GetString("Menu_Home");
            btnProfile.Text = rm.GetString("Menu_PersonalInfo");
            btnChangePassword.Text = rm.GetString("Menu_ChangePassword");
            btnContractManagement.Text = rm.GetString("Menu_ContractManagement");
            btnAdminFunction.Text = rm.GetString("AdminFunctions");
            btnNotification.Text = rm.GetString("Menu_Notifications");
            btnStatistics.Text = rm.GetString("Menu_Statistics");
            btnAbout.Text = rm.GetString("Menu_About");
            btnLogout.Text = rm.GetString("Header_Logout");
            lblDashboard_Title.Text = rm.GetString("Dashboard_Title");
            lblDashboard_SuccessfulOrders.Text = rm.GetString("Dashboard_SuccessfulOrders");
            lblDashboard_InProgressOrders.Text = rm.GetString("Dashboard_InProgressOrders");
            lblDashboard_OverdueOrders.Text = rm.GetString("Dashboard_OverdueOrders");
            lblAIDeploy_Title.Text = rm.GetString("AIDeploy_Title");

            // Cập nhật hình ảnh cờ
            if (cultureName == "vi-VN")
            {
                pbLanguageSwitcher.Image = Properties.Resources.flag_vietnam;
            }
            else
            {
                pbLanguageSwitcher.Image = Properties.Resources.flag_uk;
            }
            if (_activeControl is About aboutControl)
            {
                aboutControl.ApplyLanguage(cultureName);
            }
            if (_activeControl is Profile profileControl)
            {
                profileControl.ApplyLanguage(cultureName);
            }
            if (_activeControl is ChangePasswordAnd2FASettings changePasswordControl)
            {
                changePasswordControl.ApplyLanguage(cultureName);
            }
            if (_activeControl is Notification notificationControl)
            {
                notificationControl.ApplyLanguage(cultureName);
            }
            if (_activeControl is Statistics statisticsControl)
            {
                statisticsControl.ApplyLanguage(cultureName);
            }
            //if (_activeControl is GUI.Admin.AdminManagementUI adminControl)
            //{
            //    adminControl.ApplyLanguage(cultureName);
            //}
            if (_activeControl is ucSalesContractManagement contractManagementControl)
            {
                contractManagementControl.ApplyLanguage(cultureName);
            }
        }
        private void pbLanguageSwitcher_Click_Click(object sender, EventArgs e)
        {
            if (_currentLanguage == "en-US")
            {
                _currentLanguage = "vi-VN";
            }
            else
            {
                _currentLanguage = "en-US";
            }
            ApplyLanguage(_currentLanguage);
        }

        private void btnAdminFunctions_Click(object sender, EventArgs e)
        {
            LoadContent(new GUI.Admin.AdminManagementUI());
        }

        private void btnContractManagement_Click_1(object sender, EventArgs e)
            // Tận dụng hàm LoadContent đã có sẵn để nạp giao diện quản lý hợp đồng
        {
            LoadContent(new ucSalesContractManagement());
        }

        private void LoadRenewalPrediction()
        {
            // Các chuỗi văn bản hiển thị
            string baseText = "• Khách hàng \"{0}\" có {1}% khả năng sẽ tái ký hợp đồng cho quý tiếp theo.";
            string noDataText = "Chưa có đủ dữ liệu để đưa ra dự đoán.";
            string errorText = "Đã xảy ra lỗi trong quá trình dự đoán.";

            try
            {
                // Sử dụng namespace đầy đủ để tránh nhầm lẫn
                var contractBll = new BLL.User.ContractBLL();
                var predictionBll = new BLL.User.PredictionBLL();

                // Lấy TOP 3 hợp đồng đã hoàn thành gần đây nhất
                var topContracts = contractBll.GetTopCompletedContracts(10);

                if (topContracts != null && topContracts.Count > 0)
                {
                    var predictions = new System.Text.StringBuilder();
                    int validPredictions = 0;

                    foreach (var contract in topContracts)
                    {
                        if (contract != null && !string.IsNullOrEmpty(contract.CustomerName))
                        {
                            var predictionResult = predictionBll.PredictRenewal(contract.ContractId);

                            if (predictionResult != null)
                            {
                                // Chuyển đổi "Score" thành xác suất (0-1) bằng hàm Sigmoid
                                float probabilityToRenew = 1 / (1 + (float)Math.Exp(-predictionResult.Score));
                                int percentageToRenew = (int)(probabilityToRenew * 100);

                                // Thêm vào danh sách dự đoán
                                if (validPredictions > 0)
                                {
                                    predictions.AppendLine();
                                }
                                predictions.Append(string.Format(baseText, contract.CustomerName, percentageToRenew));
                                validPredictions++;
                            }
                        }
                    }

                    if (validPredictions > 0)
                    {
                        // Cập nhật lên giao diện với danh sách các dự đoán
                        lblAIDeploy_Description.Text = predictions.ToString();
                    }
                    else
                    {
                        lblAIDeploy_Description.Text = noDataText;
                    }
                }
                else
                {
                    lblAIDeploy_Description.Text = noDataText;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AI Prediction Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"--- Inner Exception: {ex.InnerException.Message}");
                }
                lblAIDeploy_Description.Text = errorText;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn đăng xuất không?",
            "Xác nhận đăng xuất",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
    );

            if (result == DialogResult.Yes)
            {
                // Dispose _activeControl trước khi đăng xuất để tránh lỗi
                if (_activeControl != null)
                {
                    try
                    {
                        panelContent.Controls.Remove(_activeControl);
                        _activeControl.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error disposing control on logout: {ex.Message}");
                    }
                    finally
                    {
                        _activeControl = null;
                    }
                }

                // Xóa user khỏi UserSession
                UserSession.Instance.Logout();
                this.Hide(); // Ẩn HomeForm

                LoginForm loginForm = new LoginForm();
                loginForm.Show();

                this.Close(); // Bây giờ form sẽ đóng được
            }
        }
    }
}