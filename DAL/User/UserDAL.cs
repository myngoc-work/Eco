using DTO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAL
{
    public class UserDAL
    {
        private readonly DatabaseHelper _dbHelper;

        public UserDAL()
        {
            _dbHelper = new DatabaseHelper();
        }

        public UserDTO GetUserByUsername(string username)
        {
            UserDTO user = null;
            string query = @"SELECT u.Company_UserId, u.Company_Username, u.PasswordHash, u.FullName, u.Email, u.Phone, 
                                   u.DateOfBirth, u.Gender, u.Address, u.AvatarPath, u.IsActive, 
                                   u.RoleId, u.DepartmentId, u.EmailVerifiedAt, u.PhoneVerifiedAt, u.LastLoginAt, 
                                   u.TwoFactorSecret, u.IsTwoFactorEnabled, r.RoleName, d.DepartmentName
                            FROM Company_User u
                            LEFT JOIN [Role] r ON u.RoleId = r.RoleId
                            LEFT JOIN Department d ON u.DepartmentId = d.DepartmentId
                            WHERE u.Company_Username = @Username";

            try 
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserDTO
                                {
                                    Company_UserId = reader["Company_UserId"].ToString(),
                                    Company_Username = reader["Company_Username"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                                    Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : null,
                                    Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                                    AvatarPath = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : null,
                                    IsActive = (bool)reader["IsActive"],
                                    RoleId = reader["RoleId"].ToString(),
                                    DepartmentId = reader["DepartmentId"].ToString(),
                                    EmailVerifiedAt = reader["EmailVerifiedAt"] != DBNull.Value ? (DateTime?)reader["EmailVerifiedAt"] : null,
                                    PhoneVerifiedAt = reader["PhoneVerifiedAt"] != DBNull.Value ? (DateTime?)reader["PhoneVerifiedAt"] : null,
                                    LastLoginAt = reader["LastLoginAt"] != DBNull.Value ? (DateTime?)reader["LastLoginAt"] : null,
                                    TwoFactorSecret = reader["TwoFactorSecret"] != DBNull.Value ? reader["TwoFactorSecret"].ToString() : null,
                                    IsTwoFactorEnabled = (bool)reader["IsTwoFactorEnabled"],
                                    RoleName = reader["RoleName"] != DBNull.Value ? reader["RoleName"].ToString() : null,
                                    DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy người dùng theo tên đăng nhập: {ex.Message}");
            }
            return user;
        }

        public void UpdateLastLoginTime(string userId)
        {
            string query = "UPDATE Company_User SET LastLoginAt = GETDATE() WHERE Company_UserId = @UserId";
            try 
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật thời gian đăng nhập cuối: {ex.Message}");
            }
        }

        public UserDTO GetUserByUserId(string userId)
        {
            UserDTO user = null;
            string query = @"SELECT u.Company_UserId, u.Company_Username, u.PasswordHash, u.FullName, u.Email, u.Phone, 
                                   u.DateOfBirth, u.Gender, u.Address, u.AvatarPath, u.IsActive, 
                                   u.RoleId, u.DepartmentId, u.EmailVerifiedAt, u.PhoneVerifiedAt, u.LastLoginAt, 
                                   u.TwoFactorSecret, u.IsTwoFactorEnabled, r.RoleName, d.DepartmentName
                            FROM Company_User u
                            LEFT JOIN [Role] r ON u.RoleId = r.RoleId
                            LEFT JOIN Department d ON u.DepartmentId = d.DepartmentId
                            WHERE u.Company_UserId = @UserId";

            try 
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserDTO
                                {
                                    Company_UserId = reader["Company_UserId"].ToString(),
                                    Company_Username = reader["Company_Username"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                                    Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : null,
                                    Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                                    AvatarPath = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : null,
                                    IsActive = (bool)reader["IsActive"],
                                    RoleId = reader["RoleId"].ToString(),
                                    DepartmentId = reader["DepartmentId"].ToString(),
                                    EmailVerifiedAt = reader["EmailVerifiedAt"] != DBNull.Value ? (DateTime?)reader["EmailVerifiedAt"] : null,
                                    PhoneVerifiedAt = reader["PhoneVerifiedAt"] != DBNull.Value ? (DateTime?)reader["PhoneVerifiedAt"] : null,
                                    LastLoginAt = reader["LastLoginAt"] != DBNull.Value ? (DateTime?)reader["LastLoginAt"] : null,
                                    TwoFactorSecret = reader["TwoFactorSecret"] != DBNull.Value ? reader["TwoFactorSecret"].ToString() : null,
                                    IsTwoFactorEnabled = (bool)reader["IsTwoFactorEnabled"],
                                    RoleName = reader["RoleName"] != DBNull.Value ? reader["RoleName"].ToString() : null,
                                    DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy người dùng theo ID: {ex.Message}");
            }
            return user;
        }

        public bool UpdateUserPassword(string userId, string newPasswordHash)
        {
            string query = "UPDATE Company_User SET PasswordHash = @NewPasswordHash WHERE Company_UserId = @UserId";
            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewPasswordHash", newPasswordHash);
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật mật khẩu: {ex.Message}");
                return false;
            }
        }

        public UserDTO GetUserByEmail(string email)
        {
            UserDTO user = null;
            string query = @"SELECT u.Company_UserId, u.Company_Username, u.PasswordHash, u.FullName, u.Email, u.Phone, 
                                   u.DateOfBirth, u.Gender, u.Address, u.AvatarPath, u.IsActive, 
                                   u.RoleId, u.DepartmentId, u.EmailVerifiedAt, u.PhoneVerifiedAt, u.LastLoginAt, 
                                   u.TwoFactorSecret, u.IsTwoFactorEnabled, r.RoleName, d.DepartmentName
                            FROM Company_User u
                            LEFT JOIN [Role] r ON u.RoleId = r.RoleId
                            LEFT JOIN Department d ON u.DepartmentId = d.DepartmentId
                            WHERE u.Email = @Email";

            try 
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserDTO
                                {
                                    Company_UserId = reader["Company_UserId"].ToString(),
                                    Company_Username = reader["Company_Username"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                                    Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : null,
                                    Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                                    AvatarPath = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : null,
                                    IsActive = (bool)reader["IsActive"],
                                    RoleId = reader["RoleId"].ToString(),
                                    DepartmentId = reader["DepartmentId"].ToString(),
                                    EmailVerifiedAt = reader["EmailVerifiedAt"] != DBNull.Value ? (DateTime?)reader["EmailVerifiedAt"] : null,
                                    PhoneVerifiedAt = reader["PhoneVerifiedAt"] != DBNull.Value ? (DateTime?)reader["PhoneVerifiedAt"] : null,
                                    LastLoginAt = reader["LastLoginAt"] != DBNull.Value ? (DateTime?)reader["LastLoginAt"] : null,
                                    TwoFactorSecret = reader["TwoFactorSecret"] != DBNull.Value ? reader["TwoFactorSecret"].ToString() : null,
                                    IsTwoFactorEnabled = (bool)reader["IsTwoFactorEnabled"],
                                    RoleName = reader["RoleName"] != DBNull.Value ? reader["RoleName"].ToString() : null,
                                    DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy người dùng theo email: {ex.Message}");
            }
            return user;
        }

        public bool AddVerificationToken(VerificationTokenDTO token)
        {
            string query = "INSERT INTO VerificationToken (Company_UserId, Type, TokenValue, Destination, ExpiresAt, CreatedAt) VALUES (@Company_UserId, @Type, @TokenValue, @Destination, @ExpiresAt, @CreatedAt)";
            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Company_UserId", token.Company_UserId);
                        command.Parameters.AddWithValue("@Type", token.Type);
                        command.Parameters.AddWithValue("@TokenValue", token.TokenValue);
                        command.Parameters.AddWithValue("@Destination", token.Destination);
                        command.Parameters.AddWithValue("@ExpiresAt", token.ExpiresAt);
                        command.Parameters.AddWithValue("@CreatedAt", token.CreatedAt);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm VerificationToken: {ex.Message}");
                return false;
            }
        }

        public VerificationTokenDTO GetActiveVerificationToken(string userId, string tokenValue, string type)
        {
            VerificationTokenDTO token = null;
            string query = "SELECT TokenId, Company_UserId, Type, TokenValue, Destination, ExpiresAt, ConsumedAt, CreatedAt FROM VerificationToken WHERE Company_UserId = @Company_UserId AND TokenValue = @TokenValue AND Type = @Type AND ExpiresAt > GETDATE() AND ConsumedAt IS NULL ORDER BY CreatedAt DESC";

            try 
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Company_UserId", userId);
                        command.Parameters.AddWithValue("@TokenValue", tokenValue);
                        command.Parameters.AddWithValue("@Type", type);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                token = new VerificationTokenDTO
                                {
                                    TokenId = (long)reader["TokenId"],
                                    Company_UserId = reader["Company_UserId"].ToString(),
                                    Type = reader["Type"].ToString(),
                                    TokenValue = reader["TokenValue"].ToString(),
                                    Destination = reader["Destination"].ToString(),
                                    ExpiresAt = (DateTime)reader["ExpiresAt"],
                                    ConsumedAt = reader["ConsumedAt"] != DBNull.Value ? (DateTime?)reader["ConsumedAt"] : null, // SỬA: Xử lý NULL
                                    CreatedAt = (DateTime)reader["CreatedAt"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy VerificationToken đang hoạt động: {ex.Message}");
            }
            return token;
        }

        public bool MarkTokenAsConsumed(long tokenId)
        {
            string query = "UPDATE VerificationToken SET ConsumedAt = GETDATE() WHERE TokenId = @TokenId";
            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TokenId", tokenId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đánh dấu token đã sử dụng: {ex.Message}");
                return false;
            }
        }

        public bool RemoveVerificationTokenByValue(string userId, string tokenValue, string type)
        {
            string query = "DELETE FROM VerificationToken WHERE Company_UserId = @Company_UserId AND TokenValue = @TokenValue AND Type = @Type AND ConsumedAt IS NULL";
            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Company_UserId", userId);
                        command.Parameters.AddWithValue("@TokenValue", tokenValue);
                        command.Parameters.AddWithValue("@Type", type);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa VerificationToken theo giá trị: {ex.Message}");
                return false;
            }
        }

        public bool RemoveExistingVerificationTokens(string userId, string type)
        {
            string query = "DELETE FROM VerificationToken WHERE Company_UserId = @Company_UserId AND Type = @Type AND ConsumedAt IS NULL";
            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Company_UserId", userId);
                        command.Parameters.AddWithValue("@Type", type);
                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa các VerificationToken cũ: {ex.Message}");
                return false;
            }
        }

        // 2FA
        public bool UpdateTwoFactorSecretAndStatus(string userId, string secret, bool isEnabled)
        {
            string query = "UPDATE Company_User SET TwoFactorSecret = @Secret, IsTwoFactorEnabled = @IsEnabled WHERE Company_UserId = @UserId";
            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Secret", secret ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsEnabled", isEnabled);
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật 2FA secret và trạng thái: {ex.Message}");
                return false;
            }
        }

        public string GetTwoFactorSecret(string userId)
        {
            string secret = null;
            string query = "SELECT TwoFactorSecret FROM Company_User WHERE Company_UserId = @UserId";
            try 
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            secret = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy TwoFactorSecret: {ex.Message}");
            }
            return secret;
        }

        public bool UpdateUserAvatar(string userId, string avatarPath)
        {
            string query = "UPDATE Company_User SET AvatarPath = @AvatarPath WHERE Company_UserId = @UserId";

            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@AvatarPath", string.IsNullOrEmpty(avatarPath) ? (object)DBNull.Value : avatarPath);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật avatar: {ex.Message}");
                return false;
            }
        }

        public bool UpdateUserProfile(UserDTO user)
        {
            string query = @"UPDATE Company_User 
                           SET Phone = @Phone, Email = @Email, Address = @Address, DepartmentId = @DepartmentId
                           WHERE Company_UserId = @UserId";

            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", user.Company_UserId);
                        command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(user.Phone) ? (object)DBNull.Value : user.Phone);
                        command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(user.Email) ? (object)DBNull.Value : user.Email);
                        command.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(user.Address) ? (object)DBNull.Value : user.Address);
                        command.Parameters.AddWithValue("@DepartmentId", string.IsNullOrEmpty(user.DepartmentId) ? (object)DBNull.Value : user.DepartmentId);
                        
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật thông tin người dùng: {ex.Message}");
                return false;
            }
        }

        public UserDTO GetDepartmentHead(string departmentId)
        {
            string query = @"
                SELECT u.Company_UserId, u.Company_Username, u.PasswordHash, u.FullName, u.Email, u.Phone, 
                       u.DateOfBirth, u.Gender, u.Address, u.AvatarPath, u.IsActive, 
                       u.RoleId, u.DepartmentId, u.EmailVerifiedAt, u.PhoneVerifiedAt, u.LastLoginAt, 
                       u.TwoFactorSecret, u.IsTwoFactorEnabled, r.RoleName, d.DepartmentName
                FROM Company_User u
                LEFT JOIN [Role] r ON u.RoleId = r.RoleId
                LEFT JOIN Department d ON u.DepartmentId = d.DepartmentId
                WHERE u.DepartmentId = @DepartmentId 
                AND u.RoleId = 'R002' 
                AND u.IsActive = 1";

            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DepartmentId", departmentId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserDTO
                                {
                                    Company_UserId = reader["Company_UserId"].ToString(),
                                    Company_Username = reader["Company_Username"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
                                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime?)reader["DateOfBirth"] : null,
                                    Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : null,
                                    Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                                    AvatarPath = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : null,
                                    IsActive = (bool)reader["IsActive"],
                                    RoleId = reader["RoleId"].ToString(),
                                    DepartmentId = reader["DepartmentId"].ToString(),
                                    EmailVerifiedAt = reader["EmailVerifiedAt"] != DBNull.Value ? (DateTime?)reader["EmailVerifiedAt"] : null,
                                    PhoneVerifiedAt = reader["PhoneVerifiedAt"] != DBNull.Value ? (DateTime?)reader["PhoneVerifiedAt"] : null,
                                    LastLoginAt = reader["LastLoginAt"] != DBNull.Value ? (DateTime?)reader["LastLoginAt"] : null,
                                    TwoFactorSecret = reader["TwoFactorSecret"] != DBNull.Value ? reader["TwoFactorSecret"].ToString() : null,
                                    IsTwoFactorEnabled = (bool)reader["IsTwoFactorEnabled"],
                                    RoleName = reader["RoleName"].ToString(),
                                    DepartmentName = reader["DepartmentName"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy trưởng phòng: {ex.Message}");
            }

            return null;
        }

    }
}