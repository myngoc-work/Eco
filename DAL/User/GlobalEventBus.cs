using System;

// Lưu ý: file này chỉ để thỏa mãn tham chiếu cũ trong project cấu hình.
// Không được dùng trong DTO. Bản chính sử dụng trong assembly GUI.
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

