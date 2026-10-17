using System;

namespace DTO
{
    public class OrderDTO
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime SampleStartDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public string Status { get; set; }
        public string PersonInCharge { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactName { get; set; }
    }
}