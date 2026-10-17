namespace GUI.Admin
{
    partial class ucCustomerItem
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
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblContactName = new System.Windows.Forms.Label();
            this.lblContactPhone = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCompanyName.Location = new System.Drawing.Point(28, 20);
            this.lblCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(231, 45);
            this.lblCompanyName.TabIndex = 0;
            this.lblCompanyName.Text = "[Tên Công Ty]";
            // 
            // lblContactName
            // 
            this.lblContactName.AutoSize = true;
            this.lblContactName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContactName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblContactName.Location = new System.Drawing.Point(30, 80);
            this.lblContactName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblContactName.Name = "lblContactName";
            this.lblContactName.Size = new System.Drawing.Size(260, 37);
            this.lblContactName.TabIndex = 1;
            this.lblContactName.Text = "[Tên Người Liên Hệ]";
            // 
            // lblContactPhone
            // 
            this.lblContactPhone.AutoSize = true;
            this.lblContactPhone.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblContactPhone.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblContactPhone.Location = new System.Drawing.Point(32, 120);
            this.lblContactPhone.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblContactPhone.Name = "lblContactPhone";
            this.lblContactPhone.Size = new System.Drawing.Size(222, 35);
            this.lblContactPhone.TabIndex = 2;
            this.lblContactPhone.Text = "[Số điện thoại]";
            // 
            // ucCustomerItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(250)))), ((int)(((byte)(236)))));
            this.Controls.Add(this.lblContactPhone);
            this.Controls.Add(this.lblContactName);
            this.Controls.Add(this.lblCompanyName);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(20, 9, 20, 9);
            this.Name = "ucCustomerItem";
            this.Size = new System.Drawing.Size(367, 160);
            this.Load += new System.EventHandler(this.ucCustomerItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblContactName;
        private System.Windows.Forms.Label lblContactPhone;
        private System.Windows.Forms.ToolTip toolTip;
    }
}