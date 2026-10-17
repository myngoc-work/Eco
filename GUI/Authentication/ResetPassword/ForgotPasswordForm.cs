using BLL;
using DTO; // Cần thiết nếu bạn dùng UserDTO trực tiếp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Authentication.ResetPassword
{
    public partial class ForgotPasswordForm : Form
    {
        private readonly UserBLL _userBLL;
        private Timer _otpTimer;
        private int _otpTimeRemaining;
        private string _userEmailForReset; // Lưu email để sử dụng cho bước xác nhận OTP
        private string _userIdForReset;    // Lưu userId sau khi email được tìm thấy

        public ForgotPasswordForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            _userBLL = new UserBLL();
            InitializeOtpTimer();
            SetInitialState(); // Đặt trạng thái ban đầu cho các control
        }

        private void InitializeOtpTimer()
        {
            _otpTimer = new Timer();
            _otpTimer.Interval = 1000; // 1 giây
            _otpTimer.Tick += OtpTimer_Tick;
        }

        private void SetInitialState()
        {
            txtOtpCode.Enabled = false;
            btnConfirmOtp.Enabled = false;
            lblOtpTimer.Visible = false;
            lblOtpTimer.Text = "";
            btnSendOtp.Text = "Gửi mã OTP";
            txtEmail.Enabled = true; // Đảm bảo email có thể chỉnh sửa lúc đầu
        }
        private void OtpTimer_Tick(object sender, EventArgs e)
        {
            _otpTimeRemaining--;
            if (_otpTimeRemaining >= 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(_otpTimeRemaining);
                lblOtpTimer.Text = $"Mã có hiệu lực trong: {time.Minutes:D2}:{time.Seconds:D2}";
            }
            else
            {
                StopOtpTimer();
                lblOtpTimer.Text = "Mã OTP đã hết hạn.";
                btnConfirmOtp.Enabled = false;
                MessageBox.Show("Mã OTP đã hết hạn. Vui lòng gửi lại mã mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSendOtp.Enabled = true; // Cho phép gửi lại OTP sau khi hết hạn
            }
        }

        private void StartOtpTimer(int seconds)
        {
            _otpTimeRemaining = seconds;
            _otpTimer.Start();
        }

        private void StopOtpTimer()
        {
            _otpTimer.Stop();
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                // Sử dụng Regex cơ bản hơn một chút để tránh lỗi Timeout nếu chuỗi email quá phức tạp
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {

        }

        private async void btnSendOtp_Click_1(object sender, EventArgs e)
        {
            _userEmailForReset = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(_userEmailForReset) || !IsValidEmail(_userEmailForReset))
            {
                MessageBox.Show("Vui lòng nhập một địa chỉ email hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSendOtp.Enabled = false;
            txtEmail.Enabled = false;

            // Thay đổi ở đây để nhận userId
            var (success, message, userIdFromBLL) = await _userBLL.SendPasswordResetOtpAsync(_userEmailForReset);

            if (success)
            {
                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Gán userId nhận được từ BLL
                _userIdForReset = userIdFromBLL;

                if (string.IsNullOrEmpty(_userIdForReset)) // Kiểm tra an toàn, mặc dù không nên null nếu success là true
                {
                    MessageBox.Show("Lỗi nội bộ: Không thể lấy ID người dùng sau khi gửi OTP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetInitialState();
                    return;
                }

                txtOtpCode.Enabled = true;
                btnConfirmOtp.Enabled = true;
                lblOtpTimer.Visible = true;
                StartOtpTimer(300); // 5 phút = 300 giây
                btnSendOtp.Text = "Gửi lại OTP";
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSendOtp.Enabled = true;
                txtEmail.Enabled = true;
                StopOtpTimer();
                lblOtpTimer.Visible = false;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirmOtp_Click(object sender, EventArgs e)
        {
            string otpCode = txtOtpCode.Text.Trim();

            if (string.IsNullOrEmpty(otpCode))
            {
                MessageBox.Show("Vui lòng nhập mã OTP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_userIdForReset) || string.IsNullOrEmpty(_userEmailForReset))
            {
                MessageBox.Show("Email hoặc ID người dùng không được thiết lập. Vui lòng gửi lại OTP.", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetInitialState(); // Đặt lại trạng thái
                return;
            }

            var (success, userIdReturned, message) = _userBLL.VerifyOtp(_userEmailForReset, otpCode);

            if (success)
            {
                StopOtpTimer();
                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Đảm bảo userId đã được xác định trước khi mở TypeNewPasswordForm
                if (!string.IsNullOrEmpty(userIdReturned))
                {
                    TypeNewPasswordForm typeNewPasswordForm = new TypeNewPasswordForm(userIdReturned);
                    this.Hide();
                    typeNewPasswordForm.ShowDialog();
                    this.Close(); // Đóng form hiện tại sau khi TypeNewPasswordForm đóng
                }
                else
                {
                    MessageBox.Show("Lỗi nội bộ: Không thể lấy ID người dùng sau khi xác thực OTP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetInitialState(); // Đặt lại trạng thái
                }
            }
            else
            {
                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
