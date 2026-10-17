using DAL;
using DTO;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OtpNet;
namespace BLL
{
    public class UserBLL
    {
        private readonly UserDAL _userDAL;
        private readonly EmailService _emailService;

        public UserBLL()
        {
            _userDAL = new UserDAL();

            // Khởi tạo EmailService từ App.config
            string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            // Lỗi ở đây: SmtpUsername chưa được đọc từ App.config
            string smtpUsername = ConfigurationManager.AppSettings["FromEmail"]; // SmtpUsername thường là FromEmail
            string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
            string fromEmail = ConfigurationManager.AppSettings["FromEmail"];

            _emailService = new EmailService(smtpHost, smtpPort, smtpUsername, smtpPassword, enableSsl, fromEmail);
        }

        // Phương thức xác thực đăng nhập
        public UserDTO AuthenticateUser(string username, string password)
        {
            UserDTO user = _userDAL.GetUserByUsername(username);

            if (user != null)
            {
                string hashedPasswordInput = HashPassword(password);
                if (user.PasswordHash == hashedPasswordInput)
                {
                    _userDAL.UpdateLastLoginTime(user.Company_UserId);
                    return user; // Đăng nhập thành công
                }
            }
            return null; // Đăng nhập thất bại
        }

        // Phương thức băm mật khẩu SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("X2")); // "X2" cho định dạng thập lục phân
                }
                return builder.ToString();
            }
        }

        // Phương thức kiểm tra độ phức tạp mật khẩu
        public string ValidatePasswordStrength(string password)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                errors.AppendLine("- Mật khẩu phải có ít nhất 8 ký tự.");
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errors.AppendLine("- Mật khẩu phải chứa ít nhất một chữ cái in hoa.");
            }
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errors.AppendLine("- Mật khẩu phải chứa ít nhất một chữ cái in thường.");
            }
            if (!Regex.IsMatch(password, @"\d"))
            {
                errors.AppendLine("- Mật khẩu phải chứa ít nhất một chữ số.");
            }
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
            {
                errors.AppendLine("- Mật khẩu phải chứa ít nhất một ký tự đặc biệt (!@#$%^&*...).");
            }

            return errors.ToString();
        }

        // Phương thức thay đổi mật khẩu (khi người dùng đã đăng nhập và biết mật khẩu cũ)
        public bool ChangePassword(string userId, string currentPassword, string newPassword, string confirmNewPassword, out string errorMessage)
        {
            errorMessage = "";

            UserDTO user = _userDAL.GetUserByUserId(userId);

            if (user == null)
            {
                errorMessage = "Không tìm thấy thông tin người dùng.";
                return false;
            }

            string hashedCurrentPasswordInput = HashPassword(currentPassword);
            if (user.PasswordHash != hashedCurrentPasswordInput)
            {
                errorMessage = "Mật khẩu hiện tại không đúng.";
                return false;
            }

            if (newPassword != confirmNewPassword)
            {
                errorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                return false;
            }

            string strengthErrors = ValidatePasswordStrength(newPassword);
            if (!string.IsNullOrEmpty(strengthErrors))
            {
                errorMessage = "Mật khẩu mới không đáp ứng yêu cầu độ phức tạp:\n" + strengthErrors;
                return false;
            }

            // Kiểm tra mật khẩu mới không trùng với mật khẩu hiện tại
            if (newPassword == currentPassword) // So sánh chuỗi rõ ràng trước khi băm
            {
                errorMessage = "Mật khẩu mới không được trùng với mật khẩu hiện tại.";
                return false;
            }

            // Sau khi kiểm tra chuỗi rõ ràng, băm mật khẩu mới để so sánh với mật khẩu băm cũ
            string hashedNewPasswordCheck = HashPassword(newPassword);
            if (user.PasswordHash == hashedNewPasswordCheck)
            {
                errorMessage = "Mật khẩu mới không được trùng với mật khẩu hiện tại.";
                return false;
            }

            string hashedNewPassword = HashPassword(newPassword);

            bool success = _userDAL.UpdateUserPassword(userId, hashedNewPassword);
            if (!success)
            {
                errorMessage = "Đã xảy ra lỗi khi cập nhật mật khẩu trong cơ sở dữ liệu.";
            }
            return success;
        }

        // Phương thức gửi OTP để đặt lại mật khẩu (cho chức năng quên mật khẩu)
        public async Task<(bool success, string message, string userId)> SendPasswordResetOtpAsync(string email)
        {
            UserDTO user = _userDAL.GetUserByEmail(email);
            if (user == null)
            {
                return (false, "Không tìm thấy tài khoản với email này.", null); // Thêm null cho userId
            }

            _userDAL.RemoveExistingVerificationTokens(user.Company_UserId, "OTP_PASSWORD_RESET");

            string otp = GenerateRandomOtp();
            DateTime expiresAt = DateTime.Now.AddMinutes(5);

            VerificationTokenDTO token = new VerificationTokenDTO
            {
                Company_UserId = user.Company_UserId,
                Type = "OTP_PASSWORD_RESET",
                TokenValue = otp,
                Destination = email,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.Now
            };

            if (!_userDAL.AddVerificationToken(token))
            {
                return (false, "Không thể tạo mã OTP. Vui lòng thử lại.", null); // Thêm null cho userId
            }

            string subject = "Mã OTP Đặt lại Mật khẩu của bạn";
            string body = $"Chào {user.FullName},<br><br>" +
                          $"Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản của mình. Mã OTP của bạn là: <strong>{otp}</strong>.<br><br>" +
                          $"Mã này sẽ hết hạn sau 5 phút. Vui lòng không chia sẻ mã này với bất kỳ ai.<br><br>" +
                          "Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.<br><br>" +
                          "Trân trọng,<br>Công ty TNHH EcoGreen";

            bool emailSent = await _emailService.SendEmailAsync(email, subject, body);

            if (!emailSent)
            {
                _userDAL.RemoveVerificationTokenByValue(user.Company_UserId, otp, "OTP_PASSWORD_RESET");
                return (false, "Không thể gửi mã OTP đến email của bạn. Vui lòng kiểm tra địa chỉ email và thử lại.", null); // Thêm null cho userId
            }

            // Nếu mọi thứ thành công, trả về userId
            return (true, "Mã OTP đã được gửi đến email của bạn. Vui lòng kiểm tra hộp thư đến.", user.Company_UserId);
        }

        // Phương thức xác thực OTP
        public (bool success, string userId, string message) VerifyOtp(string email, string otp)
        {
            UserDTO user = _userDAL.GetUserByEmail(email);
            if (user == null)
            {
                // Tránh tiết lộ thông tin người dùng bằng cách trả về thông báo chung chung hơn
                return (false, null, "Email hoặc mã OTP không hợp lệ.");
            }

            VerificationTokenDTO token = _userDAL.GetActiveVerificationToken(user.Company_UserId, otp, "OTP_PASSWORD_RESET");

            if (token == null)
            {
                return (false, null, "Mã OTP không hợp lệ hoặc đã hết hạn.");
            }

            // Đánh dấu token đã sử dụng
            _userDAL.MarkTokenAsConsumed(token.TokenId);

            return (true, user.Company_UserId, "Xác thực OTP thành công.");
        }

        // Phương thức đặt lại mật khẩu (sau khi OTP đã được xác thực)
        public bool ResetPassword(string userId, string newPassword, string confirmNewPassword, out string errorMessage)
        {
            errorMessage = "";

            if (newPassword != confirmNewPassword)
            {
                errorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
                return false;
            }

            string strengthErrors = ValidatePasswordStrength(newPassword);
            if (!string.IsNullOrEmpty(strengthErrors))
            {
                errorMessage = "Mật khẩu mới không đáp ứng yêu cầu độ phức tạp:\n" + strengthErrors;
                return false;
            }

            UserDTO user = _userDAL.GetUserByUserId(userId);
            if (user != null)
            {
                string hashedNewPasswordCheck = HashPassword(newPassword);
                if (user.PasswordHash == hashedNewPasswordCheck)
                {
                    errorMessage = "Mật khẩu mới không được trùng với mật khẩu hiện tại.";
                    return false;
                }
            }
            else
            {
                errorMessage = "Không tìm thấy thông tin người dùng. Vui lòng thử lại quá trình đặt lại mật khẩu.";
                return false;
            }

            string hashedNewPassword = HashPassword(newPassword);

            bool success = _userDAL.UpdateUserPassword(userId, hashedNewPassword);
            if (!success)
            {
                errorMessage = "Đã xảy ra lỗi khi cập nhật mật khẩu trong cơ sở dữ liệu.";
            }
            return success;
        }

        // Phương thức tạo mã OTP ngẫu nhiên
        private string GenerateRandomOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Tạo OTP 6 chữ số
        }
        // Phương thức tạo mã 2FA sử dụng thư viện OtpNet
        // Phương thức tạo mã 2FA sử dụng thư viện OtpNet
        // 1. Tạo Secret Key mới cho 2FA
        public string GenerateTwoFactorSecret()
        {
            // Secret Key phải là 160 bits (20 bytes) cho Google Authenticator, được encode Base32
            var key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        // 2. Lấy URI để tạo mã QR cho Google Authenticator
        public string GetQRCodeUri(string secretBase32, string username, string issuer)
        {
            // Đảm bảo secretBase32 là hợp lệ trước khi chuyển đổi
            if (string.IsNullOrWhiteSpace(secretBase32))
            {
                throw new ArgumentException("Secret key cannot be null or empty.", nameof(secretBase32));
            }

            byte[] secretBytes;
            try
            {
                secretBytes = Base32Encoding.ToBytes(secretBase32);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid Base32 secret key.", nameof(secretBase32), ex);
            }

            var uri = new OtpNet.OtpUri(
                OtpNet.OtpType.Totp,   // Sử dụng OtpNet.OtpType.Totp thay cho OtpUriType.Totp
                secretBase32,
                username,
                issuer,
                OtpNet.OtpHashMode.Sha1,
                6,
                30
            );

            return uri.ToString();
        }

        // 3. Xác thực mã TOTP
        public bool VerifyTwoFactorCode(string userId, string enteredCode)
        {
            if (string.IsNullOrWhiteSpace(enteredCode)) return false;

            string secretBase32 = _userDAL.GetTwoFactorSecret(userId);

            if (string.IsNullOrWhiteSpace(secretBase32))
            {
                // Người dùng chưa có secret key hoặc 2FA chưa được thiết lập
                return false;
            }

            try
            {
                byte[] secretBytes = OtpNet.Base32Encoding.ToBytes(secretBase32); var totp = new Totp(secretBytes);
                // Verify with a time drift tolerance (e.g., allow codes from 1 window before/after)
                return totp.VerifyTotp(enteredCode, out _, new VerificationWindow(1, 1)); // Cho phép lệch 1 cửa sổ thời gian
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xác thực mã TOTP: {ex.Message}");
                return false;
            }
        }

        // Phương thức điều chỉnh để xác thực TOTP trực tiếp với secret key (dùng khi thiết lập)
        public bool VerifyTwoFactorCodeWithSecret(string secretBase32, string enteredCode)
        {
            if (string.IsNullOrWhiteSpace(enteredCode) || string.IsNullOrWhiteSpace(secretBase32)) return false;

            try
            {
                byte[] secretBytes = OtpNet.Base32Encoding.ToBytes(secretBase32); var totp = new Totp(secretBytes);
                return totp.VerifyTotp(enteredCode, out _, new VerificationWindow(1, 1)); // Cho phép lệch 1 cửa sổ thời gian
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xác thực mã TOTP với secret: {ex.Message}");
                return false;
            }
        }

        public bool EnableTwoFactor(string userId, string secret)
        {
            if (string.IsNullOrWhiteSpace(secret)) return false;
            return _userDAL.UpdateTwoFactorSecretAndStatus(userId, secret, true);
        }

        public bool DisableTwoFactor(string userId)
        {
            // Truyền NULL cho secret để xóa nó trong DB
            return _userDAL.UpdateTwoFactorSecretAndStatus(userId, null, false);
        }

		public UserDTO GetDepartmentHead(string departmentId)
		{
			if (string.IsNullOrWhiteSpace(departmentId)) return null;
			return _userDAL.GetDepartmentHead(departmentId);
		}

        public UserDTO GetUserByUserId(string userId)
        {
            return _userDAL.GetUserByUserId(userId);
        }

        public bool UpdateUserAvatar(string userId, string avatarPath)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            return _userDAL.UpdateUserAvatar(userId, avatarPath);
        }

        public bool UpdateUserProfile(UserDTO user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Company_UserId))
            {
                return false;
            }

            return _userDAL.UpdateUserProfile(user);
        }
    }
}