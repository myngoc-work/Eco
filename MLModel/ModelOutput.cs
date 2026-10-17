using Microsoft.ML.Data;

namespace MLModel
{
    public class ModelOutput
    {
        // Kết quả dự đoán (True/False)
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        // Điểm số thô của mô hình, dùng để tính xác suất
        [ColumnName("Score")]
        public float Score { get; set; }

        // Xác suất của các lớp (không bắt buộc dùng ngay nhưng nên có)
        [ColumnName("Probability")]
        public float Probability { get; set; }
    }
}