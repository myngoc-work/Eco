using System;

namespace DTO
{
	public class ParameterPlanDTO
	{
		public string ItemId { get; set; }
		public string ParameterId { get; set; }
		public string ParameterName { get; set; }
		public string UnitSymbol { get; set; }
		public string AssignmentCode { get; set; } // HT/PT
		public string SampleType { get; set; } // Loại mẫu
		public decimal? MinValue { get; set; }
		public decimal? MaxValue { get; set; }
		public string Note { get; set; }
	}

	public class ParameterResultDTO
	{
		public string ItemId { get; set; }
		public string ParameterId { get; set; }
		public string ParameterName { get; set; }
		public string UnitSymbol { get; set; }
		public string AssignmentCode { get; set; } // HT/PT
		public string SampleType { get; set; } // Loại mẫu
		public decimal? MinValue { get; set; }
		public decimal? MaxValue { get; set; }
		public decimal? Value { get; set; }
		public string Note { get; set; }
	}
}

