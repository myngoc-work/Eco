using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;

// Namespace của dự án TrainingApp
namespace TrainingApp
{
    // Class Program chính
    public static class Program
    {
        // Chuỗi kết nối đến CSDL (ĐÃ SỬA LỖI)
        private const string ConnectionString = @"Data Source=JACKYE369\SQLEXPRESS01;Initial Catalog=QLQTMT;Integrated Security=True";
        private const string CommandString = @"SELECT [CompletionTimeInDays], [DaysOverdue], [NonStandardResultsCount], [TotalParametersCount], [ContractId], [CustomerId], [Renewed] FROM [dbo].[AI_TrainingData]";
        // Đường dẫn nơi file model.zip sẽ được lưu
        private static readonly string ModelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MLModel1.zip");

        // Phương thức chính để thực thi
        public static void Main(string[] args)
        {
            var mlContext = new MLContext();

            try
            {
                // Bước 1: Tải dữ liệu từ SQL Server
                Console.WriteLine("--> Step 1: Loading data from SQL Server...");
                IDataView dataView = LoadDataFromSQL(mlContext);
                Console.WriteLine("     Data loaded successfully.\n");

                // Bước 2: Xây dựng và Huấn luyện mô hình
                Console.WriteLine("--> Step 2: Building and training the model...");
                ITransformer model = BuildAndTrainModel(mlContext, dataView);
                Console.WriteLine("     Model trained successfully.\n");

                // Bước 3: Lưu mô hình ra file .zip
                Console.WriteLine($"--> Step 3: Saving the model...");
                mlContext.Model.Save(model, dataView.Schema, ModelPath);
                Console.WriteLine("     Model saved successfully.");
                Console.WriteLine($"     File can be found at: {ModelPath}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAn error occurred: {ex.Message}");

                // In ra lỗi thật sự (Inner Exception) để gỡ lỗi
                Exception inner = ex.InnerException;
                int level = 1;
                while (inner != null)
                {
                    Console.WriteLine($"\n--- Inner Exception (Level {level}) ---");
                    Console.WriteLine($"Message: {inner.Message}");
                    Console.WriteLine($"Stack Trace: {inner.StackTrace}\n");
                    inner = inner.InnerException;
                    level++;
                }
                Console.ResetColor();
            }

            Console.WriteLine("\nTraining process finished. Press any key to exit.");
            Console.ReadKey();
        }

        // Hàm tải dữ liệu từ SQL
        public static IDataView LoadDataFromSQL(MLContext mlContext)
        {
            var dbSource = new DatabaseSource(SqlClientFactory.Instance, ConnectionString, CommandString);
            var loader = mlContext.Data.CreateDatabaseLoader<ModelInput>();
            IDataView data = loader.Load(dbSource);
            return data;
        }

        // Hàm huấn luyện
        public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);
            return model;
        }

        // Hàm xây dựng pipeline (ĐÃ SỬA LỖI HOÀN CHỈNH)
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            var pipeline = mlContext.Transforms.ReplaceMissingValues(new[] {
                    new InputOutputColumnPair(@"CompletionTimeInDays", @"CompletionTimeInDays"),
                    new InputOutputColumnPair(@"DaysOverdue", @"DaysOverdue"),
                    new InputOutputColumnPair(@"NonStandardResultsCount", @"NonStandardResultsCount"),
                    new InputOutputColumnPair(@"TotalParametersCount", @"TotalParametersCount")
                })

                // Bước 1: Chuyển đổi các cột ID dạng text thành dạng số (key).
                .Append(mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "ContractId", outputColumnName: "ContractIdEncoded"))
                .Append(mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "CustomerId", outputColumnName: "CustomerIdEncoded"))

                // Bước 2 (MỚI): Áp dụng One-Hot Encoding.
                // Chuyển đổi các cột "Key" đã tạo thành các vector nhị phân.
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "ContractIdEncoded", outputColumnName: "ContractIdOneHot"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(inputColumnName: "CustomerIdEncoded", outputColumnName: "CustomerIdOneHot"))

                // Bước 3: Gộp tất cả các cột đặc trưng số VÀ các cột vector mới vào một cột "Features" duy nhất.
                .Append(mlContext.Transforms.Concatenate("Features",
                    "CompletionTimeInDays",
                    "DaysOverdue",
                    "NonStandardResultsCount",
                    "TotalParametersCount",
                    "ContractIdOneHot",  // <-- Sử dụng cột đã được mã hóa One-Hot
                    "CustomerIdOneHot"   // <-- Sử dụng cột đã được mã hóa One-Hot
                    ))

                // Bước 4: Thêm thuật toán huấn luyện.
                .Append(mlContext.BinaryClassification.Trainers.FastTree(new FastTreeBinaryTrainer.Options()
                {
                    NumberOfLeaves = 4,
                    MinimumExampleCountPerLeaf = 20,
                    NumberOfTrees = 4,
                    MaximumBinCountPerFeature = 254,
                    FeatureFraction = 1,
                    LearningRate = 0.09999999999999998,
                    LabelColumnName = @"Renewed",
                    FeatureColumnName = @"Features"
                }));

            return pipeline;
        }

        // Định nghĩa lớp ModelInput để ML.NET biết cách đọc dữ liệu từ SQL
        public class ModelInput
        {
            [ColumnName("CompletionTimeInDays")]
            public float CompletionTimeInDays { get; set; }

            [ColumnName("DaysOverdue")]
            public float DaysOverdue { get; set; }

            [ColumnName("NonStandardResultsCount")]
            public float NonStandardResultsCount { get; set; }

            [ColumnName("TotalParametersCount")]
            public float TotalParametersCount { get; set; }

            [ColumnName("ContractId")]
            public string ContractId { get; set; }

            [ColumnName("CustomerId")]
            public string CustomerId { get; set; }

            [ColumnName("Renewed")]
            public bool Renewed { get; set; }
        }
    }
}