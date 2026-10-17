namespace GUI.Admin
{
    partial class ucOrderItem
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
            this.lblOrderId = new System.Windows.Forms.Label();
            this.lblCustomerId = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblOrderId
            // 
            this.lblOrderId.AutoSize = true;
            this.lblOrderId.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblOrderId.Location = new System.Drawing.Point(30, 18);
            this.lblOrderId.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(168, 41);
            this.lblOrderId.TabIndex = 0;
            this.lblOrderId.Text = "[Mã Đơn]";
            // 
            // lblCustomerId
            // 
            this.lblCustomerId.AutoSize = true;
            this.lblCustomerId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCustomerId.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCustomerId.Location = new System.Drawing.Point(32, 148);
            this.lblCustomerId.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCustomerId.Name = "lblCustomerId";
            this.lblCustomerId.Size = new System.Drawing.Size(198, 32);
            this.lblCustomerId.TabIndex = 3;
            this.lblCustomerId.Text = "[Mã Khách Hàng]";
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(32, 66);
            this.lblCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(220, 37);
            this.lblCompanyName.TabIndex = 1;
            this.lblCompanyName.Text = "[Tên doanh nghiệp]";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCustomerName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCustomerName.Location = new System.Drawing.Point(32, 108);
            this.lblCustomerName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(201, 35);
            this.lblCustomerName.TabIndex = 2;
            this.lblCustomerName.Text = "[Tên khách hàng]";
            // 
            // ucOrderItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(250)))), ((int)(((byte)(236)))));
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.lblCompanyName);
            this.Controls.Add(this.lblCustomerId);
            this.Controls.Add(this.lblOrderId);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(20, 9, 20, 9);
            this.Name = "ucOrderItem";
            this.Size = new System.Drawing.Size(387, 190);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrderId;
        private System.Windows.Forms.Label lblCustomerId;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblCustomerName;
    }
}