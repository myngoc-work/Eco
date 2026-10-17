using BLL;
using DTO;
using OtpNet;
using QRCoder;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
namespace GUI.Main.User
{
    public partial class ChangePasswordAnd2FASettings : UserControl
    {
        private readonly UserBLL _userBLL;
        private UserDTO _currentUser;
        public event EventHandler PasswordChangedSuccessfully;
        private string _tempTwoFactorSecret;
        public ChangePasswordAnd2FASettings(UserDTO user)
        {
            InitializeComponent();
            _userBLL = new UserBLL();
            _currentUser = user;

            txtCurrentPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmNewPassword.UseSystemPasswordChar = true;

            this.Load += (s, e) => txtCurrentPassword.Focus();

            switchEnable2FA.CheckedChanged += SwitchEnable2FA_CheckedChanged;

            txtTOTPCodeSetup.Visible = false;
            btnSubmit2FA.Visible = false;
            pbQRCode.Visible = false;
            lblTOTPCodeSetup.Visible = false;
            lbl4FA.Visible = false;
            lbl2FAStatus.Visible = true;
            lbl2FAStatus.Text = "Xác thực 2 yếu tố: Đang tải...";
            SetStepLabelsVisibility(false);

            pbToggleCurrentPasswordVisibility.Image = Properties.Resources.eye_closed;
            pbToggleCurrentPasswordVisibility.Image = Properties.Resources.eye_closed;
            pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_closed;
            ApplyLanguage(Thread.CurrentThread.CurrentUICulture.Name);

        }
        //ngon ngu
        public void ApplyLanguage(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            ResourceManager rm = new ResourceManager("GUI.Resources.Lang", typeof(HomeForm).Assembly);
            label2.Text = rm.GetString("label2");
            label5.Text = rm.GetString("label5");
            label6.Text = rm.GetString("label6");
            label7.Text = rm.GetString("label7");
            lblTOTPCodeSetup.Text = rm.GetString("TOTPCodeSetup");
            lbl4FA.Text = rm.GetString("lbl4FA");
            btnSubmit2FA.Text = rm.GetString("btnSubmit2FA");
            tabPage1.Text = rm.GetString("tabPage1");
            tabPage2FA.Text = rm.GetString("tabPage2FA");
            label1.Text = rm.GetString("label1");
            lblUsername.Text = rm.GetString("Username");
            label3.Text = rm.GetString("label3");
            label4.Text = rm.GetString("label4");
            btnConfirmChange.Text = rm.GetString("ConfirmChange");


        }
        public void UpdateUser(UserDTO newUser)
        {
            _currentUser = newUser;
            ClearInputFields();
            LoadTwoFactorSettings();
        }
        private void SetStepLabelsVisibility(bool visible)
        {
            pbQRCode.Visible = visible;
            txtTOTPCodeSetup.Visible = visible;
            lblTOTPCodeSetup.Visible = visible; 
            btnSubmit2FA.Visible = visible;
            lbl4FA.Visible = visible; 
        }
        private void LoadTwoFactorSettings()
        {
            if (_currentUser != null)
            {
                switchEnable2FA.Checked = _currentUser.IsTwoFactorEnabled;
                UpdateUIForTwoFactorSettings(_currentUser.IsTwoFactorEnabled);
            }
            else
            {
                switchEnable2FA.Checked = false;
                UpdateUIForTwoFactorSettings(false);
            }
        }

        private void UpdateUIForTwoFactorSettings(bool isEnabled)
        {
            if (isEnabled)
            {
                SetStepLabelsVisibility(false);
                lbl2FAStatus.Text = "Xác thực 2 yếu tố: ĐANG BẬT";
                switchEnable2FA.Checked = true;
                _tempTwoFactorSecret = null;
            }
            else
            {
                SetStepLabelsVisibility(false);
                lbl2FAStatus.Text = "Xác thực 2 yếu tố: ĐANG TẮT";
                switchEnable2FA.Checked = false;
                _tempTwoFactorSecret = null; // Clear any temporary secret
            }
            lbl2FAStatus.Visible = true;
        }
        private void btnConfirmChange_Click_1(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Không có thông tin người dùng để đổi mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmNewPassword = txtConfirmNewPassword.Text;

            // Xác thực cơ bản cho các trường rỗng
            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmNewPassword))
            {
                //MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string errorMessage;
            bool success = _userBLL.ChangePassword(
                _currentUser.Company_UserId,
                currentPassword,
                newPassword,
                confirmNewPassword,
                out errorMessage
            );

            if (success)
            {
                MessageBox.Show("Mật khẩu đã được thay đổi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearInputFields();
                PasswordChangedSuccessfully?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show(errorMessage, "Lỗi đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearInputFields()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmNewPassword.Text = "";
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }

        private void SwitchEnable2FA_CheckedChanged(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Không có thông tin người dùng để quản lý 2FA.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                switchEnable2FA.Checked = false;
                UpdateUIForTwoFactorSettings(false);
                return;
            }

            if (switchEnable2FA.Checked)
            {
                if (_currentUser.IsTwoFactorEnabled)
                {
                    UpdateUIForTwoFactorSettings(true);
                    return;
                }

                lbl2FAStatus.Text = "Đang thiết lập 2FA...";

                if (string.IsNullOrWhiteSpace(_tempTwoFactorSecret))
                {
                    _tempTwoFactorSecret = _userBLL.GenerateTwoFactorSecret();
                }

                string issuer = "EcoGreen";
                string username = _currentUser.Company_Username;
                string qrCodeUri = _userBLL.GetQRCodeUri(_tempTwoFactorSecret, username, issuer);

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
                    using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                    {
                        byte[] qrCodeAsPngBytes = qrCode.GetGraphic(20);
                        using (MemoryStream ms = new MemoryStream(qrCodeAsPngBytes))
                        {
                            pbQRCode.Image = new Bitmap(ms);
                        }
                    }
                }

                SetStepLabelsVisibility(true);
                txtTOTPCodeSetup.Clear();
                txtTOTPCodeSetup.Focus();
            }
            else
            {
                if (_currentUser.IsTwoFactorEnabled)
                {
                    DialogResult confirm = MessageBox.Show(
                        "Bạn có chắc chắn muốn TẮT xác thực 2 yếu tố không? Điều này sẽ giảm bảo mật cho tài khoản của bạn.",
                        "Xác nhận vô hiệu hóa 2FA",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirm == DialogResult.Yes)
                    {
                        bool success = _userBLL.DisableTwoFactor(_currentUser.Company_UserId);
                        if (success)
                        {
                            _currentUser.IsTwoFactorEnabled = false;
                            _currentUser.TwoFactorSecret = null; 
                            MessageBox.Show("Xác thực 2 yếu tố đã được vô hiệu hóa thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UpdateUIForTwoFactorSettings(false);
                        }
                        else
                        {
                            MessageBox.Show("Không thể vô hiệu hóa xác thực 2 yếu tố. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            switchEnable2FA.Checked = true;
                        }
                    }
                    else
                    {
                        switchEnable2FA.Checked = true; 
                    }
                }
                else
                {
                    // Nếu 2FA chưa bật và người dùng tắt switch (ví dụ: trong quá trình thiết lập bị hủy)
                    UpdateUIForTwoFactorSettings(false);
                }
            }
        }

        private void btnSubmit2FA_Click(object sender, EventArgs e)
        {
            if (_currentUser == null || string.IsNullOrWhiteSpace(_currentUser.Company_UserId) || string.IsNullOrWhiteSpace(_tempTwoFactorSecret))
            {
                //MessageBox.Show("Có lỗi xảy ra trong quá trình thiết lập 2FA. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string enteredCode = txtTOTPCodeSetup.Text.Trim();
            if (string.IsNullOrWhiteSpace(enteredCode))
            {
                MessageBox.Show("Vui lòng nhập mã xác thực từ Google Authenticator.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTOTPCodeSetup.Focus();
                return;
            }


            bool isValid = _userBLL.VerifyTwoFactorCodeWithSecret(_tempTwoFactorSecret, enteredCode);

            if (isValid)
            {

                bool success = _userBLL.EnableTwoFactor(_currentUser.Company_UserId, _tempTwoFactorSecret);

                if (success)
                {
                    _currentUser.IsTwoFactorEnabled = true;
                    _currentUser.TwoFactorSecret = _tempTwoFactorSecret; 
                    MessageBox.Show("Xác thực 2 yếu tố đã được kích hoạt thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    UpdateUIForTwoFactorSettings(true); 
                }
                else
                {
                    MessageBox.Show("Không thể lưu cài đặt 2FA. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Mã xác thực không đúng. Vui lòng kiểm tra lại mã trên Google Authenticator và đảm bảo thời gian trên điện thoại của bạn chính xác.", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTOTPCodeSetup.Clear();
                txtTOTPCodeSetup.Focus();
            }
        }

        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void pbToggleCurrentPasswordVisibility_Click(object sender, EventArgs e)
        {
            if (txtCurrentPassword.UseSystemPasswordChar)
            {
                txtCurrentPassword.UseSystemPasswordChar = false;
                pbToggleCurrentPasswordVisibility.Image = Properties.Resources.eye_open;
            }
            else
            {
                txtCurrentPassword.UseSystemPasswordChar = true;
                pbToggleCurrentPasswordVisibility.Image = Properties.Resources.eye_closed;
            }
        }

        private void pbToggleConfirmNewPasswordVisibility_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.UseSystemPasswordChar)
            {
                txtNewPassword.UseSystemPasswordChar = false;
                pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_open;
            }
            else
            {
                txtNewPassword.UseSystemPasswordChar = true;
                pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_closed;
            }
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            if (txtConfirmNewPassword.UseSystemPasswordChar)
            {
                txtConfirmNewPassword.UseSystemPasswordChar = false;
                pbToggleConfirmNewPasswordVisibility.Image = Properties.Resources.eye_open;
            }
            else
            {
                txtConfirmNewPassword.UseSystemPasswordChar = true;
                pbToggleConfirmNewPasswordVisibility.Image = Properties.Resources.eye_closed;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
