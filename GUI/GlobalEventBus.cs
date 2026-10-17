using System;

namespace GUI
{
	public static class GlobalEventBus
	{
		public static event Action ContractDataChanged;
		public static event Action CustomerDataChanged;
		public static event Action OrderDataChanged;
		public static event Action EmployeeDataChanged;

		public static void RaiseContractDataChanged()
		{
			ContractDataChanged?.Invoke();
		}

		public static void RaiseCustomerDataChanged()
		{
			CustomerDataChanged?.Invoke();
		}

		public static void RaiseOrderDataChanged()
		{
			OrderDataChanged?.Invoke();
		}

		public static void RaiseEmployeeDataChanged()
		{
			EmployeeDataChanged?.Invoke();
		}
	}
}

