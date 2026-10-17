using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BLL.User;
using DTO.User;
using Guna.UI2.WinForms;
using GUI;
using GUI.Main;

namespace GUI.Main.User
{
    public partial class Statistics : UserControl
    {
        private readonly ContractBLL contractBLL;
        private List<ContractDTO> allContracts;
        private DateTime currentFromDate;
        private DateTime currentToDate;
        private bool _isCustomFilterApplied;

        public Statistics()
        {
            InitializeComponent();
            contractBLL = new ContractBLL();
            ApplyLanguage(Thread.CurrentThread.CurrentUICulture.Name);

            // Set default date range - current month
            var defaultRange = GetDefaultRange();
            currentFromDate = defaultRange.from;
            currentToDate = defaultRange.to;
            dtpFrom.Value = currentFromDate;
            dtpTo.Value = currentToDate;
            _isCustomFilterApplied = false;
            
            // Set default group by
            cmbGroupBy.SelectedIndex = 0;
            
            // Subscribe to contract data changes
            GlobalEventBus.ContractDataChanged += OnContractDataChanged;
            this.Disposed += (s, e) => { GlobalEventBus.ContractDataChanged -= OnContractDataChanged; };
        }

        private void Statistics_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void OnContractDataChanged()
        {
            // Reload data when contract changes - ensure UI thread
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(LoadData));
            }
            else
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                // Load all contracts
                allContracts = contractBLL.GetAllContracts() ?? new List<ContractDTO>();
                
                EnsureCurrentRange();
                var filtered = FilterContractsByRange(allContracts, currentFromDate, currentToDate);
                
                // Update cards
                if (_isCustomFilterApplied)
                {
                    UpdateCards(filtered, true);
                }
                else
                {
                    UpdateCards(allContracts, false);
                }
                
                // Update charts
                UpdateCharts(filtered, currentFromDate, currentToDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ApplyLanguage(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            var rm = new ResourceManager("GUI.Resources.Lang", typeof(HomeForm).Assembly);

            lblFrom.Text = rm.GetString("Statistics_Filter_From") ?? "Từ:";
            lblTo.Text = rm.GetString("Statistics_Filter_To") ?? "Đến:";
            btnStatistics.Text = rm.GetString("Statistics_Filter_Apply") ?? "Thống kê";
            btnClear.Text = rm.GetString("Statistics_Filter_Clear") ?? "Xóa";

            lblGroupBy.Text = rm.GetString("Statistics_RevenueViewLabel") ?? "Nhóm theo:";

            string monthText = rm.GetString("Statistics_RevenueView_Month") ?? "Tháng";
            string quarterText = rm.GetString("Statistics_RevenueView_Quarter") ?? "Quý";
            int previousIndex = cmbGroupBy.SelectedIndex;
            cmbGroupBy.Items.Clear();
            cmbGroupBy.Items.Add(monthText);
            cmbGroupBy.Items.Add(quarterText);
            if (previousIndex >= 0 && previousIndex < cmbGroupBy.Items.Count)
            {
                cmbGroupBy.SelectedIndex = previousIndex;
            }
            else
            {
                cmbGroupBy.SelectedIndex = 0;
            }

            lblSuccessTitle.Text = rm.GetString("SuccessTitle") ?? lblSuccessTitle.Text;
            lblInProgressTitle.Text = rm.GetString("InProgressTitle") ?? lblInProgressTitle.Text;
            lblOverdueTitle.Text = rm.GetString("OverdueTitle") ?? lblOverdueTitle.Text;
            lblReturningTitle.Text = rm.GetString("ReturningTitle") ?? lblReturningTitle.Text;

            string revenueTitle = rm.GetString("Revenue") ?? "Doanh thu";
            string ordersTitle = rm.GetString("Statistics_OrderChartTitle") ?? "Biểu đồ đơn hàng";

            chartRevenue.Titles.Clear();
            chartRevenue.Titles.Add(new Title
            {
                Text = revenueTitle,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(28, 99, 63)
            });

            chartOrders.Titles.Clear();
            chartOrders.Titles.Add(new Title
            {
                Text = ordersTitle,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(28, 99, 63)
            });
        }

        private static bool StatusContains(string status, params string[] keywords)
        {
            if (string.IsNullOrWhiteSpace(status) || keywords == null || keywords.Length == 0)
            {
                return false;
            }

            string normalized = status.Trim().ToLowerInvariant();
            foreach (var keyword in keywords)
            {
                if (!string.IsNullOrWhiteSpace(keyword) && normalized.Contains(keyword.ToLowerInvariant()))
                {
                    return true;
                }
            }
            return false;
        }

        private List<ContractDTO> FilterContractsByRange(List<ContractDTO> source, DateTime fromDate, DateTime toDate)
        {
            if (source == null)
            {
                return new List<ContractDTO>();
            }

            return source
                .Where(c => c.OrderDate != DateTime.MinValue &&
                            c.OrderDate.Date >= fromDate &&
                            c.OrderDate.Date <= toDate)
                .ToList();
        }

        private void UpdateCards(List<ContractDTO> contracts, bool useFilteredRange)
        {
            contracts = contracts ?? new List<ContractDTO>();
            DateTime today = DateTime.Today;

            int successCount;
            int inProgressCount;
            int overdueCount;

            if (useFilteredRange)
            {
                successCount = contracts.Count(c => StatusContains(c.Status, "done", "success", "thành công", "completed", "hoàn tất", "đã hoàn thành"));
                inProgressCount = contracts.Count(c => StatusContains(
                    c.Status,
                    "progress",
                    "in progress",
                    "processing",
                    "đang thực hiện",
                    "đang xử lý",
                    "inprogress"));

                overdueCount = contracts.Count(c =>
                    !StatusContains(c.Status, "done", "success", "thành công", "completed", "hoàn tất", "đã hoàn thành") &&
                    c.PlanDueDate.HasValue &&
                    c.PlanDueDate.Value.Date < today);
            }
            else
            {
                successCount = contracts.Count(c => IsCompletedStatus(c.Status) && IsWithinCurrentMonth(c));
                overdueCount = contracts.Count(c => IsOverdueContract(c, today) && IsWithinCurrentMonth(c));
                inProgressCount = contracts.Count(c => !IsCompletedStatus(c.Status) && !IsOverdueContract(c, today) && IsWithinCurrentMonth(c));
            }
            
            var customerGroups = contracts.GroupBy(c => c.CustomerId);
            int returningCount = customerGroups.Count(g => g.Count() > 1);
            
            lblSuccessCount.Text = successCount.ToString();
            lblInProgressCount.Text = inProgressCount.ToString();
            lblOverdueCount.Text = overdueCount.ToString();
            lblReturningCount.Text = returningCount.ToString();
        }

        private void UpdateCharts(List<ContractDTO> contracts, DateTime fromDate, DateTime toDate)
        {
            bool groupByMonth = cmbGroupBy.SelectedIndex == 0;
            
            if (groupByMonth)
            {
                UpdateChartsByMonth(contracts, fromDate, toDate);
            }
            else
            {
                UpdateChartsByQuarter(contracts, fromDate, toDate);
            }
        }

        private (DateTime from, DateTime to) GetDefaultRange()
        {
            var now = DateTime.Now.Date;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            return (firstDay, now);
        }

        private void EnsureCurrentRange()
        {
            if (!_isCustomFilterApplied)
            {
                var range = GetDefaultRange();
                currentFromDate = range.from;
                currentToDate = range.to;
                dtpFrom.Value = currentFromDate;
                dtpTo.Value = currentToDate;
            }
        }

        private bool IsWithinCurrentMonth(ContractDTO contract)
        {
            if (contract == null) return false;
            var referenceDate = contract.PlanDueDate ?? contract.SignDate;
            if (referenceDate == default) return false;
            var now = DateTime.Now;
            return referenceDate.Year == now.Year && referenceDate.Month == now.Month;
        }

        private bool IsCompletedStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return false;
            var normalized = status.Trim().ToLowerInvariant();
            return normalized.Contains("done") ||
                   normalized.Contains("success") ||
                   normalized.Contains("thành công") ||
                   normalized.Contains("completed") ||
                   normalized.Contains("hoàn tất") ||
                   normalized.Contains("đã hoàn thành");
        }

        private bool IsOverdueContract(ContractDTO contract, DateTime today)
        {
            if (contract == null || IsCompletedStatus(contract.Status)) return false;
            return contract.PlanDueDate.HasValue && contract.PlanDueDate.Value.Date < today.Date;
        }

        private void UpdateChartsByMonth(List<ContractDTO> contracts, DateTime fromDate, DateTime toDate)
        {
            if (contracts == null)
            {
                contracts = new List<ContractDTO>();
            }
            var validContracts = contracts.Where(c => c.OrderDate != DateTime.MinValue).ToList();
            DateTime today = DateTime.Today;
            if (toDate < fromDate)
            {
                var temp = fromDate;
                fromDate = toDate;
                toDate = temp;
            }

            int totalDays = (toDate - fromDate).Days + 1;
            if (totalDays <= 0)
            {
                totalDays = 1;
            }

            // Update revenue chart (line chart)
            chartRevenue.Series.Clear();
            var revenueSeries = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.FromArgb(28, 61, 128),
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                MarkerColor = Color.FromArgb(28, 61, 128)
            };

            var successSeries = new Series("Thành công")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(138, 185, 91)
            };
            var inProgressSeries = new Series("Đang thực hiện")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(255, 244, 118)
            };
            var overdueSeries = new Series("Quá hạn")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(229, 92, 86)
            };

            for (int i = 0; i < totalDays; i++)
            {
                DateTime dayDate = fromDate.AddDays(i);
                var dayContracts = validContracts.Where(c => c.OrderDate.Date == dayDate).ToList();

                decimal revenue = dayContracts.Sum(c => c.TotalAmount);
                int successCount = dayContracts.Count(c => StatusContains(c.Status, "done", "success", "thành công", "completed", "hoàn tất", "đã hoàn thành"));
                int inProgressCount = dayContracts.Count(c => StatusContains(c.Status, "progress", "đang thực hiện", "processing", "inprogress", "đang xử lý"));
                int overdueCount = dayContracts.Count(c =>
                    !StatusContains(c.Status, "done", "success", "thành công", "completed", "hoàn tất", "đã hoàn thành") &&
                    c.PlanDueDate.HasValue &&
                    c.PlanDueDate.Value.Date < today);

                string label = dayDate.ToString("dd/MM");
                revenueSeries.Points.AddXY(label, revenue);
                successSeries.Points.AddXY(label, successCount);
                inProgressSeries.Points.AddXY(label, inProgressCount);
                overdueSeries.Points.AddXY(label, overdueCount);
            }

            chartRevenue.Series.Add(revenueSeries);
            chartRevenue.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartRevenue.ChartAreas[0].AxisX.Interval = 1;
            chartRevenue.ChartAreas[0].AxisY.LabelStyle.Format = "{0:N0} VNĐ";
            chartRevenue.ChartAreas[0].AxisX.Title = "Ngày trong tháng";
            chartRevenue.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";

            // Update orders chart (column chart)
            chartOrders.Series.Clear();
            chartOrders.Series.Add(successSeries);
            chartOrders.Series.Add(inProgressSeries);
            chartOrders.Series.Add(overdueSeries);
            chartOrders.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartOrders.ChartAreas[0].AxisX.Interval = 1;
            chartOrders.ChartAreas[0].AxisY.Interval = 1;
            chartOrders.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
            chartOrders.ChartAreas[0].AxisX.Title = "Ngày trong tháng";
            chartOrders.ChartAreas[0].AxisY.Title = "Số lượng hợp đồng";
        }

        private void UpdateChartsByQuarter(List<ContractDTO> contracts, DateTime fromDate, DateTime toDate)
        {
            if (contracts == null)
            {
                contracts = new List<ContractDTO>();
            }
            var validContracts = contracts.Where(c => c.OrderDate != DateTime.MinValue).ToList();
            DateTime today = DateTime.Today;
            if (toDate < fromDate)
            {
                var temp = fromDate;
                fromDate = toDate;
                toDate = temp;
            }

            DateTime referenceMonthStart = new DateTime(toDate.Year, toDate.Month, 1);
            DateTime firstMonthStart = referenceMonthStart.AddMonths(-2);
            var monthLabels = new List<DateTime>
            {
                firstMonthStart,
                firstMonthStart.AddMonths(1),
                firstMonthStart.AddMonths(2)
            };

            var monthlyAggregates = new List<(string Label, decimal Revenue, int Success, int InProgress, int Overdue)>();

            foreach (var monthStart in monthLabels)
            {
                DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
                var monthContracts = validContracts
                    .Where(c => c.OrderDate.Date >= monthStart && c.OrderDate.Date <= monthEnd)
                    .ToList();

                decimal revenue = monthContracts.Sum(c => c.TotalAmount);
                int successCount = monthContracts.Count(c => StatusContains(c.Status, "done", "success", "thành công", "completed", "hoàn tất", "đã hoàn thành"));
                int inProgressCount = monthContracts.Count(c => StatusContains(c.Status, "progress", "đang thực hiện", "processing", "inprogress", "đang xử lý"));
                int overdueCount = monthContracts.Count(c =>
                    !StatusContains(c.Status, "done", "success", "thành công", "completed", "hoàn tất", "đã hoàn thành") &&
                    c.PlanDueDate.HasValue &&
                    c.PlanDueDate.Value.Date < today);

                monthlyAggregates.Add((monthStart.ToString("MM/yyyy"), revenue, successCount, inProgressCount, overdueCount));
            }

            // Update revenue chart (line chart)
            chartRevenue.Series.Clear();
            var revenueSeries = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.FromArgb(28, 61, 128),
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                MarkerColor = Color.FromArgb(28, 61, 128)
            };

            foreach (var data in monthlyAggregates)
            {
                revenueSeries.Points.AddXY(data.Label, data.Revenue);
            }

            chartRevenue.Series.Add(revenueSeries);
            chartRevenue.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartRevenue.ChartAreas[0].AxisX.Interval = 1;
            chartRevenue.ChartAreas[0].AxisY.LabelStyle.Format = "{0:N0} VNĐ";
            chartRevenue.ChartAreas[0].AxisX.Title = "3 tháng gần nhất";
            chartRevenue.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";

            // Update orders chart (column chart)
            chartOrders.Series.Clear();
            var successSeries = new Series("Thành công")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(138, 185, 91)
            };
            var inProgressSeries = new Series("Đang thực hiện")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(255, 244, 118)
            };
            var overdueSeries = new Series("Quá hạn")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(229, 92, 86)
            };

            foreach (var data in monthlyAggregates)
            {
                successSeries.Points.AddXY(data.Label, data.Success);
                inProgressSeries.Points.AddXY(data.Label, data.InProgress);
                overdueSeries.Points.AddXY(data.Label, data.Overdue);
            }

            chartOrders.Series.Add(successSeries);
            chartOrders.Series.Add(inProgressSeries);
            chartOrders.Series.Add(overdueSeries);
            chartOrders.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartOrders.ChartAreas[0].AxisX.Interval = 1;
            chartOrders.ChartAreas[0].AxisY.Interval = 1;
            chartOrders.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
            chartOrders.ChartAreas[0].AxisX.Title = "3 tháng gần nhất";
            chartOrders.ChartAreas[0].AxisY.Title = "Số lượng hợp đồng";
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fromDate = dtpFrom.Value.Date;
                DateTime toDate = dtpTo.Value.Date;

                if (fromDate > toDate)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                currentFromDate = fromDate;
                currentToDate = toDate;
                _isCustomFilterApplied = true;

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Reset date range to current month
            var defaultRange = GetDefaultRange();
            currentFromDate = defaultRange.from;
            currentToDate = defaultRange.to;
            dtpFrom.Value = currentFromDate;
            dtpTo.Value = currentToDate;
            _isCustomFilterApplied = false;
            cmbGroupBy.SelectedIndex = 0;
            
            // Reload all data
            LoadData();
        }

        private void cmbGroupBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnsureCurrentRange();
            var filteredContracts = FilterContractsByRange(allContracts, currentFromDate, currentToDate);
            UpdateCharts(filteredContracts, currentFromDate, currentToDate);
        }

        private void lblSuccessIcon_Click(object sender, EventArgs e)
        {

        }

        private void pnlCards_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
