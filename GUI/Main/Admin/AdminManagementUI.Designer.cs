namespace GUI.Admin
{
    partial class AdminManagementUI
    {
        private System.ComponentModel.IContainer components = null;

        // Dispose method đã được di chuyển sang AdminManagementUI.cs để dispose các tab controls trước

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblMainTitle = new System.Windows.Forms.Label();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageUserManagement = new System.Windows.Forms.TabPage();
            this.tabPageCustomerManagement = new System.Windows.Forms.TabPage();
            this.tabPageOrderManagement = new System.Windows.Forms.TabPage();
            this.tabControlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMainTitle
            // 
            this.lblMainTitle.AutoSize = true;
            this.lblMainTitle.Font = new System.Drawing.Font("Segoe UI", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblMainTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.lblMainTitle.Location = new System.Drawing.Point(25, 9);
            this.lblMainTitle.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblMainTitle.Name = "lblMainTitle";
            this.lblMainTitle.Size = new System.Drawing.Size(645, 92);
            this.lblMainTitle.TabIndex = 4;
            this.lblMainTitle.Text = "Chức năng quản lý";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageUserManagement);
            this.tabControlMain.Controls.Add(this.tabPageCustomerManagement);
            this.tabControlMain.Controls.Add(this.tabPageOrderManagement);
            this.tabControlMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlMain.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabControlMain.ItemSize = new System.Drawing.Size(150, 55);
            this.tabControlMain.Location = new System.Drawing.Point(41, 72);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(6);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1188, 838);
            this.tabControlMain.TabIndex = 5;
            this.tabControlMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControlMain_DrawItem);
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageUserManagement
            // 
            this.tabPageUserManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tabPageUserManagement.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabPageUserManagement.Location = new System.Drawing.Point(4, 59);
            this.tabPageUserManagement.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageUserManagement.Name = "tabPageUserManagement";
            this.tabPageUserManagement.Padding = new System.Windows.Forms.Padding(6);
            this.tabPageUserManagement.Size = new System.Drawing.Size(1180, 775);
            this.tabPageUserManagement.TabIndex = 0;
            this.tabPageUserManagement.Text = "Quản lý nhân viên";
            // 
            // tabPageCustomerManagement
            // 
            this.tabPageCustomerManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tabPageCustomerManagement.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabPageCustomerManagement.Location = new System.Drawing.Point(4, 59);
            this.tabPageCustomerManagement.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageCustomerManagement.Name = "tabPageCustomerManagement";
            this.tabPageCustomerManagement.Padding = new System.Windows.Forms.Padding(6);
            this.tabPageCustomerManagement.Size = new System.Drawing.Size(1554, 796);
            this.tabPageCustomerManagement.TabIndex = 1;
            this.tabPageCustomerManagement.Text = "Quản lý khách hàng";
            // 
            // tabPageOrderManagement
            // 
            this.tabPageOrderManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.tabPageOrderManagement.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabPageOrderManagement.Location = new System.Drawing.Point(4, 59);
            this.tabPageOrderManagement.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageOrderManagement.Name = "tabPageOrderManagement";
            this.tabPageOrderManagement.Size = new System.Drawing.Size(1554, 796);
            this.tabPageOrderManagement.TabIndex = 2;
            this.tabPageOrderManagement.Text = "Quản lý đơn hàng";
            // 
            // AdminManagementUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.lblMainTitle);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AdminManagementUI";
            this.Size = new System.Drawing.Size(2070, 1321);
            this.Load += new System.EventHandler(this.AdminManagementUI_Load);
            this.tabControlMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblMainTitle;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageUserManagement;
        private System.Windows.Forms.TabPage tabPageCustomerManagement;
        private System.Windows.Forms.TabPage tabPageOrderManagement;
    }
}