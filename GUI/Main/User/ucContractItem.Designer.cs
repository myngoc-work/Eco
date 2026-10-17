namespace GUI
{
    partial class ucContractItem
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
            this.lblContractID = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblDueDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblContractID
            // 
            this.lblContractID.AutoSize = true;
            this.lblContractID.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContractID.Location = new System.Drawing.Point(26, 19);
            this.lblContractID.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblContractID.Name = "lblContractID";
            this.lblContractID.Size = new System.Drawing.Size(114, 41);
            this.lblContractID.TabIndex = 0;
            this.lblContractID.Text = "HD001";
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(28, 71);
            this.lblCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(235, 32);
            this.lblCompanyName.TabIndex = 1;
            this.lblCompanyName.Text = "Cty Môi trường xanh";
            // 
            // lblDueDate
            // 
            this.lblDueDate.AutoSize = true;
            this.lblDueDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDueDate.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDueDate.Location = new System.Drawing.Point(28, 115);
            this.lblDueDate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.Size = new System.Drawing.Size(278, 32);
            this.lblDueDate.TabIndex = 2;
            this.lblDueDate.Text = "Ngày trả KQ: 10/10/2025";
            // 
            // ucContractItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(250)))), ((int)(((byte)(236)))));
            this.Controls.Add(this.lblDueDate);
            this.Controls.Add(this.lblCompanyName);
            this.Controls.Add(this.lblContractID);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(20, 19, 20, 19);
            this.Name = "ucContractItem";
            this.Size = new System.Drawing.Size(480, 173);
            this.Load += new System.EventHandler(this.ucContractItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblContractID;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblDueDate;
    }
}