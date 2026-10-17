using DAL.User;
using DTO;
using DTO.User;
using System.Collections.Generic;
using DAL;

namespace BLL.User
{
    public class ContractBLL
    {
        private ContractDAL dal = new ContractDAL();

        public List<SalesContractDTO> GetContractSummaryList()
        {
            return dal.GetContractSummaryList();
        }

        public SalesContractDTO GetContractDetails(string contractID)
        {
            return dal.GetContractDetails(contractID);
        }

        public bool UpdateContract(SalesContractDTO contract)
        {
            // Có thể thêm các logic kiểm tra dữ liệu ở đây trước khi gọi DAL
            // Ví dụ: if (contract.SigningDate > contract.ExpectedResultDate) return false;
            return dal.UpdateContractAndCustomer(contract);
        }

        /// <summary>
        /// Cập nhật trạng thái hợp đồng
        /// </summary>
        public bool UpdateContractStatus(string contractId, string status)
        {
            return dal.UpdateContractStatus(contractId, status);
        }

        public List<ContractItemDTO> GetItemsByContract(string contractId)
        {
            return dal.GetItemsByContract(contractId);
        }

        public string AddContractItem(string contractId, string sampleName, string sampleType = null, string sampleLocation = null)
        {
            return dal.AddContractItem(contractId, sampleName, sampleType, sampleLocation);
        }
        
        public bool DeleteContractItem(string itemId)
        {
            return dal.DeleteContractItem(itemId);
        }

        public List<ContractDTO> GetAllContracts()
        {
            return dal.GetAllContracts();
        }

        public System.Data.DataTable GetDailyRevenue(int year, int month)
        {
            return dal.GetDailyRevenue(year, month);
        }

        public System.Data.DataTable GetQuarterlyRevenue(int year)
        {
            return dal.GetQuarterlyRevenue(year);
        }

        // Phương thức mới để gọi xuống DAL, lấy hợp đồng gần nhất cho việc dự đoán
        public ContractDTO GetLatestCompletedContract()
        {
            return dal.GetLatestCompletedContract();
        }

        // Phương thức mới để lấy TOP N hợp đồng đã hoàn thành gần đây nhất
        public List<ContractDTO> GetTopCompletedContracts(int topCount = 3)
        {
            return dal.GetTopCompletedContracts(topCount);
        }
    }
}