using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class NotificationCard : UserControl
    {
        public string ContractId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int? DaysOverdue { get; set; }

        public NotificationCard()
        {
            InitializeComponent();
        }

        public NotificationCard(string contractId, string customerName, DateTime? expectedDate, int? daysOverdue) : this()
        {
            ContractId = contractId;
            CustomerName = customerName;
            ExpectedDate = expectedDate;
            DaysOverdue = daysOverdue;
            
            LoadData();
        }

        private void LoadData()
        {
            lblContractId.Text = $"{ContractId} - {CustomerName}";
            lblExpectedDate.Text = ExpectedDate.HasValue ? $"Ngày dự kiến trả kết quả: {ExpectedDate.Value:dd/MM/yyyy}" : "Ngày dự kiến trả kết quả: Chưa xác định";
            lblStatus.Text = "Tình trạng: ";
            lblOverdue.Text = DaysOverdue.HasValue ? $"Quá hạn {DaysOverdue.Value} ngày" : "Chưa xác định";
        }

        private void NotificationCard_Load(object sender, EventArgs e)
        {

        }
    }
}
