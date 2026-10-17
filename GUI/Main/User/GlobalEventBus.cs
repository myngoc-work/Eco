using System;

namespace GUI
{
	public static class GlobalEventBus
	{
		public static event Action ContractDataChanged;

		public static void RaiseContractDataChanged()
		{
			ContractDataChanged?.Invoke();
		}
	}
}

