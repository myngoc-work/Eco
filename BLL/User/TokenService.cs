
using DTO; 
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace BLL.Utils
{
    public class TokenService
    {
        private const string TokenFilePath = "user_token.txt"; 
        public string GenerateToken(string username)
        {
            string rawToken = $"{username}:{DateTime.UtcNow.Ticks}";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                string token = builder.ToString();
                try
                {
                    File.WriteAllText(TokenFilePath, token);
                    return token;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving token: {ex.Message}");
                    return null;
                }
            }
        }

        public UserDTO ValidateToken()
        {
            // Trong một ứng dụng thực, bạn sẽ kiểm tra tính hợp lệ của token
            // với cơ sở dữ liệu hoặc thông tin đã mã hóa trong token.
            // Ở đây, chúng ta chỉ kiểm tra xem có token tồn tại không.

            if (!File.Exists(TokenFilePath))
            {
                return null;
            }

            try
            {
                string storedToken = File.ReadAllText(TokenFilePath);
                if (!string.IsNullOrEmpty(storedToken))
                {
                    // Giả định token hợp lệ và trả về một UserDTO mẫu.
                    // THỰC TẾ: bạn cần giải mã token và tìm UserDTO từ database.
                    // Để đơn giản cho login, chúng ta sẽ không dùng auto-login bằng token ngay.
                    return null; // Không tự động đăng nhập bằng token trong ví dụ này để tập trung vào login thủ công
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading token: {ex.Message}");
            }
            return null;
        }

        public bool ClearToken()
        {
            try
            {
                if (File.Exists(TokenFilePath))
                {
                    File.Delete(TokenFilePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing token: {ex.Message}");
                return false;
            }
        }
    }
}