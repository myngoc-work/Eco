using System.Collections.Generic;
using DAL.User;
using DTO;

namespace BLL
{
	public class ContractParameterBLL
	{
		private readonly ContractParameterDAL _dal = new ContractParameterDAL();

		public List<ParameterPlanDTO> GetPlanningParameters(string itemId)
		{
			return _dal.GetPlanningParameters(itemId);
		}

		public List<ParameterResultDTO> GetFieldLabParametersWithResults(string itemId)
		{
			return _dal.GetFieldLabParametersWithResults(itemId);
		}

		public MeasurementInfoDTO GetLatestBatchInfo(string itemId)
		{
			return _dal.GetLatestBatchInfo(itemId);
		}

		public bool DeleteAssignment(string itemId, string parameterId)
		{
			return _dal.DeleteAssignment(itemId, parameterId);
		}

		public bool SavePlanningSetting(string itemId, string parameterId, decimal? minValue, decimal? maxValue, string note, string updatedBy, string sampleType = null)
		{
			return _dal.UpsertItemParameterSetting(itemId, parameterId, minValue, maxValue, note, updatedBy, sampleType);
		}

		public bool SaveAssignment(string itemId, string parameterId, string assignmentCode)
		{
			return _dal.UpsertAssignmentDepartment(itemId, parameterId, assignmentCode);
		}

			public List<ParameterPlanDTO> GetUnassignedParameters(string itemId)
			{
				return _dal.GetUnassignedParameters(itemId);
			}

		public string GetLatestBatchId(string itemId)
		{
			return _dal.GetLatestBatchIdByItem(itemId);
		}

		public bool SaveResult(string batchId, string parameterId, decimal? value, string note, string userId)
		{
			return _dal.UpsertResult(batchId, parameterId, value, note, userId);
		}

		public bool SaveItemSampleType(string itemId, string sampleType)
		{
			var contractDAL = new DAL.ContractDAL();
			return contractDAL.UpdateItemSampleType(itemId, sampleType);
		}

			public string CreateMeasurementBatch(string itemId, System.DateTime startDate, string leaderUserId = null)
			{
				return _dal.CreateMeasurementBatch(itemId, startDate, leaderUserId);
			}

			public string CreateMeasurementBatch(string itemId, System.DateTime startDate)
			{
				return _dal.CreateMeasurementBatch(itemId, startDate, null);
			}
	}
}

