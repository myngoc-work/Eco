using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using DAL;
using DAL.User;
using DTO;
using DTO.User;

namespace BLL.User
{
    public class OverdueNotificationService
    {
        private ContractDAL _contractDAL;
        private UserDAL _userDAL;
        private NotificationDAL _notificationDAL;
        private EmailService _emailService;

        public OverdueNotificationService()
        {
            _contractDAL = new ContractDAL();
            _userDAL = new UserDAL();
            _notificationDAL = new NotificationDAL();
            
            try
            {
                // Đọc cấu hình email từ App.config (sẽ đọc từ file app.config của executable - GUI.exe.config)
                // ConfigurationManager sẽ tự động tìm file app.config của executable
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                string smtpPortStr = ConfigurationManager.AppSettings["SmtpPort"];
                string smtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
                string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                string enableSslStr = ConfigurationManager.AppSettings["EnableSsl"];
                string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                
                // Log để debug
                System.Diagnostics.Debug.WriteLine($"[OverdueNotificationService] Đọc cấu hình email:");
                System.Diagnostics.Debug.WriteLine($"  SmtpHost: {smtpHost ?? "NULL"}");
                System.Diagnostics.Debug.WriteLine($"  SmtpPort: {smtpPortStr ?? "NULL"}");
                System.Diagnostics.Debug.WriteLine($"  SmtpUsername: {smtpUsername ?? "NULL"}");
                System.Diagnostics.Debug.WriteLine($"  SmtpPassword: {(string.IsNullOrEmpty(smtpPassword) ? "NULL" : "***")}");
                System.Diagnostics.Debug.WriteLine($"  EnableSsl: {enableSslStr ?? "NULL"}");
                System.Diagnostics.Debug.WriteLine($"  FromEmail: {fromEmail ?? "NULL"}");
                
                Console.WriteLine($"[OverdueNotificationService] Đọc cấu hình email:");
                Console.WriteLine($"  SmtpHost: {smtpHost ?? "NULL"}");
                Console.WriteLine($"  SmtpPort: {smtpPortStr ?? "NULL"}");
                Console.WriteLine($"  SmtpUsername: {smtpUsername ?? "NULL"}");
                
                // Kiểm tra các giá trị bắt buộc
                if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpPortStr) || 
                    string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword) || 
                    string.IsNullOrEmpty(fromEmail))
                {
                    string errorMsg = "❌ Cảnh báo: Thiếu cấu hình email trong App.config. Email thông báo quá hạn sẽ không hoạt động.";
                    Console.WriteLine(errorMsg);
                    Console.WriteLine($"   SmtpHost: {smtpHost ?? "NULL"}, SmtpPort: {smtpPortStr ?? "NULL"}, SmtpUsername: {smtpUsername ?? "NULL"}");
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    _emailService = null; // Không khởi tạo email service nếu thiếu cấu hình
                    return;
                }
                
                int smtpPort = int.Parse(smtpPortStr);
                bool enableSsl = !string.IsNullOrEmpty(enableSslStr) && bool.Parse(enableSslStr);
                
                _emailService = new EmailService(smtpHost, smtpPort, smtpUsername, smtpPassword, enableSsl, fromEmail);
                string successMsg = "✅ Đã khởi tạo EmailService thành công";
                Console.WriteLine(successMsg);
                System.Diagnostics.Debug.WriteLine(successMsg);
            }
            catch (Exception ex)
            {
                string errorMsg = $"❌ Lỗi khi khởi tạo EmailService: {ex.Message}";
                Console.WriteLine(errorMsg);
                Console.WriteLine($"   StackTrace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine(errorMsg);
                System.Diagnostics.Debug.WriteLine($"   StackTrace: {ex.StackTrace}");
                _emailService = null;
            }
        }

        /// <summary>
        /// Kiểm tra và gửi thông báo cho các hợp đồng quá hạn
        /// Chỉ xét các hợp đồng trong tháng hiện tại (giống logic dashboard)
        /// </summary>
        public void CheckAndSendOverdueNotifications()
        {
            try
            {
                Console.WriteLine("🔍 Bắt đầu kiểm tra hợp đồng quá hạn...");
                
                // Lấy tất cả hợp đồng
                var contracts = _contractDAL.GetAllContracts();
                if (contracts == null)
                {
                    Console.WriteLine("⚠️ Không lấy được danh sách hợp đồng từ database");
                    return;
                }
                
                // Lọc hợp đồng trong tháng hiện tại (giống logic dashboard)
                var now = DateTime.Now;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);
                
                var thisMonthContracts = contracts
                    .Where(c =>
                    {
                        // Sử dụng PlanDueDate hoặc SignDate làm reference date (giống dashboard)
                        DateTime referenceDate = c.PlanDueDate ?? c.SignDate;
                        return referenceDate >= startOfMonth && referenceDate < endOfMonth;
                    })
                    .ToList();
                
                Console.WriteLine($"📊 Tìm thấy {thisMonthContracts.Count} hợp đồng trong tháng hiện tại");

                if (thisMonthContracts.Count == 0)
                {
                    Console.WriteLine("ℹ️ Không có hợp đồng nào trong tháng hiện tại");
                    return;
                }

                int overdueCount = 0;
                int emailSentCount = 0;
                int emailFailedCount = 0;
                
                foreach (var contract in thisMonthContracts)
                {
                    // Kiểm tra quá hạn: PlanDueDate phải có giá trị và nhỏ hơn ngày hiện tại (giống logic dashboard)
                    if (contract.PlanDueDate.HasValue && contract.PlanDueDate.Value.Date < now.Date)
                    {
                        // Loại bỏ các hợp đồng đã hoàn thành
                        if (IsCompletedStatus(contract.Status))
                        {
                            Console.WriteLine($"ℹ️ Bỏ qua hợp đồng {contract.ContractId}: Đã hoàn thành (Status: {contract.Status})");
                            continue;
                        }
                        
                        var daysOverdue = (now.Date - contract.PlanDueDate.Value.Date).Days;
                        overdueCount++;
                        Console.WriteLine($"⚠️ Phát hiện hợp đồng quá hạn: {contract.ContractId} (quá hạn {daysOverdue} ngày, Status: {contract.Status})");
                        
                        // Lấy thông tin trưởng phòng của nhân viên bán hàng
                        var salesUser = _userDAL.GetUserByUserId(contract.SalesCompany_UserId);
                        if (salesUser != null)
                        {
                            // Lấy trưởng phòng của bộ phận
                            var departmentHead = _userDAL.GetDepartmentHead(salesUser.DepartmentId);
                            if (departmentHead != null)
                            {
                                // Gửi email thông báo
                                if (_emailService != null)
                                {
                                    SendOverdueEmail(departmentHead, contract, daysOverdue);
                                    emailSentCount++;
                                }
                                else
                                {
                                    Console.WriteLine($"⚠️ Không thể gửi email cho hợp đồng {contract.ContractId}: EmailService chưa được khởi tạo");
                                    emailFailedCount++;
                                }
                                
                                // Tạo thông báo trong database
                                CreateOverdueNotification(contract, departmentHead, daysOverdue);
                            }
                            else
                            {
                                Console.WriteLine($"⚠️ Không tìm thấy trưởng phòng cho bộ phận {salesUser.DepartmentId} của hợp đồng {contract.ContractId}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ Không tìm thấy nhân viên bán hàng {contract.SalesCompany_UserId} cho hợp đồng {contract.ContractId}");
                        }
                    }
                }
                
                Console.WriteLine($"📈 Tổng kết: {overdueCount} hợp đồng quá hạn, {emailSentCount} email đã gửi, {emailFailedCount} email thất bại");
                
                if (overdueCount > 0)
                {
                    Console.WriteLine($"✅ Đã kiểm tra và xử lý {overdueCount} hợp đồng quá hạn");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi kiểm tra hợp đồng quá hạn: {ex.Message}");
                Console.WriteLine($"   Chi tiết: {ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// Kiểm tra xem status có phải là trạng thái đã hoàn thành không (giống logic dashboard)
        /// </summary>
        private bool IsCompletedStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return false;
            var normalized = status.Trim().ToLowerInvariant();
            return normalized.Contains("done") ||
                   normalized.Contains("success") ||
                   normalized.Contains("thành công") ||
                   normalized.Contains("completed") ||
                   normalized.Contains("hoàn thành");
        }

        /// <summary>
        /// Gửi email thông báo quá hạn
        /// </summary>
        private void SendOverdueEmail(UserDTO departmentHead, ContractDTO contract, int daysOverdue)
        {
            try
            {
                // Kiểm tra xem EmailService đã được khởi tạo chưa
                if (_emailService == null)
                {
                    Console.WriteLine($"⚠️ Không thể gửi email cho hợp đồng {contract.ContractId}: EmailService chưa được khởi tạo (thiếu cấu hình)");
                    return;
                }
                
                // Kiểm tra email của người nhận
                if (string.IsNullOrEmpty(departmentHead?.Email))
                {
                    Console.WriteLine($"⚠️ Không thể gửi email cho hợp đồng {contract.ContractId}: Trưởng phòng {departmentHead?.FullName} không có email");
                    return;
                }
                
                string subject = $"THÔNG BÁO: Hợp đồng {contract.ContractId} đã quá hạn {daysOverdue} ngày";
                
                string body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                        <h2 style='color: #d32f2f; margin-bottom: 20px;'>🚨 THÔNG BÁO HỢP ĐỒNG QUÁ HẠN</h2>
                        
                        <p>Kính gửi Anh/Chị <strong>{departmentHead.FullName}</strong>,</p>
                        
                        <p>Hệ thống xin thông báo về tình trạng hợp đồng quá hạn như sau:</p>
                        
                        <div style='background-color: #fff3cd; padding: 20px; border-left: 4px solid #ffc107; margin: 20px 0; border-radius: 4px;'>
                            <h3 style='margin-top: 0; color: #856404;'>📋 Thông tin hợp đồng:</h3>
                            <table style='width: 100%; border-collapse: collapse;'>
                                <tr>
                                    <td style='padding: 8px 0; font-weight: bold; width: 200px;'>Mã hợp đồng:</td>
                                    <td style='padding: 8px 0;'>{contract.ContractId}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px 0; font-weight: bold;'>Khách hàng:</td>
                                    <td style='padding: 8px 0;'>{contract.CustomerName}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px 0; font-weight: bold;'>Ngày ký hợp đồng:</td>
                                    <td style='padding: 8px 0;'>{contract.SignDate.ToString("dd/MM/yyyy")}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px 0; font-weight: bold;'>Ngày dự kiến hoàn thành:</td>
                                    <td style='padding: 8px 0;'>{contract.PlanDueDate?.ToString("dd/MM/yyyy")}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px 0; font-weight: bold;'>Số ngày quá hạn:</td>
                                    <td style='padding: 8px 0; color: #d32f2f; font-weight: bold; font-size: 16px;'>{daysOverdue} ngày</td>
                                </tr>
                            </table>
                        </div>
                        
                        <div style='background-color: #f8d7da; padding: 15px; border-left: 4px solid #dc3545; margin: 20px 0; border-radius: 4px;'>
                            <p style='margin: 0; color: #721c24; font-weight: bold;'>
                                ⚠️ Yêu cầu: Vui lòng kiểm tra và xử lý hợp đồng này để đảm bảo tiến độ công việc.
                            </p>
                        </div>
                        
                        <p>Trân trọng,<br>
                        <strong>Hệ thống Quản lý Hợp đồng Quan trắc Môi trường</strong></p>
                        
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #ddd;'>
                        <p style='font-size: 12px; color: #666;'>
                            📧 Email này được gửi tự động từ hệ thống. Vui lòng không trả lời email này.
                        </p>
                    </body>
                    </html>";

                bool emailSent = _emailService.SendEmail(departmentHead.Email, subject, body);
                if (emailSent)
                {
                    Console.WriteLine($"✅ Đã gửi email thông báo quá hạn cho {departmentHead.FullName} ({departmentHead.Email}) về hợp đồng {contract.ContractId}");
                }
                else
                {
                    Console.WriteLine($"❌ Không thể gửi email thông báo quá hạn cho {departmentHead.Email} về hợp đồng {contract.ContractId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi gửi email thông báo cho hợp đồng {contract.ContractId}: {ex.Message}");
                Console.WriteLine($"   Chi tiết: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Tạo thông báo quá hạn trong database
        /// </summary>
        private void CreateOverdueNotification(ContractDTO contract, UserDTO departmentHead, int daysOverdue)
        {
            try
            {
                string notificationId = $"TB_{contract.ContractId}_{DateTime.Now:yyyyMMddHHmmss}";
                string message = $"Hợp đồng {contract.ContractId} đã quá hạn {daysOverdue} ngày";
                
                // Kiểm tra xem thông báo đã tồn tại chưa
                var existingNotifications = _notificationDAL.GetOverdueContractNotifications();
                var existingNotification = existingNotifications.FirstOrDefault(n => 
                    n.ContractId == contract.ContractId && 
                    n.DaysLate == daysOverdue);

                if (existingNotification == null)
                {
                    // Tạo thông báo mới
                    bool success = _notificationDAL.CreateNotification(
                        notificationId,
                        "OVERDUE_CONTRACT",
                        message,
                        daysOverdue,
                        contract.ContractId,
                        departmentHead.Company_UserId,
                        departmentHead.DepartmentId
                    );

                    if (success)
                    {
                        Console.WriteLine($"Đã tạo thông báo quá hạn: {message}");
                    }
                    else
                    {
                        Console.WriteLine($"Không thể tạo thông báo quá hạn: {message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo thông báo: {ex.Message}");
            }
        }

        /// <summary>
        /// Chạy kiểm tra định kỳ (có thể gọi từ timer hoặc scheduled task)
        /// </summary>
        public void StartPeriodicCheck()
        {
            // Kiểm tra ngay lập tức
            CheckAndSendOverdueNotifications();
        }

    }
}
