using Guna.UI2.WinForms;

namespace GUI.Admin
{
    partial class AdminOrderManagementContentUI
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.panelLeftContainer = new System.Windows.Forms.Panel();
            this.flowLayoutPanelOrders = new System.Windows.Forms.FlowLayoutPanel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.picSearchIcon = new System.Windows.Forms.PictureBox();
            this.picMicrophoneIcon = new System.Windows.Forms.PictureBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanelDetails = new System.Windows.Forms.TableLayoutPanel();
            this.lblOrderId = new System.Windows.Forms.Label();
            this.txtOrderId = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblCustomerId = new System.Windows.Forms.Label();
            this.txtCustomerId = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblSampleStartDate = new System.Windows.Forms.Label();
            this.dtpSampleStartDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblExpectedReturnDate = new System.Windows.Forms.Label();
            this.dtpExpectedReturnDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblPersonInCharge = new System.Windows.Forms.Label();
            this.txtPersonInCharge = new Guna.UI2.WinForms.Guna2TextBox();
            this.panelDetailHeader = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.PictureBox();
            this.btnAdd = new System.Windows.Forms.PictureBox();
            this.btnHide = new System.Windows.Forms.PictureBox();
            this.lblDetailTitle = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.panelLeftContainer.SuspendLayout();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSearchIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMicrophoneIcon)).BeginInit();
            this.panelDetails.SuspendLayout();
            this.tableLayoutPanelDetails.SuspendLayout();
            this.panelDetailHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHide)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.panelLeftContainer);
            this.splitContainerMain.Panel1MinSize = 320;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.panelDetails);
            this.splitContainerMain.Size = new System.Drawing.Size(2384, 2306);
            this.splitContainerMain.SplitterDistance = 455;
            this.splitContainerMain.SplitterWidth = 8;
            this.splitContainerMain.TabIndex = 1;
            // 
            // panelLeftContainer
            // 
            this.panelLeftContainer.BackColor = System.Drawing.Color.White;
            this.panelLeftContainer.Controls.Add(this.flowLayoutPanelOrders);
            this.panelLeftContainer.Controls.Add(this.panelSearch);
            this.panelLeftContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftContainer.Location = new System.Drawing.Point(0, 0);
            this.panelLeftContainer.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelLeftContainer.Name = "panelLeftContainer";
            this.panelLeftContainer.Padding = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.panelLeftContainer.Size = new System.Drawing.Size(455, 2306);
            this.panelLeftContainer.TabIndex = 0;
            // 
            // flowLayoutPanelOrders
            // 
            this.flowLayoutPanelOrders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelOrders.AutoScroll = true;
            this.flowLayoutPanelOrders.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelOrders.Location = new System.Drawing.Point(18, 95);
            this.flowLayoutPanelOrders.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.flowLayoutPanelOrders.Name = "flowLayoutPanelOrders";
            this.flowLayoutPanelOrders.Size = new System.Drawing.Size(419, 844);
            this.flowLayoutPanelOrders.TabIndex = 1;
            this.flowLayoutPanelOrders.WrapContents = false;
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.picSearchIcon);
            this.panelSearch.Controls.Add(this.picMicrophoneIcon);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(18, 16);
            this.panelSearch.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(419, 77);
            this.panelSearch.TabIndex = 0;
            // 
            // picSearchIcon
            // 
            this.picSearchIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picSearchIcon.Image = global::GUI.Properties.Resources.search_icon;
            this.picSearchIcon.Location = new System.Drawing.Point(256, 19);
            this.picSearchIcon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picSearchIcon.Name = "picSearchIcon";
            this.picSearchIcon.Size = new System.Drawing.Size(30, 31);
            this.picSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSearchIcon.TabIndex = 6;
            this.picSearchIcon.TabStop = false;
            // 
            // picMicrophoneIcon
            // 
            this.picMicrophoneIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picMicrophoneIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMicrophoneIcon.Image = global::GUI.Properties.Resources.mic_icon;
            this.picMicrophoneIcon.Location = new System.Drawing.Point(299, 19);
            this.picMicrophoneIcon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picMicrophoneIcon.Name = "picMicrophoneIcon";
            this.picMicrophoneIcon.Size = new System.Drawing.Size(30, 31);
            this.picMicrophoneIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMicrophoneIcon.TabIndex = 5;
            this.picMicrophoneIcon.TabStop = false;
            this.toolTip.SetToolTip(this.picMicrophoneIcon, "Tìm kiếm bằng giọng nói");
            this.picMicrophoneIcon.Click += new System.EventHandler(this.picMicrophoneIcon_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Location = new System.Drawing.Point(23, 19);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(226, 50);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // panelDetails
            // 
            this.panelDetails.AutoScroll = true;
            this.panelDetails.BackColor = System.Drawing.Color.White;
            this.panelDetails.Controls.Add(this.btnCancel);
            this.panelDetails.Controls.Add(this.btnSave);
            this.panelDetails.Controls.Add(this.tableLayoutPanelDetails);
            this.panelDetails.Controls.Add(this.panelDetailHeader);
            this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetails.Location = new System.Drawing.Point(0, 0);
            this.panelDetails.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Padding = new System.Windows.Forms.Padding(40, 34, 40, 34);
            this.panelDetails.Size = new System.Drawing.Size(1921, 2306);
            this.panelDetails.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.BorderRadius = 20;
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.LightGray;
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.btnCancel.FillColor = System.Drawing.Color.LightGray;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 13.125F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(71)))), ((int)(((byte)(80)))));
            this.btnCancel.Location = new System.Drawing.Point(1143, 682);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(230, 70);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.BorderRadius = 20;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.LightGray;
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(764, 682);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(286, 70);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Lưu thay đổi";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanelDetails
            // 
            this.tableLayoutPanelDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelDetails.ColumnCount = 4;
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162F));
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDetails.Controls.Add(this.lblOrderId, 0, 0);
            this.tableLayoutPanelDetails.Controls.Add(this.txtOrderId, 1, 0);
            this.tableLayoutPanelDetails.Controls.Add(this.lblSampleStartDate, 0, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.dtpSampleStartDate, 1, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.lblStatus, 0, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.cmbStatus, 1, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.txtCustomerId, 1, 3);
            this.tableLayoutPanelDetails.Controls.Add(this.dtpExpectedReturnDate, 1, 4);
            this.tableLayoutPanelDetails.Controls.Add(this.lblCustomerId, 0, 3);
            this.tableLayoutPanelDetails.Controls.Add(this.lblExpectedReturnDate, 0, 4);
            this.tableLayoutPanelDetails.Controls.Add(this.lblPersonInCharge, 0, 5);
            this.tableLayoutPanelDetails.Controls.Add(this.txtPersonInCharge, 1, 5);
            this.tableLayoutPanelDetails.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.tableLayoutPanelDetails.Location = new System.Drawing.Point(40, 141);
            this.tableLayoutPanelDetails.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tableLayoutPanelDetails.Name = "tableLayoutPanelDetails";
            this.tableLayoutPanelDetails.RowCount = 6;
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanelDetails.Size = new System.Drawing.Size(1874, 502);
            this.tableLayoutPanelDetails.TabIndex = 1;
            // 
            // lblOrderId
            // 
            this.lblOrderId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOrderId.AutoSize = true;
            this.lblOrderId.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblOrderId.Location = new System.Drawing.Point(6, 1);
            this.lblOrderId.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(142, 90);
            this.lblOrderId.TabIndex = 0;
            this.lblOrderId.Text = "Mã đơn hàng";
            this.lblOrderId.Click += new System.EventHandler(this.lblOrderId_Click);
            // 
            // txtOrderId
            // 
            this.txtOrderId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOrderId.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtOrderId.BorderRadius = 24;
            this.tableLayoutPanelDetails.SetColumnSpan(this.txtOrderId, 3);
            this.txtOrderId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOrderId.DefaultText = "";
            this.txtOrderId.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtOrderId.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtOrderId.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtOrderId.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtOrderId.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtOrderId.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtOrderId.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtOrderId.Location = new System.Drawing.Point(160, 8);
            this.txtOrderId.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtOrderId.Name = "txtOrderId";
            this.txtOrderId.PlaceholderText = "";
            this.txtOrderId.SelectedText = "";
            this.txtOrderId.Size = new System.Drawing.Size(1708, 77);
            this.txtOrderId.TabIndex = 1;
            // 
            // lblCustomerId
            // 
            this.lblCustomerId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCustomerId.AutoSize = true;
            this.lblCustomerId.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCustomerId.Location = new System.Drawing.Point(6, 261);
            this.lblCustomerId.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCustomerId.Name = "lblCustomerId";
            this.lblCustomerId.Size = new System.Drawing.Size(112, 103);
            this.lblCustomerId.TabIndex = 2;
            this.lblCustomerId.Text = "Mã khách hàng";
            // 
            // txtCustomerId
            // 
            this.txtCustomerId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomerId.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtCustomerId.BorderRadius = 24;
            this.tableLayoutPanelDetails.SetColumnSpan(this.txtCustomerId, 3);
            this.txtCustomerId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerId.DefaultText = "";
            this.txtCustomerId.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtCustomerId.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtCustomerId.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtCustomerId.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtCustomerId.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtCustomerId.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtCustomerId.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtCustomerId.Location = new System.Drawing.Point(160, 279);
            this.txtCustomerId.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtCustomerId.Name = "txtCustomerId";
            this.txtCustomerId.PlaceholderText = "";
            this.txtCustomerId.SelectedText = "";
            this.txtCustomerId.Size = new System.Drawing.Size(1708, 67);
            this.txtCustomerId.TabIndex = 3;
            // 
            // lblSampleStartDate
            // 
            this.lblSampleStartDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSampleStartDate.AutoSize = true;
            this.lblSampleStartDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSampleStartDate.Location = new System.Drawing.Point(6, 93);
            this.lblSampleStartDate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSampleStartDate.Name = "lblSampleStartDate";
            this.lblSampleStartDate.Size = new System.Drawing.Size(137, 84);
            this.lblSampleStartDate.TabIndex = 4;
            this.lblSampleStartDate.Text = "Ngày bắt đầu lấy mẫu";
            // 
            // dtpSampleStartDate
            // 
            this.dtpSampleStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpSampleStartDate.BorderRadius = 24;
            this.dtpSampleStartDate.Checked = true;
            this.tableLayoutPanelDetails.SetColumnSpan(this.dtpSampleStartDate, 3);
            this.dtpSampleStartDate.CustomFormat = "dd/MM/yyyy";
            this.dtpSampleStartDate.Enabled = false;
            this.dtpSampleStartDate.FillColor = System.Drawing.Color.White;
            this.dtpSampleStartDate.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.dtpSampleStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSampleStartDate.Location = new System.Drawing.Point(160, 99);
            this.dtpSampleStartDate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dtpSampleStartDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpSampleStartDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpSampleStartDate.Name = "dtpSampleStartDate";
            this.dtpSampleStartDate.Size = new System.Drawing.Size(1708, 72);
            this.dtpSampleStartDate.TabIndex = 5;
            this.dtpSampleStartDate.Value = new System.DateTime(2024, 4, 23, 0, 0, 0, 0);
            // 
            // lblExpectedReturnDate
            // 
            this.lblExpectedReturnDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblExpectedReturnDate.AutoSize = true;
            this.lblExpectedReturnDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblExpectedReturnDate.Location = new System.Drawing.Point(6, 364);
            this.lblExpectedReturnDate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblExpectedReturnDate.Name = "lblExpectedReturnDate";
            this.lblExpectedReturnDate.Size = new System.Drawing.Size(141, 55);
            this.lblExpectedReturnDate.TabIndex = 6;
            this.lblExpectedReturnDate.Text = "Ngày dự kiến trả kết quả";
            // 
            // dtpExpectedReturnDate
            // 
            this.dtpExpectedReturnDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpExpectedReturnDate.BorderRadius = 24;
            this.dtpExpectedReturnDate.Checked = true;
            this.tableLayoutPanelDetails.SetColumnSpan(this.dtpExpectedReturnDate, 3);
            this.dtpExpectedReturnDate.CustomFormat = "dd/MM/yyyy";
            this.dtpExpectedReturnDate.Enabled = false;
            this.dtpExpectedReturnDate.FillColor = System.Drawing.Color.White;
            this.dtpExpectedReturnDate.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.dtpExpectedReturnDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpectedReturnDate.Location = new System.Drawing.Point(160, 370);
            this.dtpExpectedReturnDate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dtpExpectedReturnDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpExpectedReturnDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpExpectedReturnDate.Name = "dtpExpectedReturnDate";
            this.dtpExpectedReturnDate.Size = new System.Drawing.Size(1708, 43);
            this.dtpExpectedReturnDate.TabIndex = 7;
            this.dtpExpectedReturnDate.Value = new System.DateTime(2024, 4, 23, 0, 0, 0, 0);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblStatus.Location = new System.Drawing.Point(6, 177);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(138, 84);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Trạng thái đơn hàng";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStatus.BackColor = System.Drawing.Color.Transparent;
            this.cmbStatus.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbStatus.BorderRadius = 24;
            this.tableLayoutPanelDetails.SetColumnSpan(this.cmbStatus, 3);
            this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbStatus.ForeColor = System.Drawing.Color.Black;
            this.cmbStatus.ItemHeight = 40;
            this.cmbStatus.Location = new System.Drawing.Point(160, 196);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(1708, 46);
            this.cmbStatus.TabIndex = 9;
            // 
            // lblPersonInCharge
            // 
            this.lblPersonInCharge.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPersonInCharge.AutoSize = true;
            this.lblPersonInCharge.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblPersonInCharge.Location = new System.Drawing.Point(6, 419);
            this.lblPersonInCharge.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPersonInCharge.Name = "lblPersonInCharge";
            this.lblPersonInCharge.Size = new System.Drawing.Size(118, 83);
            this.lblPersonInCharge.TabIndex = 10;
            this.lblPersonInCharge.Text = "Người phụ trách đơn hàng";
            // 
            // txtPersonInCharge
            // 
            this.txtPersonInCharge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPersonInCharge.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtPersonInCharge.BorderRadius = 24;
            this.tableLayoutPanelDetails.SetColumnSpan(this.txtPersonInCharge, 3);
            this.txtPersonInCharge.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPersonInCharge.DefaultText = "";
            this.txtPersonInCharge.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtPersonInCharge.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtPersonInCharge.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtPersonInCharge.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtPersonInCharge.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtPersonInCharge.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtPersonInCharge.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtPersonInCharge.Location = new System.Drawing.Point(160, 425);
            this.txtPersonInCharge.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtPersonInCharge.Name = "txtPersonInCharge";
            this.txtPersonInCharge.PlaceholderText = "";
            this.txtPersonInCharge.SelectedText = "";
            this.txtPersonInCharge.Size = new System.Drawing.Size(1708, 71);
            this.txtPersonInCharge.TabIndex = 11;
            // 
            // panelDetailHeader
            // 
            this.panelDetailHeader.Controls.Add(this.btnEdit);
            this.panelDetailHeader.Controls.Add(this.btnDelete);
            this.panelDetailHeader.Controls.Add(this.btnAdd);
            this.panelDetailHeader.Controls.Add(this.btnHide);
            this.panelDetailHeader.Controls.Add(this.lblDetailTitle);
            this.panelDetailHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDetailHeader.Location = new System.Drawing.Point(40, 34);
            this.panelDetailHeader.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelDetailHeader.Name = "panelDetailHeader";
            this.panelDetailHeader.Size = new System.Drawing.Size(1841, 70);
            this.panelDetailHeader.TabIndex = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Image = global::GUI.Properties.Resources.edit_icon;
            this.btnEdit.Location = new System.Drawing.Point(1205, 14);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(22, 23);
            this.btnEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnEdit.TabIndex = 11;
            this.btnEdit.TabStop = false;
            this.toolTip.SetToolTip(this.btnEdit, "Sửa thông tin đơn hàng");
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Image = global::GUI.Properties.Resources.delete_icon;
            this.btnDelete.Location = new System.Drawing.Point(1250, 14);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(22, 23);
            this.btnDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnDelete.TabIndex = 10;
            this.btnDelete.TabStop = false;
            this.toolTip.SetToolTip(this.btnDelete, "Xóa đơn hàng");
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Image = global::GUI.Properties.Resources.add_icon;
            this.btnAdd.Location = new System.Drawing.Point(1158, 14);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(22, 23);
            this.btnAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnAdd.TabIndex = 9;
            this.btnAdd.TabStop = false;
            this.toolTip.SetToolTip(this.btnAdd, "Thêm đơn hàng mới");
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnHide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHide.Image = global::GUI.Properties.Resources.eye_closed;
            this.btnHide.Location = new System.Drawing.Point(1112, 14);
            this.btnHide.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(22, 23);
            this.btnHide.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnHide.TabIndex = 12;
            this.btnHide.TabStop = false;
            this.toolTip.SetToolTip(this.btnHide, "Ẩn/Hiện thông tin phụ");
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.AutoSize = true;
            this.lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 19.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblDetailTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(99)))), ((int)(((byte)(63)))));
            this.lblDetailTitle.Location = new System.Drawing.Point(-6, 14);
            this.lblDetailTitle.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(716, 71);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "Thông tin chi tiết đơn hàng";
            // 
            // AdminOrderManagementContentUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "AdminOrderManagementContentUI";
            this.Size = new System.Drawing.Size(2384, 2306);
            this.Load += new System.EventHandler(this.AdminOrderManagementContentUI_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.panelLeftContainer.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSearchIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMicrophoneIcon)).EndInit();
            this.panelDetails.ResumeLayout(false);
            this.tableLayoutPanelDetails.ResumeLayout(false);
            this.tableLayoutPanelDetails.PerformLayout();
            this.panelDetailHeader.ResumeLayout(false);
            this.panelDetailHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHide)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelLeftContainer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelOrders;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox picMicrophoneIcon;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.PictureBox picSearchIcon;
        private System.Windows.Forms.Panel panelDetails;
        private Guna2Button btnCancel;
        private Guna2Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDetails;
        private System.Windows.Forms.Label lblOrderId;
        private Guna2TextBox txtOrderId;
        private System.Windows.Forms.Label lblCustomerId;
        private Guna2TextBox txtCustomerId;
        private System.Windows.Forms.Label lblSampleStartDate;
        private Guna2DateTimePicker dtpSampleStartDate;
        private System.Windows.Forms.Label lblExpectedReturnDate;
        private Guna2DateTimePicker dtpExpectedReturnDate;
        private System.Windows.Forms.Label lblStatus;
        private Guna2ComboBox cmbStatus;
        private System.Windows.Forms.Label lblPersonInCharge;
        private Guna2TextBox txtPersonInCharge;
        private System.Windows.Forms.Panel panelDetailHeader;
        private System.Windows.Forms.PictureBox btnEdit;
        private System.Windows.Forms.PictureBox btnDelete;
        private System.Windows.Forms.PictureBox btnAdd;
        private System.Windows.Forms.PictureBox btnHide;
        private System.Windows.Forms.Label lblDetailTitle;
    }
}