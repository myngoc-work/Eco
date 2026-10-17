using System;

namespace DTO
{
    public class SalesContractDTO
    {
        // Dùng cho cả danh sách và chi tiết
        public string ContractID { get; set; }
        public string CompanyName { get; set; }
        public DateTime ExpectedResultDate { get; set; }

        // Dùng cho phần chi tiết
        public string CustomerID { get; set; }
        public string Address { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeEmail { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime SigningDate { get; set; }
    }
}