using Guna.UI2.WinForms;

namespace GUI.Admin
{
    partial class AdminEmployeeManagementContentUI
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
            this.flowLayoutPanelUsers = new System.Windows.Forms.FlowLayoutPanel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.picSearchIcon = new System.Windows.Forms.PictureBox();
            this.picMicrophoneIcon = new System.Windows.Forms.PictureBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanelDetails = new System.Windows.Forms.TableLayoutPanel();
            this.lblUserID = new System.Windows.Forms.Label();
            this.txtUserID = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblDateOfBirth = new System.Windows.Forms.Label();
            this.dtpDateOfBirth = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.lblPhoneNumber = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblHometown = new System.Windows.Forms.Label();
            this.lblNationality = new System.Windows.Forms.Label();
            this.lblCurrentAddress = new System.Windows.Forms.Label();
            this.txtCurrentAddress = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbGender = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbPosition = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbDepartment = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtHometown = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtNationality = new Guna.UI2.WinForms.Guna2TextBox();
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
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(4);
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
            this.splitContainerMain.Size = new System.Drawing.Size(1589, 1476);
            this.splitContainerMain.SplitterDistance = 320;
            this.splitContainerMain.SplitterWidth = 5;
            this.splitContainerMain.TabIndex = 1;
            this.splitContainerMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerMain_SplitterMoved);
            // 
            // panelLeftContainer
            // 
            this.panelLeftContainer.BackColor = System.Drawing.Color.White;
            this.panelLeftContainer.Controls.Add(this.flowLayoutPanelUsers);
            this.panelLeftContainer.Controls.Add(this.panelSearch);
            this.panelLeftContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftContainer.Location = new System.Drawing.Point(0, 0);
            this.panelLeftContainer.Margin = new System.Windows.Forms.Padding(4);
            this.panelLeftContainer.Name = "panelLeftContainer";
            this.panelLeftContainer.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.panelLeftContainer.Size = new System.Drawing.Size(320, 1476);
            this.panelLeftContainer.TabIndex = 0;
            // 
            // flowLayoutPanelUsers
            // 
            this.flowLayoutPanelUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelUsers.AutoScroll = true;
            this.flowLayoutPanelUsers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelUsers.Location = new System.Drawing.Point(16, 70);
            this.flowLayoutPanelUsers.Margin = new System.Windows.Forms.Padding(6);
            this.flowLayoutPanelUsers.Name = "flowLayoutPanelUsers";
            this.flowLayoutPanelUsers.Size = new System.Drawing.Size(258, 703);
            this.flowLayoutPanelUsers.TabIndex = 1;
            this.flowLayoutPanelUsers.WrapContents = false;
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.picSearchIcon);
            this.panelSearch.Controls.Add(this.picMicrophoneIcon);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(16, 12);
            this.panelSearch.Margin = new System.Windows.Forms.Padding(4);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(288, 54);
            this.panelSearch.TabIndex = 0;
            // 
            // picSearchIcon
            // 
            this.picSearchIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picSearchIcon.Image = global::GUI.Properties.Resources.search_icon;
            this.picSearchIcon.Location = new System.Drawing.Point(227, 17);
            this.picSearchIcon.Name = "picSearchIcon";
            this.picSearchIcon.Size = new System.Drawing.Size(24, 24);
            this.picSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSearchIcon.TabIndex = 6;
            this.picSearchIcon.TabStop = false;
            // 
            // picMicrophoneIcon
            // 
            this.picMicrophoneIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picMicrophoneIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMicrophoneIcon.Image = global::GUI.Properties.Resources.mic_icon;
            this.picMicrophoneIcon.Location = new System.Drawing.Point(257, 17);
            this.picMicrophoneIcon.Name = "picMicrophoneIcon";
            this.picMicrophoneIcon.Size = new System.Drawing.Size(24, 24);
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
            this.txtSearch.Location = new System.Drawing.Point(-10, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(231, 50);
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
            this.panelDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F);
            this.panelDetails.Location = new System.Drawing.Point(0, 0);
            this.panelDetails.Margin = new System.Windows.Forms.Padding(4);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Padding = new System.Windows.Forms.Padding(20);
            this.panelDetails.Size = new System.Drawing.Size(1264, 1476);
            this.panelDetails.TabIndex = 1;
            this.panelDetails.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDetails_Paint);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.BorderRadius = 25;
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.LightGray;
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.btnCancel.FillColor = System.Drawing.Color.LightGray;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(71)))), ((int)(((byte)(80)))));
            this.btnCancel.Location = new System.Drawing.Point(697, 616);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(177, 68);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.BorderRadius = 25;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.LightGray;
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(432, 616);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(224, 68);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Lưu thay đổi";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanelDetails
            // 
            this.tableLayoutPanelDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelDetails.ColumnCount = 4;
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDetails.Controls.Add(this.lblUserID, 0, 0);
            this.tableLayoutPanelDetails.Controls.Add(this.txtUserID, 1, 0);
            this.tableLayoutPanelDetails.Controls.Add(this.lblFullName, 2, 0);
            this.tableLayoutPanelDetails.Controls.Add(this.txtFullName, 3, 0);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDateOfBirth, 0, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.dtpDateOfBirth, 1, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDepartment, 2, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.lblPhoneNumber, 0, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.txtPhoneNumber, 1, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.lblEmail, 2, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.txtEmail, 3, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.lblGender, 0, 3);
            this.tableLayoutPanelDetails.Controls.Add(this.lblPosition, 2, 3);
            this.tableLayoutPanelDetails.Controls.Add(this.lblHometown, 0, 4);
            this.tableLayoutPanelDetails.Controls.Add(this.lblNationality, 2, 4);
            this.tableLayoutPanelDetails.Controls.Add(this.lblCurrentAddress, 0, 5);
            this.tableLayoutPanelDetails.Controls.Add(this.txtCurrentAddress, 1, 5);
            this.tableLayoutPanelDetails.Controls.Add(this.cmbGender, 1, 3);
            this.tableLayoutPanelDetails.Controls.Add(this.cmbPosition, 3, 3);
            this.tableLayoutPanelDetails.Controls.Add(this.cmbDepartment, 3, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.txtHometown, 1, 4);
            this.tableLayoutPanelDetails.Controls.Add(this.txtNationality, 3, 4);
            this.tableLayoutPanelDetails.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.tableLayoutPanelDetails.Location = new System.Drawing.Point(23, 76);
            this.tableLayoutPanelDetails.Name = "tableLayoutPanelDetails";
            this.tableLayoutPanelDetails.RowCount = 6;
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDetails.Size = new System.Drawing.Size(1218, 511);
            this.tableLayoutPanelDetails.TabIndex = 1;
            // 
            // lblUserID
            // 
            this.lblUserID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserID.AutoSize = true;
            this.lblUserID.Font = new System.Drawing.Font("Segoe UI", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserID.Location = new System.Drawing.Point(3, 0);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(96, 71);
            this.lblUserID.TabIndex = 0;
            this.lblUserID.Text = "Mã nhân viên";
            this.lblUserID.Click += new System.EventHandler(this.lblUserID_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserID.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtUserID.BorderRadius = 33;
            this.txtUserID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUserID.DefaultText = "";
            this.txtUserID.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtUserID.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtUserID.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtUserID.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtUserID.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtUserID.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtUserID.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtUserID.Location = new System.Drawing.Point(110, 8);
            this.txtUserID.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.txtUserID.MaxLength = 10;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.PlaceholderText = "";
            this.txtUserID.SelectedText = "";
            this.txtUserID.Size = new System.Drawing.Size(490, 55);
            this.txtUserID.TabIndex = 0;
            // 
            // lblFullName
            // 
            this.lblFullName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblFullName.Location = new System.Drawing.Point(611, 0);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(97, 71);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "Họ và tên";
            // 
            // txtFullName
            // 
            this.txtFullName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtFullName.BorderRadius = 33;
            this.txtFullName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFullName.DefaultText = "";
            this.txtFullName.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtFullName.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtFullName.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtFullName.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtFullName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtFullName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtFullName.Location = new System.Drawing.Point(719, 8);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.PlaceholderText = "";
            this.txtFullName.SelectedText = "";
            this.txtFullName.Size = new System.Drawing.Size(491, 55);
            this.txtFullName.TabIndex = 1;
            // 
            // lblDateOfBirth
            // 
            this.lblDateOfBirth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateOfBirth.AutoSize = true;
            this.lblDateOfBirth.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblDateOfBirth.Location = new System.Drawing.Point(3, 71);
            this.lblDateOfBirth.Name = "lblDateOfBirth";
            this.lblDateOfBirth.Size = new System.Drawing.Size(96, 81);
            this.lblDateOfBirth.TabIndex = 4;
            this.lblDateOfBirth.Text = "Ngày sinh";
            // 
            // dtpDateOfBirth
            // 
            this.dtpDateOfBirth.BorderRadius = 33;
            this.dtpDateOfBirth.Checked = true;
            this.dtpDateOfBirth.CustomFormat = "dd/MM/yyyy";
            this.dtpDateOfBirth.FillColor = System.Drawing.Color.White;
            this.dtpDateOfBirth.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.dtpDateOfBirth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateOfBirth.Location = new System.Drawing.Point(105, 74);
            this.dtpDateOfBirth.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDateOfBirth.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDateOfBirth.Name = "dtpDateOfBirth";
            this.dtpDateOfBirth.Size = new System.Drawing.Size(500, 64);
            this.dtpDateOfBirth.TabIndex = 2;
            this.dtpDateOfBirth.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            // 
            // lblDepartment
            // 
            this.lblDepartment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblDepartment.Location = new System.Drawing.Point(611, 71);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(97, 81);
            this.lblDepartment.TabIndex = 6;
            this.lblDepartment.Text = "Phòng ban";
            // 
            // lblPhoneNumber
            // 
            this.lblPhoneNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPhoneNumber.AutoSize = true;
            this.lblPhoneNumber.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblPhoneNumber.Location = new System.Drawing.Point(3, 152);
            this.lblPhoneNumber.Name = "lblPhoneNumber";
            this.lblPhoneNumber.Size = new System.Drawing.Size(96, 84);
            this.lblPhoneNumber.TabIndex = 8;
            this.lblPhoneNumber.Text = "Số điện thoại";
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtPhoneNumber.BorderRadius = 33;
            this.txtPhoneNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPhoneNumber.DefaultText = "";
            this.txtPhoneNumber.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtPhoneNumber.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtPhoneNumber.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtPhoneNumber.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtPhoneNumber.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtPhoneNumber.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtPhoneNumber.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtPhoneNumber.Location = new System.Drawing.Point(110, 160);
            this.txtPhoneNumber.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.PlaceholderText = "";
            this.txtPhoneNumber.SelectedText = "";
            this.txtPhoneNumber.Size = new System.Drawing.Size(490, 64);
            this.txtPhoneNumber.TabIndex = 4;
            this.txtPhoneNumber.TextChanged += new System.EventHandler(this.txtPhoneNumber_TextChanged);
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblEmail.Location = new System.Drawing.Point(611, 171);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(97, 45);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtEmail.BorderRadius = 33;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.DefaultText = "";
            this.txtEmail.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtEmail.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtEmail.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtEmail.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtEmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtEmail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtEmail.Location = new System.Drawing.Point(719, 160);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PlaceholderText = "";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(491, 64);
            this.txtEmail.TabIndex = 5;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // lblGender
            // 
            this.lblGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblGender.Location = new System.Drawing.Point(3, 236);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(96, 80);
            this.lblGender.TabIndex = 12;
            this.lblGender.Text = "Giới tính";
            // 
            // lblPosition
            // 
            this.lblPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPosition.AutoSize = true;
            this.lblPosition.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblPosition.Location = new System.Drawing.Point(611, 236);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(97, 80);
            this.lblPosition.TabIndex = 14;
            this.lblPosition.Text = "Chức vụ";
            // 
            // lblHometown
            // 
            this.lblHometown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHometown.AutoSize = true;
            this.lblHometown.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblHometown.Location = new System.Drawing.Point(3, 324);
            this.lblHometown.Name = "lblHometown";
            this.lblHometown.Size = new System.Drawing.Size(96, 90);
            this.lblHometown.TabIndex = 16;
            this.lblHometown.Text = "Quê quán";
            // 
            // lblNationality
            // 
            this.lblNationality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNationality.AutoSize = true;
            this.lblNationality.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblNationality.Location = new System.Drawing.Point(611, 324);
            this.lblNationality.Name = "lblNationality";
            this.lblNationality.Size = new System.Drawing.Size(97, 90);
            this.lblNationality.TabIndex = 18;
            this.lblNationality.Text = "Quốc tịch";
            // 
            // lblCurrentAddress
            // 
            this.lblCurrentAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentAddress.AutoSize = true;
            this.lblCurrentAddress.Location = new System.Drawing.Point(3, 422);
            this.lblCurrentAddress.Name = "lblCurrentAddress";
            this.lblCurrentAddress.Size = new System.Drawing.Size(96, 89);
            this.lblCurrentAddress.TabIndex = 20;
            this.lblCurrentAddress.Text = "Nơi ở hiện nay";
            // 
            // txtCurrentAddress
            // 
            this.txtCurrentAddress.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtCurrentAddress.BorderRadius = 33;
            this.tableLayoutPanelDetails.SetColumnSpan(this.txtCurrentAddress, 3);
            this.txtCurrentAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCurrentAddress.DefaultText = "";
            this.txtCurrentAddress.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtCurrentAddress.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtCurrentAddress.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtCurrentAddress.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtCurrentAddress.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtCurrentAddress.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtCurrentAddress.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtCurrentAddress.Location = new System.Drawing.Point(110, 430);
            this.txtCurrentAddress.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.txtCurrentAddress.Multiline = true;
            this.txtCurrentAddress.Name = "txtCurrentAddress";
            this.txtCurrentAddress.PlaceholderText = "";
            this.txtCurrentAddress.SelectedText = "";
            this.txtCurrentAddress.Size = new System.Drawing.Size(1100, 64);
            this.txtCurrentAddress.TabIndex = 10;
            // 
            // cmbGender
            // 
            this.cmbGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbGender.BackColor = System.Drawing.Color.Transparent;
            this.cmbGender.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbGender.BorderRadius = 33;
            this.cmbGender.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbGender.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbGender.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbGender.ForeColor = System.Drawing.Color.Black;
            this.cmbGender.ItemHeight = 55;
            this.cmbGender.Items.AddRange(new object[] {
            "Nữ",
            "Nam",
            "Khác"});
            this.cmbGender.Location = new System.Drawing.Point(105, 245);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(500, 61);
            this.cmbGender.TabIndex = 6;
            // 
            // cmbPosition
            // 
            this.cmbPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPosition.BackColor = System.Drawing.Color.Transparent;
            this.cmbPosition.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbPosition.BorderRadius = 33;
            this.cmbPosition.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPosition.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbPosition.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbPosition.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbPosition.ForeColor = System.Drawing.Color.Black;
            this.cmbPosition.ItemHeight = 55;
            this.cmbPosition.Location = new System.Drawing.Point(714, 245);
            this.cmbPosition.Name = "cmbPosition";
            this.cmbPosition.Size = new System.Drawing.Size(501, 61);
            this.cmbPosition.TabIndex = 7;
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDepartment.BackColor = System.Drawing.Color.Transparent;
            this.cmbDepartment.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbDepartment.BorderRadius = 33;
            this.cmbDepartment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbDepartment.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.cmbDepartment.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbDepartment.ForeColor = System.Drawing.Color.Black;
            this.cmbDepartment.ItemHeight = 55;
            this.cmbDepartment.Location = new System.Drawing.Point(714, 81);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(501, 61);
            this.cmbDepartment.TabIndex = 3;
            // 
            // txtHometown
            // 
            this.txtHometown.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtHometown.BorderRadius = 33;
            this.txtHometown.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtHometown.DefaultText = "";
            this.txtHometown.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtHometown.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtHometown.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtHometown.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtHometown.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtHometown.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtHometown.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtHometown.Location = new System.Drawing.Point(110, 324);
            this.txtHometown.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.txtHometown.Name = "txtHometown";
            this.txtHometown.PlaceholderText = "";
            this.txtHometown.SelectedText = "";
            this.txtHometown.Size = new System.Drawing.Size(490, 64);
            this.txtHometown.TabIndex = 8;
            // 
            // txtNationality
            // 
            this.txtNationality.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtNationality.BorderRadius = 33;
            this.txtNationality.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNationality.DefaultText = "Việt Nam";
            this.txtNationality.DisabledState.BorderColor = System.Drawing.Color.LightGray;
            this.txtNationality.DisabledState.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtNationality.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.txtNationality.DisabledState.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtNationality.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtNationality.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtNationality.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.txtNationality.Location = new System.Drawing.Point(719, 324);
            this.txtNationality.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.txtNationality.Name = "txtNationality";
            this.txtNationality.PlaceholderText = "";
            this.txtNationality.ReadOnly = true;
            this.txtNationality.SelectedText = "";
            this.txtNationality.Size = new System.Drawing.Size(491, 64);
            this.txtNationality.TabIndex = 9;
            // 
            // panelDetailHeader
            // 
            this.panelDetailHeader.Controls.Add(this.btnEdit);
            this.panelDetailHeader.Controls.Add(this.btnDelete);
            this.panelDetailHeader.Controls.Add(this.btnAdd);
            this.panelDetailHeader.Controls.Add(this.btnHide);
            this.panelDetailHeader.Controls.Add(this.lblDetailTitle);
            this.panelDetailHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDetailHeader.Location = new System.Drawing.Point(20, 20);
            this.panelDetailHeader.Margin = new System.Windows.Forms.Padding(4);
            this.panelDetailHeader.Name = "panelDetailHeader";
            this.panelDetailHeader.Size = new System.Drawing.Size(1224, 49);
            this.panelDetailHeader.TabIndex = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Image = global::GUI.Properties.Resources.edit_icon;
            this.btnEdit.Location = new System.Drawing.Point(886, 13);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(20, 20);
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
            this.btnDelete.Location = new System.Drawing.Point(936, 13);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 20);
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
            this.btnAdd.Location = new System.Drawing.Point(834, 13);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(20, 20);
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
            this.btnHide.Location = new System.Drawing.Point(784, 13);
            this.btnHide.Margin = new System.Windows.Forms.Padding(4);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(20, 20);
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
            this.lblDetailTitle.Location = new System.Drawing.Point(4, 8);
            this.lblDetailTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(721, 71);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "Thông tin chi tiết nhân viên";
            // 
            // AdminEmployeeManagementContentUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.splitContainerMain);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AdminEmployeeManagementContentUI";
            this.Size = new System.Drawing.Size(1589, 1476);
            this.Load += new System.EventHandler(this.AdminEmployeeManagementContentUI_Load);
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
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.PictureBox picSearchIcon;
        private System.Windows.Forms.PictureBox picMicrophoneIcon;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.Panel panelDetailHeader;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDetails;
        private System.Windows.Forms.Label lblUserID;
        private Guna2TextBox txtUserID;
        private System.Windows.Forms.Label lblFullName;
        private Guna2TextBox txtFullName;
        private System.Windows.Forms.Label lblDateOfBirth;
        private Guna2DateTimePicker dtpDateOfBirth;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.Label lblPhoneNumber;
        private Guna2TextBox txtPhoneNumber;
        private System.Windows.Forms.Label lblEmail;
        private Guna2TextBox txtEmail;
        private System.Windows.Forms.Label lblGender;
        private Guna2ComboBox cmbGender;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblHometown;
        private Guna2TextBox txtHometown;
        private System.Windows.Forms.Label lblNationality;
        private Guna2TextBox txtNationality;
        private System.Windows.Forms.Label lblCurrentAddress;
        private Guna2TextBox txtCurrentAddress;
        private Guna2Button btnCancel;
        private Guna2Button btnSave;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelLeftContainer;
        private Guna2ComboBox cmbDepartment;
        private Guna2ComboBox cmbPosition;
        private System.Windows.Forms.PictureBox btnEdit;
        private System.Windows.Forms.PictureBox btnDelete;
        private System.Windows.Forms.PictureBox btnAdd;
        private System.Windows.Forms.PictureBox btnHide;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelUsers;
    }
}