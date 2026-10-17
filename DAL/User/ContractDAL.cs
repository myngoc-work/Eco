using DTO;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ContractDAL
    {
        // Lấy danh sách tóm tắt các hợp đồng
        public List<SalesContractDTO> GetContractSummaryList()
        {
            List<SalesContractDTO> contractList = new List<SalesContractDTO>();

            string query = "SELECT c.ContractId, cust.CustomerName, c.PlanDueDate FROM Contract c JOIN Customer cust ON c.CustomerId = cust.CustomerId ORDER BY c.ContractId";

            DataTable data = DataProvider.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                SalesContractDTO contract = new SalesContractDTO
                {
                    // FIX: Đổi tên cột từ "ContractID", "CompanyName", "ExpectedResultDate" cho khớp với code DTO và câu query
                    ContractID = row["ContractId"].ToString(),
                    CompanyName = row["CustomerName"].ToString(),
                    ExpectedResultDate = Convert.ToDateTime(row["PlanDueDate"])
                };
                contractList.Add(contract);
            }
            return contractList;
        }

        // Lấy thông tin chi tiết của một hợp đồng
        public SalesContractDTO GetContractDetails(string contractID)
        {
            SalesContractDTO contract = null;
            // FIX: Đã đổi tên bảng từ "Contracts" thành "Contract" và "Customers" thành "Customer"
            string query = @"SELECT c.ContractId, cust.CustomerId, cust.CustomerName, cust.Address, cust.RepName, cust.RepEmail, cust.RepPhone, c.SignDate, c.PlanDueDate
                             FROM Contract c
                             JOIN Customer cust ON c.CustomerId = cust.CustomerId
                             WHERE c.ContractId = @ContractID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ContractID", contractID)
            };

            DataTable data = DataProvider.ExecuteQuery(query, parameters);
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                contract = new SalesContractDTO
                {
                    // FIX: Đổi tên các cột cho khớp với code DTO và câu query
                    ContractID = row["ContractId"].ToString(),
                    CustomerID = row["CustomerId"].ToString(),
                    CompanyName = row["CustomerName"].ToString(),
                    Address = row["Address"].ToString(),
                    RepresentativeName = row["RepName"].ToString(),
                    RepresentativeEmail = row["RepEmail"] != DBNull.Value ? row["RepEmail"].ToString() : null,
                    PhoneNumber = row["RepPhone"].ToString(),
                    SigningDate = Convert.ToDateTime(row["SignDate"]),
                    ExpectedResultDate = Convert.ToDateTime(row["PlanDueDate"])
                };
            }
            return contract;
        }

        // Cập nhật thông tin hợp đồng và khách hàng
        public bool UpdateContractAndCustomer(SalesContractDTO contract)
        {
            // FIX: Đã đổi tên bảng từ "Customers" thành "Customer"
            string queryCustomer = "UPDATE Customer SET CustomerName = @CompanyName, Address = @Address, RepName = @RepName, RepEmail = @RepEmail, RepPhone = @Phone WHERE CustomerId = @CustomerID";
            SqlParameter[] paramCustomer = new SqlParameter[]
            {
                new SqlParameter("@CompanyName", contract.CompanyName),
                new SqlParameter("@Address", contract.Address),
                new SqlParameter("@RepName", contract.RepresentativeName),
                new SqlParameter("@RepEmail", contract.RepresentativeEmail ?? (object)DBNull.Value),
                new SqlParameter("@Phone", contract.PhoneNumber),
                new SqlParameter("@CustomerID", contract.CustomerID)
            };
            int resultCustomer = DataProvider.ExecuteNonQuery(queryCustomer, paramCustomer);

            // FIX: Đã đổi tên bảng từ "Contracts" thành "Contract"
            string queryContract = "UPDATE Contract SET SignDate = @SignDate, PlanDueDate = @ExpectedDate WHERE ContractId = @ContractID";
            SqlParameter[] paramContract = new SqlParameter[]
            {
                new SqlParameter("@SignDate", contract.SigningDate),
                new SqlParameter("@ExpectedDate", contract.ExpectedResultDate),
                new SqlParameter("@ContractID", contract.ContractID)
            };
            int resultContract = DataProvider.ExecuteNonQuery(queryContract, paramContract);

            return resultCustomer > 0 && resultContract > 0;
        }

        /// <summary>
        /// Tự động cập nhật trạng thái hợp đồng thành 'Done' khi đến ngày trả kết quả
        /// Và normalize các status tiếng Việt "Đã hoàn thành", "Hoàn thành" thành "Done"
        /// </summary>
        private void AutoCompleteContractsByDueDate()
        {
            // 1. Tự động hoàn thành các hợp đồng đã đến ngày trả kết quả
            string queryAutoComplete = @"
                UPDATE Contract
                SET Status = 'Done'
                WHERE PlanDueDate IS NOT NULL
                  AND PlanDueDate <= CAST(GETDATE() AS DATE)
                  AND Status IN ('InProgress', 'Planned')";

            // 2. Normalize các status tiếng Việt "Đã hoàn thành", "Hoàn thành" thành "Done"
            string queryNormalize = @"
                UPDATE Contract
                SET Status = 'Done'
                WHERE Status IN (N'Đã hoàn thành', N'Hoàn thành', N'Thành công', 'Completed')";

            try
            {
                // Thực hiện tự động hoàn thành
                DataProvider.ExecuteNonQuery(queryAutoComplete, new SqlParameter[0]);
                
                // Normalize các status tiếng Việt
                DataProvider.ExecuteNonQuery(queryNormalize, new SqlParameter[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tự động cập nhật trạng thái hợp đồng hoàn thành: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật trạng thái hợp đồng
        /// </summary>
        public bool UpdateContractStatus(string contractId, string status)
        {
            string query = "UPDATE Contract SET Status = @Status WHERE ContractId = @ContractId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@ContractId", contractId)
            };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        // Lấy tất cả hợp đồng
        public List<ContractDTO> GetAllContracts()
        {
            // Tự động hoàn thành các hợp đồng đã đến ngày trả kết quả
            AutoCompleteContractsByDueDate();
            
            List<ContractDTO> contracts = new List<ContractDTO>();
            
            string query = @"
                SELECT c.ContractId, c.CustomerId, cu.CustomerName, c.SignDate, 
                       c.PlanDueDate, c.Status, c.SalesCompany_UserId,
                       ISNULL(c.Amount, 0) AS TotalAmount
                FROM Contract c
                LEFT JOIN Customer cu ON c.CustomerId = cu.CustomerId
                ORDER BY c.ContractId";

            try
            {
                DataTable data = DataProvider.ExecuteQuery(query);
                foreach (DataRow row in data.Rows)
                {
                    var contract = new ContractDTO
                    {
                        ContractId = row["ContractId"].ToString(),
                        CustomerId = row["CustomerId"].ToString(),
                        CustomerName = row["CustomerName"] != DBNull.Value ? row["CustomerName"].ToString() : null,
                        SignDate = (DateTime)row["SignDate"],
                        OrderDate = (DateTime)row["SignDate"], // Sử dụng SignDate làm OrderDate
                        PlanDueDate = row["PlanDueDate"] != DBNull.Value ? (DateTime?)row["PlanDueDate"] : null,
                        StartDate = null,
                        Status = row["Status"].ToString(),
                        SalesCompany_UserId = row["SalesCompany_UserId"].ToString(),
                        TotalAmount = row["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(row["TotalAmount"]) : 0
                    };
                    contracts.Add(contract);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách hợp đồng: {ex.Message}");
            }

            return contracts;
        }

        public DataTable GetDailyRevenue(int year, int month)
        {
            string query = @"SELECT DAY(SignDate) AS [Day], SUM(ISNULL(Amount,0)) AS Total
                             FROM Contract
                             WHERE YEAR(SignDate) = @Y AND MONTH(SignDate) = @M AND Status <> 'Cancelled'
                             GROUP BY DAY(SignDate)
                             ORDER BY DAY(SignDate)";
            var prms = new SqlParameter[]
            {
                new SqlParameter("@Y", year),
                new SqlParameter("@M", month)
            };
            return DataProvider.ExecuteQuery(query, prms);
        }

        public DataTable GetQuarterlyRevenue(int year)
        {
            string query = @"SELECT DATEPART(QUARTER, SignDate) AS Quarter, SUM(ISNULL(Amount,0)) AS Total
                             FROM Contract
                             WHERE YEAR(SignDate) = @Y AND Status <> 'Cancelled'
                             GROUP BY DATEPART(QUARTER, SignDate)
                             ORDER BY DATEPART(QUARTER, SignDate)";
            var prms = new SqlParameter[]
            {
                new SqlParameter("@Y", year)
            };
            return DataProvider.ExecuteQuery(query, prms);
        }

        // Lấy danh sách nền mẫu theo hợp đồng
        public List<ContractItemDTO> GetItemsByContract(string contractId)
        {
            var items = new List<ContractItemDTO>();
            string query = @"SELECT ItemId, ContractId, SampleCode, SampleName, SampleLocation, SampleType FROM ContractItem WHERE ContractId = @ContractId ORDER BY ItemId";
            try
            {
                SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@ContractId", contractId) };
                DataTable dt = DataProvider.ExecuteQuery(query, prms);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new ContractItemDTO
                    {
                        ItemId = row["ItemId"].ToString(),
                        ContractId = row["ContractId"].ToString(),
                        SampleCode = row["SampleCode"].ToString(),
                        SampleName = row["SampleName"].ToString(),
                        SampleLocation = row["SampleLocation"].ToString(),
                        SampleType = row["SampleType"] != DBNull.Value ? row["SampleType"].ToString() : null
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy ContractItem: {ex.Message}");
            }
            return items;
        }

        // Cập nhật loại mẫu cho ContractItem
        public bool UpdateItemSampleType(string itemId, string sampleType)
        {
            string query = "UPDATE ContractItem SET SampleType = @SampleType WHERE ItemId = @ItemId";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@SampleType", string.IsNullOrEmpty(sampleType) ? (object)DBNull.Value : sampleType)
            };
            try
            {
                return DataProvider.ExecuteNonQuery(query, prms) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật SampleType: {ex.Message}");
                return false;
            }
        }

        // Thêm nền mẫu mới cho hợp đồng, tự sinh ItemId và SampleCode
        public string AddContractItem(string contractId, string sampleName, string sampleType = null, string sampleLocation = null)
        {
            // Xác định prefix dựa trên loại mẫu
            string prefix = "IT"; // Mặc định nếu không có loại mẫu
            if (!string.IsNullOrWhiteSpace(sampleType))
            {
                switch (sampleType.Trim())
                {
                    case "Khí thải":
                        prefix = "KT";
                        break;
                    case "Nước thải":
                        prefix = "NT";
                        break;
                    case "Không khí":
                        prefix = "KK";
                        break;
                    case "Tiếng ồn":
                        prefix = "TN";
                        break;
                }
            }

            // Sử dụng parameter để tránh SQL injection và xử lý prefix động
            string sql = @"
DECLARE @NewItemId VARCHAR(10);
DECLARE @NextNum INT;
DECLARE @Retry INT = 0;
DECLARE @Prefix VARCHAR(2) = @SampleTypePrefix;

-- Tìm số tiếp theo cho loại mẫu này
SELECT @NextNum = ISNULL(MAX(CAST(SUBSTRING(ItemId, 3, LEN(ItemId) - 2) AS INT)), 0) + 1
FROM ContractItem
WHERE ItemId LIKE @Prefix + '%' AND ISNUMERIC(SUBSTRING(ItemId, 3, LEN(ItemId) - 2)) = 1;

-- Tạo ItemId mới
SET @NewItemId = @Prefix + RIGHT('00' + CAST(@NextNum AS VARCHAR(2)), 2);

-- Kiểm tra xem ItemId đã tồn tại chưa, nếu có thì tăng số
WHILE EXISTS (SELECT 1 FROM ContractItem WHERE ItemId = @NewItemId) AND @Retry < 100
BEGIN
    SET @NextNum = @NextNum + 1;
    SET @NewItemId = @Prefix + RIGHT('00' + CAST(@NextNum AS VARCHAR(2)), 2);
    SET @Retry = @Retry + 1;
END

INSERT INTO ContractItem(ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType)
VALUES(@NewItemId, @ContractId, @NewItemId, @SampleName, @SampleLocation, 'D003', @SampleType);

SELECT @NewItemId;";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@ContractId", contractId),
                new SqlParameter("@SampleName", sampleName ?? (object)DBNull.Value),
                new SqlParameter("@SampleLocation", (object)sampleLocation ?? DBNull.Value),
                new SqlParameter("@SampleType", (object)sampleType ?? DBNull.Value),
                new SqlParameter("@SampleTypePrefix", prefix)
            };
            try
            {
                var dt = DataProvider.ExecuteQuery(sql, prms);
                return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm ContractItem: {ex.Message}");
                return null;
            }
        }
        
        public bool DeleteContractItem(string itemId)
        {
            // Xóa ContractItem sẽ tự động xóa các bản ghi liên quan nhờ CASCADE
            string sql = "DELETE FROM ContractItem WHERE ItemId = @ItemId";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@ItemId", itemId)
            };
            try
            {
                int rowsAffected = DataProvider.ExecuteNonQuery(sql, prms);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa ContractItem: {ex.Message}");
                return false;
            }
        }

        // =====================================================================
        // ========== PHẦN CODE MỚI THÊM CHO CHỨC NĂNG DỰ ĐOÁN AI ============
        // =====================================================================

        /// <summary>
        /// Lấy các đặc trưng của một hợp đồng cụ thể từ View SQL để đưa vào mô hình AI.
        /// </summary>
        public ContractFeaturesDTO GetContractFeaturesForPrediction(string contractId)
        {
            // Truy vấn trực tiếp từ View đã được tạo sẵn
            string query = @"
                SELECT 
                    CompletionTimeInDays,
                    DaysOverdue,
                    NonStandardResultsCount,
                    TotalParametersCount
                FROM 
                    V_Contract_History_For_AI
                WHERE 
                    ContractId = @ContractId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ContractId", contractId)
            };

            DataTable data = DataProvider.ExecuteQuery(query, parameters);
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return new ContractFeaturesDTO
                {
                    CompletionTimeInDays = Convert.ToSingle(row["CompletionTimeInDays"]),
                    DaysOverdue = Convert.ToSingle(row["DaysOverdue"]),
                    NonStandardResultsCount = Convert.ToSingle(row["NonStandardResultsCount"]),
                    TotalParametersCount = Convert.ToSingle(row["TotalParametersCount"])
                };
            }
            return null;
        }

        /// <summary>
        /// Lấy hợp đồng vừa hoàn thành gần đây nhất để làm ví dụ dự đoán trên Dashboard.
        /// </summary>
        public ContractDTO GetLatestCompletedContract()
        {
            string query = @"
                SELECT TOP 1 c.ContractId, c.CustomerId, cu.CustomerName
                FROM Contract c
                JOIN Customer cu ON c.CustomerId = cu.CustomerId
                WHERE c.Status = 'Done'
                ORDER BY c.PlanDueDate DESC"; // Lấy hợp đồng có trạng thái 'Done' gần nhất

            DataTable data = DataProvider.ExecuteQuery(query);
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return new ContractDTO
                {
                    ContractId = row["ContractId"].ToString(),
                    CustomerId = row["CustomerId"].ToString(),
                    CustomerName = row["CustomerName"].ToString()
                };
            }
            return null;
        }

        /// <summary>
        /// Lấy danh sách TOP N hợp đồng đã hoàn thành gần đây nhất để dự đoán trên Dashboard.
        /// </summary>
        public List<ContractDTO> GetTopCompletedContracts(int topCount = 3)
        {
            string query = $@"
                SELECT TOP {topCount} c.ContractId, c.CustomerId, cu.CustomerName
                FROM Contract c
                JOIN Customer cu ON c.CustomerId = cu.CustomerId
                WHERE c.Status = 'Done'
                ORDER BY c.PlanDueDate DESC"; // Lấy TOP N hợp đồng có trạng thái 'Done' gần nhất

            DataTable data = DataProvider.ExecuteQuery(query);
            List<ContractDTO> contracts = new List<ContractDTO>();

            foreach (DataRow row in data.Rows)
            {
                contracts.Add(new ContractDTO
                {
                    ContractId = row["ContractId"].ToString(),
                    CustomerId = row["CustomerId"].ToString(),
                    CustomerName = row["CustomerName"].ToString()
                });
            }

            return contracts;
        }
    }

}

