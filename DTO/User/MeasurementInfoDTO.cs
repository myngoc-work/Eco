using System;

namespace DTO
{
    public class MeasurementInfoDTO
    {
        public string BatchId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? LastResultUpdatedAt { get; set; }
    }
}

