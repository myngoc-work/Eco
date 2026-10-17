using System;

namespace DTO
{
    public class NotificationDTO
    {
        public string NotificationId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? DaysLate { get; set; }
        public string ContractId { get; set; }
        public string BatchId { get; set; }
        public string TargetCompany_UserId { get; set; }
        public string TargetDeptId { get; set; }
        
        // Thông tin hợp đồng liên quan
        public string CustomerName { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string Status { get; set; }
    }
}
