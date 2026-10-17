namespace DTO
{
	public class ContractItemDTO
	{
		public string ItemId { get; set; }
		public string ContractId { get; set; }
		public string SampleCode { get; set; }
		public string SampleName { get; set; }
		public string SampleLocation { get; set; }
		public string SampleType { get; set; }
		public string DisplayText => $"{ItemId} - {SampleName}";
	}
}

