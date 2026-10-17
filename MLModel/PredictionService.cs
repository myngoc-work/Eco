using Microsoft.ML;
using System;
using System.IO;

namespace MLModel
{
    public static class PredictionService
    {
        private static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> _predictionEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(CreatePredictionEngine);

        // Hàm này chỉ chạy một lần để tải mô hình
        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine()
        {
            var mlContext = new MLContext();
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MLModel1.zip");

            // Kiểm tra xem file model có tồn tại không
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"Model file not found at {modelPath}");
            }

            ITransformer mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        // Hàm để gọi dự đoán từ bên ngoài
        public static ModelOutput Predict(ModelInput input)
        {
            return _predictionEngine.Value.Predict(input);
        }
    }
}