using System;

namespace DTO.User
{
    public class ContractDTO
    {
        public string ContractId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime SignDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string Status { get; set; }
        public string SalesCompany_UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
