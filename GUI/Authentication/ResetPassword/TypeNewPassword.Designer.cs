namespace GUI.Authentication.ResetPassword
{
    partial class TypeNewPasswordForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnCancelReset = new Guna.UI2.WinForms.Guna2Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNewPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtConfirmNewPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnConfirmReset = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbToggleConfirmPasswordVisibility = new Guna.UI2.WinForms.Guna2PictureBox();
            this.pbToggleNewPasswordVisibility = new Guna.UI2.WinForms.Guna2PictureBox();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleConfirmPasswordVisibility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleNewPasswordVisibility)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.panel1.Controls.Add(this.btnMinimize);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(601, 43);
            this.panel1.TabIndex = 50;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.btnMinimize.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.btnMinimize.IconColor = System.Drawing.Color.Black;
            this.btnMinimize.Location = new System.Drawing.Point(500, 0);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(52, 43);
            this.btnMinimize.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(207)))));
            this.btnClose.IconColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(545, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(56, 43);
            this.btnClose.TabIndex = 0;
            // 
            // btnCancelReset
            // 
            this.btnCancelReset.BorderRadius = 25;
            this.btnCancelReset.BorderThickness = 2;
            this.btnCancelReset.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancelReset.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancelReset.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancelReset.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancelReset.FillColor = System.Drawing.Color.White;
            this.btnCancelReset.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnCancelReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.btnCancelReset.Location = new System.Drawing.Point(271, 442);
            this.btnCancelReset.Name = "btnCancelReset";
            this.btnCancelReset.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.btnCancelReset.PressedDepth = 80;
            this.btnCancelReset.Size = new System.Drawing.Size(142, 62);
            this.btnCancelReset.TabIndex = 70;
            this.btnCancelReset.Text = "Hủy";
            this.btnCancelReset.Click += new System.EventHandler(this.btnCancelReset_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(84)))), ((int)(((byte)(43)))));
            this.label2.Location = new System.Drawing.Point(106, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 38);
            this.label2.TabIndex = 36;
            this.label2.Text = "Đặt lại mật khẩu mới";
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.txtNewPassword.BorderRadius = 25;
            this.txtNewPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNewPassword.DefaultText = "";
            this.txtNewPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNewPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNewPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNewPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNewPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNewPassword.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            this.txtNewPassword.ForeColor = System.Drawing.Color.Black;
            this.txtNewPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNewPassword.Location = new System.Drawing.Point(39, 173);
            this.txtNewPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PlaceholderText = "Nhập mật khẩu mới";
            this.txtNewPassword.SelectedText = "";
            this.txtNewPassword.Size = new System.Drawing.Size(421, 61);
            this.txtNewPassword.TabIndex = 66;
            this.txtNewPassword.TextChanged += new System.EventHandler(this.txtNewPassword_TextChanged);
            // 
            // txtConfirmNewPassword
            // 
            this.txtConfirmNewPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.txtConfirmNewPassword.BorderRadius = 25;
            this.txtConfirmNewPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtConfirmNewPassword.DefaultText = "";
            this.txtConfirmNewPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtConfirmNewPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtConfirmNewPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtConfirmNewPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtConfirmNewPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtConfirmNewPassword.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            this.txtConfirmNewPassword.ForeColor = System.Drawing.Color.Black;
            this.txtConfirmNewPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtConfirmNewPassword.Location = new System.Drawing.Point(38, 323);
            this.txtConfirmNewPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtConfirmNewPassword.Name = "txtConfirmNewPassword";
            this.txtConfirmNewPassword.PlaceholderText = "Nhập lại mật khẩu mới";
            this.txtConfirmNewPassword.SelectedText = "";
            this.txtConfirmNewPassword.Size = new System.Drawing.Size(422, 61);
            this.txtConfirmNewPassword.TabIndex = 65;
            // 
            // btnConfirmReset
            // 
            this.btnConfirmReset.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.btnConfirmReset.BorderRadius = 25;
            this.btnConfirmReset.BorderThickness = 1;
            this.btnConfirmReset.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirmReset.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirmReset.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConfirmReset.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnConfirmReset.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(140)))), ((int)(((byte)(47)))));
            this.btnConfirmReset.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnConfirmReset.ForeColor = System.Drawing.Color.White;
            this.btnConfirmReset.Location = new System.Drawing.Point(56, 442);
            this.btnConfirmReset.Name = "btnConfirmReset";
            this.btnConfirmReset.Size = new System.Drawing.Size(164, 62);
            this.btnConfirmReset.TabIndex = 64;
            this.btnConfirmReset.Text = "Xác nhận";
            this.btnConfirmReset.Click += new System.EventHandler(this.btnConfirmReset_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(34, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 28);
            this.label1.TabIndex = 63;
            this.label1.Text = "Nhập lại mật khẩu mới";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(34, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 28);
            this.label5.TabIndex = 62;
            this.label5.Text = "Mật khẩu mới";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.panel3.Controls.Add(this.pbToggleConfirmPasswordVisibility);
            this.panel3.Controls.Add(this.pbToggleNewPasswordVisibility);
            this.panel3.Controls.Add(this.btnCancelReset);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txtNewPassword);
            this.panel3.Controls.Add(this.txtConfirmNewPassword);
            this.panel3.Controls.Add(this.btnConfirmReset);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(37, 50);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(510, 594);
            this.panel3.TabIndex = 49;
            // 
            // pbToggleConfirmPasswordVisibility
            // 
            this.pbToggleConfirmPasswordVisibility.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbToggleConfirmPasswordVisibility.Image = global::GUI.Properties.Resources.eye_closed;
            this.pbToggleConfirmPasswordVisibility.ImageRotate = 0F;
            this.pbToggleConfirmPasswordVisibility.Location = new System.Drawing.Point(418, 343);
            this.pbToggleConfirmPasswordVisibility.Name = "pbToggleConfirmPasswordVisibility";
            this.pbToggleConfirmPasswordVisibility.Size = new System.Drawing.Size(27, 24);
            this.pbToggleConfirmPasswordVisibility.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbToggleConfirmPasswordVisibility.TabIndex = 72;
            this.pbToggleConfirmPasswordVisibility.TabStop = false;
            this.pbToggleConfirmPasswordVisibility.Click += new System.EventHandler(this.pbToggleConfirmPasswordVisibility_Click);
            // 
            // pbToggleNewPasswordVisibility
            // 
            this.pbToggleNewPasswordVisibility.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbToggleNewPasswordVisibility.Image = global::GUI.Properties.Resources.eye_closed;
            this.pbToggleNewPasswordVisibility.ImageRotate = 0F;
            this.pbToggleNewPasswordVisibility.Location = new System.Drawing.Point(418, 189);
            this.pbToggleNewPasswordVisibility.Name = "pbToggleNewPasswordVisibility";
            this.pbToggleNewPasswordVisibility.Size = new System.Drawing.Size(27, 24);
            this.pbToggleNewPasswordVisibility.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbToggleNewPasswordVisibility.TabIndex = 71;
            this.pbToggleNewPasswordVisibility.TabStop = false;
            this.pbToggleNewPasswordVisibility.Click += new System.EventHandler(this.pbToggleNewPasswordVisibility_Click);
            // 
            // TypeNewPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 656);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TypeNewPasswordForm";
            this.Text = "TypeNewPassword";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleConfirmPasswordVisibility)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleNewPasswordVisibility)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2ControlBox btnMinimize;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private Guna.UI2.WinForms.Guna2Button btnCancelReset;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtNewPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmNewPassword;
        private Guna.UI2.WinForms.Guna2Button btnConfirmReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private Guna.UI2.WinForms.Guna2PictureBox pbToggleConfirmPasswordVisibility;
        private Guna.UI2.WinForms.Guna2PictureBox pbToggleNewPasswordVisibility;
    }
}