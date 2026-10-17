namespace DTO
{
    // Lớp này dùng để chứa các đặc trưng (features) được tính toán từ DAL để đưa vào mô hình AI.
    public class ContractFeaturesDTO
    {
        public float CompletionTimeInDays { get; set; }
        public float DaysOverdue { get; set; }
        public float NonStandardResultsCount { get; set; }
        public float TotalParametersCount { get; set; }
    }
}