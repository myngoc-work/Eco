using Microsoft.ML.Data;

namespace MLModel
{
    public class ModelInput
    {
        // Các thuộc tính này phải khớp với các cột trong View SQL của bạn
        [ColumnName("CompletionTimeInDays"), LoadColumn(0)]
        public float CompletionTimeInDays { get; set; }

        [ColumnName("DaysOverdue"), LoadColumn(1)]
        public float DaysOverdue { get; set; }

        [ColumnName("NonStandardResultsCount"), LoadColumn(2)]
        public float NonStandardResultsCount { get; set; }

        [ColumnName("TotalParametersCount"), LoadColumn(3)]
        public float TotalParametersCount { get; set; }

        [ColumnName("ContractId"), LoadColumn(4)]
        public string ContractId { get; set; }

        [ColumnName("CustomerId"), LoadColumn(5)]
        public string CustomerId { get; set; }

        [ColumnName("Renewed"), LoadColumn(6)]
        public bool Renewed { get; set; }
    }
}