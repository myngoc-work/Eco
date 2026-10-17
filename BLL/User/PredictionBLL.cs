using DTO;
using MLModel; // Quan trọng: Using đến project MLModel
using DAL.User;
using DAL;

namespace BLL.User
{
    public class PredictionBLL
    {
        private ContractDAL _contractDAL = new ContractDAL();

        // Hàm này sẽ được gọi từ GUI để thực hiện dự đoán
        public ModelOutput PredictRenewal(string contractId)
        {
            // Bước 1: Gọi DAL để lấy các đặc trưng (features) của hợp đồng
            ContractFeaturesDTO features = _contractDAL.GetContractFeaturesForPrediction(contractId);

            if (features == null)
            {
                return null; // Không có dữ liệu thì không dự đoán
            }

            // Bước 2: Chuẩn bị dữ liệu đầu vào cho mô hình AI
            var modelInput = new ModelInput()
            {
                CompletionTimeInDays = features.CompletionTimeInDays,
                DaysOverdue = features.DaysOverdue,
                NonStandardResultsCount = features.NonStandardResultsCount,
                TotalParametersCount = features.TotalParametersCount,

                // Các trường này không phải feature nhưng class ModelInput yêu cầu
                ContractId = contractId,
                CustomerId = ""
            };

            // Bước 3: Gọi dịch vụ dự đoán tĩnh từ project MLModel
            return PredictionService.Predict(modelInput);
        }
    }
}