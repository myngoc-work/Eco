namespace GUI.Admin
{
    partial class ucUserItem
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
            this.lblLine1 = new System.Windows.Forms.Label();
            this.lblLine2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblLine1
            // 
            this.lblLine1.AutoSize = true;
            this.lblLine1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLine1.Location = new System.Drawing.Point(30, 23);
            this.lblLine1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new System.Drawing.Size(248, 41);
            this.lblLine1.TabIndex = 0;
            this.lblLine1.Text = "[Họ Tên Đầy Đủ]";
            // 
            // lblLine2
            // 
            this.lblLine2.AutoSize = true;
            this.lblLine2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLine2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLine2.Location = new System.Drawing.Point(32, 81);
            this.lblLine2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblLine2.Name = "lblLine2";
            this.lblLine2.Size = new System.Drawing.Size(184, 32);
            this.lblLine2.TabIndex = 1;
            this.lblLine2.Text = "[Chức vụ] ([Mã])";
            // 
            // ucUserItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(250)))), ((int)(((byte)(236)))));
            this.Controls.Add(this.lblLine2);
            this.Controls.Add(this.lblLine1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Margin = new System.Windows.Forms.Padding(20, 9, 20, 9);
            this.Name = "ucUserItem";
            this.Size = new System.Drawing.Size(317, 123);
            this.Load += new System.EventHandler(this.ucUserItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLine1;
        private System.Windows.Forms.Label lblLine2;
    }
}