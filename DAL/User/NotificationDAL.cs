using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAL.User
{
    public class NotificationDAL
    {
        private DatabaseHelper _dbHelper;

        public NotificationDAL()
        {
            _dbHelper = new DatabaseHelper();
        }

        public List<NotificationDTO> GetOverdueContractNotifications()
        {
            List<NotificationDTO> notifications = new List<NotificationDTO>();
            
            string query = @"
                SELECT 
                    n.NotificationId,
                    n.Type,
                    n.Message,
                    n.CreatedAt,
                    n.DaysLate,
                    n.ContractId,
                    n.BatchId,
                    n.TargetCompany_UserId,
                    n.TargetDeptId,
                    c.CustomerId,
                    cu.CustomerName,
                    c.PlanDueDate as ExpectedDate,
                    c.Status
                FROM Notification n
                LEFT JOIN Contract c ON n.ContractId = c.ContractId
                LEFT JOIN Customer cu ON c.CustomerId = cu.CustomerId
                WHERE n.Type = 'OVERDUE_CONTRACT'
                ORDER BY n.CreatedAt DESC";

            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var notification = new NotificationDTO
                                {
                                    NotificationId = reader["NotificationId"].ToString(),
                                    Type = reader["Type"].ToString(),
                                    Message = reader["Message"].ToString(),
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    DaysLate = reader["DaysLate"] != DBNull.Value ? (int?)reader["DaysLate"] : null,
                                    ContractId = reader["ContractId"] != DBNull.Value ? reader["ContractId"].ToString() : null,
                                    BatchId = reader["BatchId"] != DBNull.Value ? reader["BatchId"].ToString() : null,
                                    TargetCompany_UserId = reader["TargetCompany_UserId"] != DBNull.Value ? reader["TargetCompany_UserId"].ToString() : null,
                                    TargetDeptId = reader["TargetDeptId"] != DBNull.Value ? reader["TargetDeptId"].ToString() : null,
                                    CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : null,
                                    ExpectedDate = reader["ExpectedDate"] != DBNull.Value ? (DateTime?)reader["ExpectedDate"] : null,
                                    Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null
                                };
                                notifications.Add(notification);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách thông báo: {ex.Message}");
            }

            return notifications;
        }

        public List<NotificationDTO> SearchOverdueContractNotifications(string searchText)
        {
            List<NotificationDTO> notifications = new List<NotificationDTO>();
            
            string query = @"
                SELECT 
                    n.NotificationId,
                    n.Type,
                    n.Message,
                    n.CreatedAt,
                    n.DaysLate,
                    n.ContractId,
                    n.BatchId,
                    n.TargetCompany_UserId,
                    n.TargetDeptId,
                    c.CustomerId,
                    cu.CustomerName,
                    c.PlanDueDate as ExpectedDate,
                    c.Status
                FROM Notification n
                LEFT JOIN Contract c ON n.ContractId = c.ContractId
                LEFT JOIN Customer cu ON c.CustomerId = cu.CustomerId
                WHERE n.Type = 'OVERDUE_CONTRACT'
                AND (n.ContractId LIKE @SearchText 
                     OR cu.CustomerName LIKE @SearchText
                     OR c.ContractId LIKE @SearchText)
                ORDER BY n.CreatedAt DESC";

            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchText", $"%{searchText}%");
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var notification = new NotificationDTO
                                {
                                    NotificationId = reader["NotificationId"].ToString(),
                                    Type = reader["Type"].ToString(),
                                    Message = reader["Message"].ToString(),
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    DaysLate = reader["DaysLate"] != DBNull.Value ? (int?)reader["DaysLate"] : null,
                                    ContractId = reader["ContractId"] != DBNull.Value ? reader["ContractId"].ToString() : null,
                                    BatchId = reader["BatchId"] != DBNull.Value ? reader["BatchId"].ToString() : null,
                                    TargetCompany_UserId = reader["TargetCompany_UserId"] != DBNull.Value ? reader["TargetCompany_UserId"].ToString() : null,
                                    TargetDeptId = reader["TargetDeptId"] != DBNull.Value ? reader["TargetDeptId"].ToString() : null,
                                    CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : null,
                                    ExpectedDate = reader["ExpectedDate"] != DBNull.Value ? (DateTime?)reader["ExpectedDate"] : null,
                                    Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null
                                };
                                notifications.Add(notification);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tìm kiếm thông báo: {ex.Message}");
            }

            return notifications;
        }

        public bool CreateNotification(string notificationId, string type, string message, int daysLate, string contractId, string targetUserId, string targetDeptId)
        {
            string query = @"
                INSERT INTO Notification (NotificationId, Type, Message, CreatedAt, DaysLate, ContractId, TargetCompany_UserId, TargetDeptId)
                VALUES (@NotificationId, @Type, @Message, @CreatedAt, @DaysLate, @ContractId, @TargetCompany_UserId, @TargetDeptId)";

            try
            {
                using (SqlConnection connection = _dbHelper.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NotificationId", notificationId);
                        command.Parameters.AddWithValue("@Type", type);
                        command.Parameters.AddWithValue("@Message", message);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@DaysLate", daysLate);
                        command.Parameters.AddWithValue("@ContractId", contractId);
                        command.Parameters.AddWithValue("@TargetCompany_UserId", targetUserId);
                        command.Parameters.AddWithValue("@TargetDeptId", targetDeptId);
                        
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo thông báo: {ex.Message}");
                return false;
            }
        }
    }
}
