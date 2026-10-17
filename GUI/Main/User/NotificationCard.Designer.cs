namespace GUI
{
    partial class NotificationCard
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblContractId = new System.Windows.Forms.Label();
            this.lblExpectedDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblOverdue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblContractId
            // 
            this.lblContractId.AutoSize = true;
            this.lblContractId.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblContractId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(83)))), ((int)(((byte)(47)))));
            this.lblContractId.Location = new System.Drawing.Point(20, 20);
            this.lblContractId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContractId.Name = "lblContractId";
            this.lblContractId.Size = new System.Drawing.Size(457, 51);
            this.lblContractId.TabIndex = 0;
            this.lblContractId.Text = "HD002 - KCN Sóng Thần";
            // 
            // lblExpectedDate
            // 
            this.lblExpectedDate.AutoSize = true;
            this.lblExpectedDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblExpectedDate.ForeColor = System.Drawing.Color.Black;
            this.lblExpectedDate.Location = new System.Drawing.Point(20, 70);
            this.lblExpectedDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExpectedDate.Name = "lblExpectedDate";
            this.lblExpectedDate.Size = new System.Drawing.Size(547, 45);
            this.lblExpectedDate.TabIndex = 1;
            this.lblExpectedDate.Text = "Ngày dự kiến trả kết quả: 03/03/2025";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(20, 120);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(181, 45);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Tình trạng: ";
            // 
            // lblOverdue
            // 
            this.lblOverdue.AutoSize = true;
            this.lblOverdue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverdue.ForeColor = System.Drawing.Color.Red;
            this.lblOverdue.Location = new System.Drawing.Point(170, 120);
            this.lblOverdue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOverdue.Name = "lblOverdue";
            this.lblOverdue.Size = new System.Drawing.Size(271, 45);
            this.lblOverdue.TabIndex = 3;
            this.lblOverdue.Text = "Quá hạn 12 ngày";
            // 
            // NotificationCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblOverdue);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblExpectedDate);
            this.Controls.Add(this.lblContractId);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NotificationCard";
            this.Size = new System.Drawing.Size(1271, 180);
            this.Load += new System.EventHandler(this.NotificationCard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblContractId;
        private System.Windows.Forms.Label lblExpectedDate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblOverdue;
    }
}
