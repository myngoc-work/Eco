using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAL.User
{
	public class ContractParameterDAL
	{
		private readonly DatabaseHelper _dbHelper = new DatabaseHelper();

		public List<ParameterPlanDTO> GetPlanningParameters(string itemId)
		{
			string sql = @"
				SELECT a.ItemId,
				       p.ParameterId,
				       p.ParameterName,
				       u.Symbol AS UnitSymbol,
				       CASE WHEN a.DepartmentId='D004' THEN 'HT' WHEN a.DepartmentId='D005' THEN 'PT' ELSE 'HT' END AS AssignmentCode,
				       COALESCE(ips.SampleType, ci.SampleType) AS SampleType,
				       COALESCE(ips.MinValue, p.MinValue) AS MinValue,
				       COALESCE(ips.MaxValue, p.MaxValue) AS MaxValue,
				       ips.Note
				FROM Assignment a
				JOIN SampleParameter p ON p.ParameterId = a.ParameterId
				JOIN Unit u ON u.UnitId = p.UnitId
				JOIN ContractItem ci ON ci.ItemId = a.ItemId
				LEFT JOIN ItemParameterSetting ips ON ips.ItemId = a.ItemId AND ips.ParameterId = a.ParameterId
				WHERE a.ItemId = @ItemId
				ORDER BY p.ParameterId";

			var list = new List<ParameterPlanDTO>();
			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@ItemId", itemId);
				conn.Open();
				using (var r = cmd.ExecuteReader())
				{
					while (r.Read())
					{
						list.Add(new ParameterPlanDTO
						{
							ItemId = r["ItemId"].ToString(),
							ParameterId = r["ParameterId"].ToString(),
							ParameterName = r["ParameterName"].ToString(),
							UnitSymbol = r["UnitSymbol"].ToString(),
							AssignmentCode = r["AssignmentCode"].ToString(),
							SampleType = r["SampleType"] != DBNull.Value ? r["SampleType"].ToString() : null,
							MinValue = r["MinValue"] != DBNull.Value ? (decimal?)Convert.ToDecimal(r["MinValue"]) : null,
							MaxValue = r["MaxValue"] != DBNull.Value ? (decimal?)Convert.ToDecimal(r["MaxValue"]) : null,
							Note = r["Note"] != DBNull.Value ? r["Note"].ToString() : null
						});
					}
				}
			}
			return list;
		}

		public string CreateMeasurementBatch(string itemId, DateTime startDate, string leaderUserId = null)
		{
			string sql = @"
		DECLARE @NewBatchId VARCHAR(10);
		DECLARE @Retry INT = 0;
		WHILE @Retry < 10
		BEGIN
		    SET @NewBatchId = CONCAT('B', RIGHT(CONCAT('000000', ABS(CHECKSUM(NEWID())) % 1000000), 6));
		    IF NOT EXISTS (SELECT 1 FROM MeasurementBatch WHERE BatchId = @NewBatchId) BREAK;
		    SET @Retry = @Retry + 1;
		END

		INSERT INTO MeasurementBatch(BatchId, ItemId, StartDate, EndDate, Status, LeaderCompany_UserId)
		VALUES(@NewBatchId, @ItemId, @StartDate, NULL, 'Planned', @LeaderId);

		SELECT @NewBatchId;";

			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@ItemId", itemId);
				cmd.Parameters.AddWithValue("@StartDate", startDate);
				cmd.Parameters.AddWithValue("@LeaderId", (object)leaderUserId ?? DBNull.Value);
				conn.Open();
				var dt = new DataTable();
				using (var da = new SqlDataAdapter(cmd))
				{
					da.Fill(dt);
				}
				return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : null;
			}
		}

		public List<ParameterPlanDTO> GetUnassignedParameters(string itemId)
		{
			string sql = @"
				SELECT 
					p.ParameterId,
					p.ParameterName,
					u.Symbol AS UnitSymbol
				FROM SampleParameter p
				JOIN Unit u ON u.UnitId = p.UnitId
				WHERE NOT EXISTS (
					SELECT 1 FROM Assignment a WHERE a.ItemId=@ItemId AND a.ParameterId = p.ParameterId
				)
				ORDER BY p.ParameterId";

			var list = new List<ParameterPlanDTO>();
			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@ItemId", itemId);
				conn.Open();
				using (var r = cmd.ExecuteReader())
				{
					while (r.Read())
					{
						list.Add(new ParameterPlanDTO
						{
							ParameterId = r["ParameterId"].ToString(),
							ParameterName = r["ParameterName"].ToString(),
							UnitSymbol = r["UnitSymbol"].ToString()
						});
					}
				}
			}
			return list;
		}

		public bool UpsertItemParameterSetting(string itemId, string parameterId, decimal? minValue, decimal? maxValue, string note, string updatedBy, string sampleType = null)
		{
			string sql = @"
				MERGE ItemParameterSetting AS tgt
				USING (SELECT @ItemId AS ItemId, @ParameterId AS ParameterId) AS src
				ON (tgt.ItemId = src.ItemId AND tgt.ParameterId = src.ParameterId)
				WHEN MATCHED THEN UPDATE SET MinValue=@MinValue, MaxValue=@MaxValue, Note=@Note, SampleType=@SampleType, UpdatedBy=@UpdatedBy, UpdatedAt=SYSDATETIME()
				WHEN NOT MATCHED THEN INSERT (ItemId, ParameterId, MinValue, MaxValue, Note, SampleType, UpdatedBy) VALUES (@ItemId, @ParameterId, @MinValue, @MaxValue, @Note, @SampleType, @UpdatedBy);";

			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.CommandTimeout = 60; // Tăng timeout lên 60 giây
				cmd.Parameters.AddWithValue("@ItemId", itemId);
				cmd.Parameters.AddWithValue("@ParameterId", parameterId);
				cmd.Parameters.AddWithValue("@MinValue", (object)minValue ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@MaxValue", (object)maxValue ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Note", (object)note ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@SampleType", (object)sampleType ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@UpdatedBy", (object)updatedBy ?? DBNull.Value);
				conn.Open();
				return cmd.ExecuteNonQuery() > 0;
			}
		}

		public bool UpsertAssignmentDepartment(string itemId, string parameterId, string assignmentCode)
		{
			string deptId = assignmentCode == "PT" ? "D005" : "D004"; // default HT=D004
			string sql = @"
				MERGE Assignment AS tgt
				USING (SELECT @ItemId AS ItemId, @ParameterId AS ParameterId) AS src
				ON (tgt.ItemId = src.ItemId AND tgt.ParameterId = src.ParameterId)
				WHEN MATCHED THEN UPDATE SET DepartmentId=@DeptId
				WHEN NOT MATCHED THEN INSERT (ItemId, ParameterId, DepartmentId) VALUES (@ItemId, @ParameterId, @DeptId);";
			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.CommandTimeout = 60; // Tăng timeout lên 60 giây
				cmd.Parameters.AddWithValue("@ItemId", itemId);
				cmd.Parameters.AddWithValue("@ParameterId", parameterId);
				cmd.Parameters.AddWithValue("@DeptId", deptId);
				conn.Open();
				return cmd.ExecuteNonQuery() > 0;
			}
		}

		public string GetLatestBatchIdByItem(string itemId)
		{
			string sql = @"SELECT TOP 1 BatchId FROM MeasurementBatch WHERE ItemId=@ItemId ORDER BY CASE Status WHEN 'InProgress' THEN 1 WHEN 'Planned' THEN 2 WHEN 'Completed' THEN 3 END, StartDate DESC";
			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@ItemId", itemId);
				conn.Open();
				var obj = cmd.ExecuteScalar();
				return obj == null ? null : obj.ToString();
			}
		}

		public bool UpsertResult(string batchId, string parameterId, decimal? value, string note, string userId)
		{
			// Tối ưu: Tạo ResultId trước, tránh WHILE loop trong SQL
			string newResultId = GenerateUniqueResultId();
			
			string sql = @"
				DECLARE @MaxVal DECIMAL(18,4) = (
				    SELECT COALESCE(ips.MaxValue, p.MaxValue)
				    FROM MeasurementBatch b
				    JOIN Assignment a ON a.ItemId = b.ItemId AND a.ParameterId = @ParameterId
				    JOIN SampleParameter p ON p.ParameterId = a.ParameterId
				    LEFT JOIN ItemParameterSetting ips ON ips.ItemId = a.ItemId AND ips.ParameterId = a.ParameterId
				    WHERE b.BatchId = @BatchId
				);

				MERGE Result AS tgt
				USING (SELECT @BatchId AS BatchId, @ParameterId AS ParameterId) AS src
				ON (tgt.BatchId = src.BatchId AND tgt.ParameterId = src.ParameterId)
				WHEN MATCHED THEN UPDATE SET Value=@Value, Note=@Note, Company_UserId=ISNULL(@UserId,'Admin'), ImportAt=SYSDATETIME(), OutofStandard = CASE WHEN @Value IS NOT NULL AND @MaxVal IS NOT NULL AND @Value > @MaxVal THEN 1 ELSE 0 END
				WHEN NOT MATCHED THEN INSERT (ResultId, BatchId, ParameterId, Value, ImportAt, Company_UserId, OutofStandard, Note)
				VALUES (@NewResultId, @BatchId, @ParameterId, @Value, SYSDATETIME(), ISNULL(@UserId,'Admin'), CASE WHEN @Value IS NOT NULL AND @MaxVal IS NOT NULL AND @Value > @MaxVal THEN 1 ELSE 0 END, @Note);";

			if (!value.HasValue)
			{
				// Nếu không có giá trị, bỏ qua lưu để tránh vi phạm NOT NULL
				return true;
			}

			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.CommandTimeout = 60; // Tăng timeout lên 60 giây
				cmd.Parameters.AddWithValue("@BatchId", batchId);
				cmd.Parameters.AddWithValue("@ParameterId", parameterId);
				cmd.Parameters.AddWithValue("@Value", (object)value ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Note", (object)note ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@NewResultId", newResultId);
				conn.Open();
				return cmd.ExecuteNonQuery() > 0;
			}
		}
		
		private string GenerateUniqueResultId()
		{
			// Tạo ResultId unique bằng cách kết hợp timestamp và random
			var random = new Random();
			int retryCount = 0;
			string resultId = null;
			
			while (retryCount < 10)
			{
				string candidate = "KQ" + DateTime.Now.ToString("MMdd") + random.Next(1000, 9999).ToString();
				
				// Kiểm tra xem ID đã tồn tại chưa
				string checkSql = "SELECT COUNT(1) FROM Result WHERE ResultId = @ResultId";
				using (var conn = _dbHelper.GetConnection())
				using (var cmd = new SqlCommand(checkSql, conn))
				{
					cmd.CommandTimeout = 10;
					cmd.Parameters.AddWithValue("@ResultId", candidate);
					conn.Open();
					int count = (int)cmd.ExecuteScalar();
					if (count == 0)
					{
						resultId = candidate;
						break;
					}
				}
				retryCount++;
			}
			
			// Nếu vẫn không tạo được, dùng GUID làm fallback
			if (string.IsNullOrEmpty(resultId))
			{
				resultId = "KQ" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
			}
			
			return resultId;
		}

		public List<ParameterResultDTO> GetFieldLabParametersWithResults(string itemId)
		{
			string sql = @"
				WITH latest_batch AS (
				    SELECT TOP 1 b.BatchId
				    FROM MeasurementBatch b
				    WHERE b.ItemId = @ItemId
				    ORDER BY CASE b.Status WHEN 'InProgress' THEN 1 WHEN 'Planned' THEN 2 WHEN 'Completed' THEN 3 END, b.StartDate DESC
				)
				SELECT a.ItemId,
				       p.ParameterId,
				       p.ParameterName,
				       u.Symbol AS UnitSymbol,
				       CASE WHEN a.DepartmentId='D004' THEN 'HT' WHEN a.DepartmentId='D005' THEN 'PT' ELSE 'HT' END AS AssignmentCode,
				       COALESCE(ips.SampleType, ci.SampleType) AS SampleType,
				       COALESCE(ips.MinValue, p.MinValue) AS MinValue,
				       COALESCE(ips.MaxValue, p.MaxValue) AS MaxValue,
				       r.Value,
				       r.Note
				FROM Assignment a
				JOIN SampleParameter p ON p.ParameterId = a.ParameterId
				JOIN Unit u ON u.UnitId = p.UnitId
				JOIN ContractItem ci ON ci.ItemId = a.ItemId
				LEFT JOIN ItemParameterSetting ips ON ips.ItemId = a.ItemId AND ips.ParameterId = a.ParameterId
				LEFT JOIN latest_batch lb ON 1=1
				LEFT JOIN Result r ON r.BatchId = lb.BatchId AND r.ParameterId = a.ParameterId
				WHERE a.ItemId = @ItemId
				ORDER BY p.ParameterId";

			var list = new List<ParameterResultDTO>();
			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@ItemId", itemId);
				conn.Open();
				using (var r = cmd.ExecuteReader())
				{
					while (r.Read())
					{
						list.Add(new ParameterResultDTO
						{
							ItemId = r["ItemId"].ToString(),
							ParameterId = r["ParameterId"].ToString(),
							ParameterName = r["ParameterName"].ToString(),
							UnitSymbol = r["UnitSymbol"].ToString(),
							AssignmentCode = r["AssignmentCode"].ToString(),
							SampleType = r["SampleType"] != DBNull.Value ? r["SampleType"].ToString() : null,
							MinValue = r["MinValue"] != DBNull.Value ? (decimal?)Convert.ToDecimal(r["MinValue"]) : null,
							MaxValue = r["MaxValue"] != DBNull.Value ? (decimal?)Convert.ToDecimal(r["MaxValue"]) : null,
							Value = r["Value"] != DBNull.Value ? (decimal?)Convert.ToDecimal(r["Value"]) : null,
							Note = r["Note"] != DBNull.Value ? r["Note"].ToString() : null
						});
					}
				}
			}
			return list;
		}

		public MeasurementInfoDTO GetLatestBatchInfo(string itemId)
		{
			string sql = @"
				WITH latest_batch AS (
				    SELECT TOP 1 b.BatchId, b.StartDate, b.EndDate
				    FROM MeasurementBatch b
				    WHERE b.ItemId = @ItemId
				    ORDER BY CASE b.Status WHEN 'InProgress' THEN 1 WHEN 'Planned' THEN 2 WHEN 'Completed' THEN 3 END, b.StartDate DESC
				)
				SELECT lb.BatchId,
				       lb.StartDate,
				       lb.EndDate,
				       MAX(r.ImportAt) AS LastResultUpdatedAt
				FROM latest_batch lb
				LEFT JOIN Result r ON r.BatchId = lb.BatchId
				GROUP BY lb.BatchId, lb.StartDate, lb.EndDate";

			using (var conn = _dbHelper.GetConnection())
			using (var cmd = new SqlCommand(sql, conn))
			{
				cmd.Parameters.AddWithValue("@ItemId", itemId);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return new MeasurementInfoDTO
						{
							BatchId = reader["BatchId"] != DBNull.Value ? reader["BatchId"].ToString() : null,
							StartDate = reader["StartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["StartDate"]) : null,
							EndDate = reader["EndDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["EndDate"]) : null,
							LastResultUpdatedAt = reader["LastResultUpdatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["LastResultUpdatedAt"]) : null
						};
					}
				}
			}
			return null;
		}

		public bool DeleteAssignment(string itemId, string parameterId)
		{
			// Xóa Result liên quan (thông qua MeasurementBatch)
			string sqlDeleteResult = @"
				DELETE FROM Result 
				WHERE BatchId IN (SELECT BatchId FROM MeasurementBatch WHERE ItemId = @ItemId)
				AND ParameterId = @ParameterId";
			
			// Xóa ItemParameterSetting
			string sqlDeleteSetting = @"DELETE FROM ItemParameterSetting WHERE ItemId = @ItemId AND ParameterId = @ParameterId";
			
			// Xóa Assignment
			string sqlDeleteAssignment = @"DELETE FROM Assignment WHERE ItemId = @ItemId AND ParameterId = @ParameterId";

			using (var conn = _dbHelper.GetConnection())
			{
				conn.Open();
				using (var transaction = conn.BeginTransaction())
				{
					try
					{
						// Xóa Result liên quan trước
						using (var cmd1 = new SqlCommand(sqlDeleteResult, conn, transaction))
						{
							cmd1.CommandTimeout = 60; // Tăng timeout
							cmd1.Parameters.AddWithValue("@ItemId", itemId);
							cmd1.Parameters.AddWithValue("@ParameterId", parameterId);
							cmd1.ExecuteNonQuery();
						}

						// Xóa ItemParameterSetting
						using (var cmd2 = new SqlCommand(sqlDeleteSetting, conn, transaction))
						{
							cmd2.CommandTimeout = 60; // Tăng timeout
							cmd2.Parameters.AddWithValue("@ItemId", itemId);
							cmd2.Parameters.AddWithValue("@ParameterId", parameterId);
							cmd2.ExecuteNonQuery();
						}

						// Xóa Assignment
						using (var cmd3 = new SqlCommand(sqlDeleteAssignment, conn, transaction))
						{
							cmd3.CommandTimeout = 60; // Tăng timeout
							cmd3.Parameters.AddWithValue("@ItemId", itemId);
							cmd3.Parameters.AddWithValue("@ParameterId", parameterId);
							int rowsAffected = cmd3.ExecuteNonQuery();
							transaction.Commit();
							return rowsAffected > 0;
						}
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						System.Diagnostics.Debug.WriteLine($"Lỗi khi xóa Assignment: {ex.Message}");
						return false;
					}
				}
			}
		}
	}
}

