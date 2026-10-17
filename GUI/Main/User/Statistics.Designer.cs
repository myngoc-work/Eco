namespace GUI.Main.User
{
    partial class Statistics
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pnlFilterBar = new Guna.UI2.WinForms.Guna2Panel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.btnStatistics = new Guna.UI2.WinForms.Guna2Button();
            this.btnClear = new Guna.UI2.WinForms.Guna2Button();
            this.lblGroupBy = new System.Windows.Forms.Label();
            this.cmbGroupBy = new Guna.UI2.WinForms.Guna2ComboBox();
            this.pnlCards = new System.Windows.Forms.Panel();
            this.cardSuccess = new Guna.UI2.WinForms.Guna2Panel();
            this.lblSuccessIcon = new System.Windows.Forms.Label();
            this.lblSuccessTitle = new System.Windows.Forms.Label();
            this.lblSuccessCount = new System.Windows.Forms.Label();
            this.cardInProgress = new Guna.UI2.WinForms.Guna2Panel();
            this.lblInProgressIcon = new System.Windows.Forms.Label();
            this.lblInProgressTitle = new System.Windows.Forms.Label();
            this.lblInProgressCount = new System.Windows.Forms.Label();
            this.cardOverdue = new Guna.UI2.WinForms.Guna2Panel();
            this.lblOverdueIcon = new System.Windows.Forms.Label();
            this.lblOverdueTitle = new System.Windows.Forms.Label();
            this.lblOverdueCount = new System.Windows.Forms.Label();
            this.cardReturning = new Guna.UI2.WinForms.Guna2Panel();
            this.lblReturningIcon = new System.Windows.Forms.Label();
            this.lblReturningTitle = new System.Windows.Forms.Label();
            this.lblReturningCount = new System.Windows.Forms.Label();
            this.chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartOrders = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlFilterBar.SuspendLayout();
            this.pnlCards.SuspendLayout();
            this.cardSuccess.SuspendLayout();
            this.cardInProgress.SuspendLayout();
            this.cardOverdue.SuspendLayout();
            this.cardReturning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.BackColor = System.Drawing.Color.White;
            this.pnlFilterBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlFilterBar.BorderRadius = 30;
            this.pnlFilterBar.Controls.Add(this.lblFrom);
            this.pnlFilterBar.Controls.Add(this.dtpFrom);
            this.pnlFilterBar.Controls.Add(this.lblTo);
            this.pnlFilterBar.Controls.Add(this.dtpTo);
            this.pnlFilterBar.Controls.Add(this.btnStatistics);
            this.pnlFilterBar.Controls.Add(this.btnClear);
            this.pnlFilterBar.Controls.Add(this.lblGroupBy);
            this.pnlFilterBar.Controls.Add(this.cmbGroupBy);
            this.pnlFilterBar.Location = new System.Drawing.Point(65, 22);
            this.pnlFilterBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Size = new System.Drawing.Size(1575, 65);
            this.pnlFilterBar.TabIndex = 0;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblFrom.Location = new System.Drawing.Point(42, 23);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(41, 25);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Từ:";
            // 
            // dtpFrom
            // 
            this.dtpFrom.BackColor = System.Drawing.Color.White;
            this.dtpFrom.BorderRadius = 10;
            this.dtpFrom.Checked = true;
            this.dtpFrom.FillColor = System.Drawing.Color.White;
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 10.875F);
            this.dtpFrom.ForeColor = System.Drawing.Color.Black;
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(146, 17);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpFrom.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpFrom.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(183, 32);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.Value = new System.DateTime(2025, 11, 20, 20, 41, 5, 452);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTo.Location = new System.Drawing.Point(419, 24);
            this.lblTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(53, 25);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "Đến:";
            // 
            // dtpTo
            // 
            this.dtpTo.BackColor = System.Drawing.Color.White;
            this.dtpTo.BorderRadius = 10;
            this.dtpTo.Checked = true;
            this.dtpTo.FillColor = System.Drawing.Color.White;
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 10.875F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(540, 17);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpTo.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpTo.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(183, 32);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.Value = new System.DateTime(2025, 11, 20, 20, 41, 5, 495);
            // 
            // btnStatistics
            // 
            this.btnStatistics.BorderRadius = 20;
            this.btnStatistics.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(61)))), ((int)(((byte)(128)))));
            this.btnStatistics.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStatistics.ForeColor = System.Drawing.Color.White;
            this.btnStatistics.Location = new System.Drawing.Point(812, 15);
            this.btnStatistics.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(152, 43);
            this.btnStatistics.TabIndex = 4;
            this.btnStatistics.Text = "Thống kê";
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnClear
            // 
            this.btnClear.BorderRadius = 20;
            this.btnClear.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(61)))), ((int)(((byte)(128)))));
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(1060, 12);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(93, 46);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Xóa";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblGroupBy
            // 
            this.lblGroupBy.AutoSize = true;
            this.lblGroupBy.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblGroupBy.Location = new System.Drawing.Point(1235, 17);
            this.lblGroupBy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGroupBy.Name = "lblGroupBy";
            this.lblGroupBy.Size = new System.Drawing.Size(117, 25);
            this.lblGroupBy.TabIndex = 6;
            this.lblGroupBy.Text = "Nhóm theo:";
            // 
            // cmbGroupBy
            // 
            this.cmbGroupBy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbGroupBy.BackColor = System.Drawing.Color.Transparent;
            this.cmbGroupBy.BorderRadius = 20;
            this.cmbGroupBy.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupBy.FocusedColor = System.Drawing.Color.Empty;
            this.cmbGroupBy.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbGroupBy.ForeColor = System.Drawing.Color.Black;
            this.cmbGroupBy.ItemHeight = 30;
            this.cmbGroupBy.Items.AddRange(new object[] {
            "Tháng",
            "Quý"});
            this.cmbGroupBy.Location = new System.Drawing.Point(1391, 12);
            this.cmbGroupBy.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbGroupBy.Name = "cmbGroupBy";
            this.cmbGroupBy.Size = new System.Drawing.Size(184, 36);
            this.cmbGroupBy.StartIndex = 0;
            this.cmbGroupBy.TabIndex = 7;
            this.cmbGroupBy.SelectedIndexChanged += new System.EventHandler(this.cmbGroupBy_SelectedIndexChanged);
            // 
            // pnlCards
            // 
            this.pnlCards.Controls.Add(this.cardSuccess);
            this.pnlCards.Location = new System.Drawing.Point(65, 104);
            this.pnlCards.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Size = new System.Drawing.Size(1575, 145);
            this.pnlCards.TabIndex = 1;
            this.pnlCards.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCards_Paint);
            // 
            // cardSuccess
            // 
            this.cardSuccess.BorderRadius = 40;
            this.cardSuccess.Controls.Add(this.lblSuccessIcon);
            this.cardSuccess.Controls.Add(this.lblSuccessTitle);
            this.cardSuccess.Controls.Add(this.lblSuccessCount);
            this.cardSuccess.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(185)))), ((int)(((byte)(91)))));
            this.cardSuccess.Location = new System.Drawing.Point(18, 2);
            this.cardSuccess.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cardSuccess.Name = "cardSuccess";
            this.cardSuccess.Size = new System.Drawing.Size(311, 145);
            this.cardSuccess.TabIndex = 0;
            // 
            // lblSuccessIcon
            // 
            this.lblSuccessIcon.AutoSize = true;
            this.lblSuccessIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblSuccessIcon.Font = new System.Drawing.Font("Segoe UI", 28F);
            this.lblSuccessIcon.ForeColor = System.Drawing.Color.White;
            this.lblSuccessIcon.Location = new System.Drawing.Point(206, 19);
            this.lblSuccessIcon.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSuccessIcon.Name = "lblSuccessIcon";
            this.lblSuccessIcon.Size = new System.Drawing.Size(86, 62);
            this.lblSuccessIcon.TabIndex = 0;
            this.lblSuccessIcon.Text = "✔";
            this.lblSuccessIcon.Click += new System.EventHandler(this.lblSuccessIcon_Click);
            // 
            // lblSuccessTitle
            // 
            this.lblSuccessTitle.AutoSize = true;
            this.lblSuccessTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSuccessTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSuccessTitle.ForeColor = System.Drawing.Color.White;
            this.lblSuccessTitle.Location = new System.Drawing.Point(20, 77);
            this.lblSuccessTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSuccessTitle.Name = "lblSuccessTitle";
            this.lblSuccessTitle.Size = new System.Drawing.Size(220, 28);
            this.lblSuccessTitle.TabIndex = 1;
            this.lblSuccessTitle.Text = "Hợp đồng thành công";
            // 
            // lblSuccessCount
            // 
            this.lblSuccessCount.AutoSize = true;
            this.lblSuccessCount.BackColor = System.Drawing.Color.Transparent;
            this.lblSuccessCount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblSuccessCount.ForeColor = System.Drawing.Color.White;
            this.lblSuccessCount.Location = new System.Drawing.Point(20, 26);
            this.lblSuccessCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSuccessCount.Name = "lblSuccessCount";
            this.lblSuccessCount.Size = new System.Drawing.Size(46, 54);
            this.lblSuccessCount.TabIndex = 2;
            this.lblSuccessCount.Text = "0";
            // 
            // cardInProgress
            // 
            this.cardInProgress.BorderRadius = 40;
            this.cardInProgress.Controls.Add(this.lblInProgressIcon);
            this.cardInProgress.Controls.Add(this.lblInProgressTitle);
            this.cardInProgress.Controls.Add(this.lblInProgressCount);
            this.cardInProgress.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(118)))));
            this.cardInProgress.Location = new System.Drawing.Point(489, 104);
            this.cardInProgress.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cardInProgress.Name = "cardInProgress";
            this.cardInProgress.Size = new System.Drawing.Size(325, 145);
            this.cardInProgress.TabIndex = 1;
            // 
            // lblInProgressIcon
            // 
            this.lblInProgressIcon.AutoSize = true;
            this.lblInProgressIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblInProgressIcon.Font = new System.Drawing.Font("Segoe UI", 28F);
            this.lblInProgressIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblInProgressIcon.Location = new System.Drawing.Point(213, 19);
            this.lblInProgressIcon.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInProgressIcon.Name = "lblInProgressIcon";
            this.lblInProgressIcon.Size = new System.Drawing.Size(86, 62);
            this.lblInProgressIcon.TabIndex = 0;
            this.lblInProgressIcon.Text = "⏱";
            // 
            // lblInProgressTitle
            // 
            this.lblInProgressTitle.AutoSize = true;
            this.lblInProgressTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblInProgressTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblInProgressTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblInProgressTitle.Location = new System.Drawing.Point(20, 77);
            this.lblInProgressTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInProgressTitle.Name = "lblInProgressTitle";
            this.lblInProgressTitle.Size = new System.Drawing.Size(257, 28);
            this.lblInProgressTitle.TabIndex = 1;
            this.lblInProgressTitle.Text = "Hợp đồng đang thực hiện";
            // 
            // lblInProgressCount
            // 
            this.lblInProgressCount.AutoSize = true;
            this.lblInProgressCount.BackColor = System.Drawing.Color.Transparent;
            this.lblInProgressCount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblInProgressCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblInProgressCount.Location = new System.Drawing.Point(20, 26);
            this.lblInProgressCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInProgressCount.Name = "lblInProgressCount";
            this.lblInProgressCount.Size = new System.Drawing.Size(46, 54);
            this.lblInProgressCount.TabIndex = 2;
            this.lblInProgressCount.Text = "0";
            // 
            // cardOverdue
            // 
            this.cardOverdue.BorderRadius = 40;
            this.cardOverdue.Controls.Add(this.lblOverdueIcon);
            this.cardOverdue.Controls.Add(this.lblOverdueTitle);
            this.cardOverdue.Controls.Add(this.lblOverdueCount);
            this.cardOverdue.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(92)))), ((int)(((byte)(86)))));
            this.cardOverdue.Location = new System.Drawing.Point(919, 106);
            this.cardOverdue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cardOverdue.Name = "cardOverdue";
            this.cardOverdue.Size = new System.Drawing.Size(324, 143);
            this.cardOverdue.TabIndex = 2;
            // 
            // lblOverdueIcon
            // 
            this.lblOverdueIcon.AutoSize = true;
            this.lblOverdueIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblOverdueIcon.Font = new System.Drawing.Font("Segoe UI", 28F);
            this.lblOverdueIcon.ForeColor = System.Drawing.Color.White;
            this.lblOverdueIcon.Location = new System.Drawing.Point(213, 19);
            this.lblOverdueIcon.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOverdueIcon.Name = "lblOverdueIcon";
            this.lblOverdueIcon.Size = new System.Drawing.Size(86, 62);
            this.lblOverdueIcon.TabIndex = 0;
            this.lblOverdueIcon.Text = "⚠";
            // 
            // lblOverdueTitle
            // 
            this.lblOverdueTitle.AutoSize = true;
            this.lblOverdueTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblOverdueTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblOverdueTitle.ForeColor = System.Drawing.Color.White;
            this.lblOverdueTitle.Location = new System.Drawing.Point(20, 77);
            this.lblOverdueTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOverdueTitle.Name = "lblOverdueTitle";
            this.lblOverdueTitle.Size = new System.Drawing.Size(189, 28);
            this.lblOverdueTitle.TabIndex = 1;
            this.lblOverdueTitle.Text = "Hợp đồng quá hạn";
            // 
            // lblOverdueCount
            // 
            this.lblOverdueCount.AutoSize = true;
            this.lblOverdueCount.BackColor = System.Drawing.Color.Transparent;
            this.lblOverdueCount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblOverdueCount.ForeColor = System.Drawing.Color.White;
            this.lblOverdueCount.Location = new System.Drawing.Point(20, 26);
            this.lblOverdueCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOverdueCount.Name = "lblOverdueCount";
            this.lblOverdueCount.Size = new System.Drawing.Size(46, 54);
            this.lblOverdueCount.TabIndex = 2;
            this.lblOverdueCount.Text = "0";
            // 
            // cardReturning
            // 
            this.cardReturning.BorderRadius = 40;
            this.cardReturning.Controls.Add(this.lblReturningIcon);
            this.cardReturning.Controls.Add(this.lblReturningTitle);
            this.cardReturning.Controls.Add(this.lblReturningCount);
            this.cardReturning.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(133)))), ((int)(((byte)(63)))));
            this.cardReturning.Location = new System.Drawing.Point(1321, 104);
            this.cardReturning.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cardReturning.Name = "cardReturning";
            this.cardReturning.Size = new System.Drawing.Size(319, 143);
            this.cardReturning.TabIndex = 3;
            // 
            // lblReturningIcon
            // 
            this.lblReturningIcon.AutoSize = true;
            this.lblReturningIcon.BackColor = System.Drawing.Color.Transparent;
            this.lblReturningIcon.Font = new System.Drawing.Font("Segoe UI", 28F);
            this.lblReturningIcon.ForeColor = System.Drawing.Color.White;
            this.lblReturningIcon.Location = new System.Drawing.Point(238, 18);
            this.lblReturningIcon.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReturningIcon.Name = "lblReturningIcon";
            this.lblReturningIcon.Size = new System.Drawing.Size(67, 62);
            this.lblReturningIcon.TabIndex = 0;
            this.lblReturningIcon.Text = "⭐";
            // 
            // lblReturningTitle
            // 
            this.lblReturningTitle.AutoSize = true;
            this.lblReturningTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblReturningTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblReturningTitle.ForeColor = System.Drawing.Color.White;
            this.lblReturningTitle.Location = new System.Drawing.Point(20, 77);
            this.lblReturningTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReturningTitle.Name = "lblReturningTitle";
            this.lblReturningTitle.Size = new System.Drawing.Size(204, 28);
            this.lblReturningTitle.TabIndex = 1;
            this.lblReturningTitle.Text = "Khách hàng quay lại";
            // 
            // lblReturningCount
            // 
            this.lblReturningCount.AutoSize = true;
            this.lblReturningCount.BackColor = System.Drawing.Color.Transparent;
            this.lblReturningCount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblReturningCount.ForeColor = System.Drawing.Color.White;
            this.lblReturningCount.Location = new System.Drawing.Point(20, 26);
            this.lblReturningCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReturningCount.Name = "lblReturningCount";
            this.lblReturningCount.Size = new System.Drawing.Size(46, 54);
            this.lblReturningCount.TabIndex = 2;
            this.lblReturningCount.Text = "0";
            // 
            // chartRevenue
            // 
            this.chartRevenue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(233)))), ((int)(((byte)(168)))));
            this.chartRevenue.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(233)))), ((int)(((byte)(168)))));
            chartArea1.Name = "ChartArea1";
            this.chartRevenue.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRevenue.Legends.Add(legend1);
            this.chartRevenue.Location = new System.Drawing.Point(65, 269);
            this.chartRevenue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chartRevenue.Name = "chartRevenue";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Doanh thu";
            this.chartRevenue.Series.Add(series1);
            this.chartRevenue.Size = new System.Drawing.Size(1575, 309);
            this.chartRevenue.TabIndex = 2;
            this.chartRevenue.Text = "";
            this.chartRevenue.Titles.Add(new System.Windows.Forms.DataVisualization.Charting.Title
            {
                Text = "Doanh số theo ngày (VND)",
                Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(28, 99, 63)
            });
            // 
            // chartOrders
            // 
            this.chartOrders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(233)))), ((int)(((byte)(168)))));
            this.chartOrders.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(233)))), ((int)(((byte)(168)))));
            chartArea2.Name = "ChartArea1";
            this.chartOrders.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartOrders.Legends.Add(legend2);
            this.chartOrders.Location = new System.Drawing.Point(65, 595);
            this.chartOrders.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chartOrders.Name = "chartOrders";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Hợp đồng";
            this.chartOrders.Series.Add(series2);
            this.chartOrders.Size = new System.Drawing.Size(1575, 374);
            this.chartOrders.TabIndex = 3;
            this.chartOrders.Text = "";
            this.chartOrders.Titles.Add(new System.Windows.Forms.DataVisualization.Charting.Title
            {
                Text = "Biểu đồ đơn hàng",
                Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(28, 99, 63)
            });
            // 
            // Statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chartOrders);
            this.Controls.Add(this.cardOverdue);
            this.Controls.Add(this.cardReturning);
            this.Controls.Add(this.cardInProgress);
            this.Controls.Add(this.chartRevenue);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.pnlFilterBar);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Statistics";
            this.Size = new System.Drawing.Size(1901, 998);
            this.Load += new System.EventHandler(this.Statistics_Load);
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            this.pnlCards.ResumeLayout(false);
            this.cardSuccess.ResumeLayout(false);
            this.cardSuccess.PerformLayout();
            this.cardInProgress.ResumeLayout(false);
            this.cardInProgress.PerformLayout();
            this.cardOverdue.ResumeLayout(false);
            this.cardOverdue.PerformLayout();
            this.cardReturning.ResumeLayout(false);
            this.cardReturning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartOrders)).EndInit();
            this.ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel pnlFilterBar;
        private System.Windows.Forms.Label lblFrom;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpTo;
        private Guna.UI2.WinForms.Guna2Button btnStatistics;
        private Guna.UI2.WinForms.Guna2Button btnClear;
        private System.Windows.Forms.Label lblGroupBy;
        private Guna.UI2.WinForms.Guna2ComboBox cmbGroupBy;
        
        private System.Windows.Forms.Panel pnlCards;
        private Guna.UI2.WinForms.Guna2Panel cardSuccess;
        private System.Windows.Forms.Label lblSuccessIcon;
        private System.Windows.Forms.Label lblSuccessTitle;
        private System.Windows.Forms.Label lblSuccessCount;
        private Guna.UI2.WinForms.Guna2Panel cardInProgress;
        private System.Windows.Forms.Label lblInProgressIcon;
        private System.Windows.Forms.Label lblInProgressTitle;
        private System.Windows.Forms.Label lblInProgressCount;
        private Guna.UI2.WinForms.Guna2Panel cardOverdue;
        private System.Windows.Forms.Label lblOverdueIcon;
        private System.Windows.Forms.Label lblOverdueTitle;
        private System.Windows.Forms.Label lblOverdueCount;
        private Guna.UI2.WinForms.Guna2Panel cardReturning;
        private System.Windows.Forms.Label lblReturningIcon;
        private System.Windows.Forms.Label lblReturningTitle;
        private System.Windows.Forms.Label lblReturningCount;
        
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartOrders;
    }
}
