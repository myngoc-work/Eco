namespace GUI.Main.User
{
    partial class ChangePasswordAnd2FASettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePasswordAnd2FASettings));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbToggleConfirmNewPasswordVisibility = new Guna.UI2.WinForms.Guna2PictureBox();
            this.pbToggleNewPasswordVisibility = new Guna.UI2.WinForms.Guna2PictureBox();
            this.pbToggleCurrentPasswordVisibility = new Guna.UI2.WinForms.Guna2PictureBox();
            this.txtCurrentPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtNewPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtConfirmNewPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnConfirmChange = new Guna.UI2.WinForms.Guna2Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage2FA = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl2FAStatus = new System.Windows.Forms.Label();
            this.pbQRCode = new Guna.UI2.WinForms.Guna2PictureBox();
            this.switchEnable2FA = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.lblTOTPCodeSetup = new System.Windows.Forms.Label();
            this.btnSubmit2FA = new Guna.UI2.WinForms.Guna2Button();
            this.txtTOTPCodeSetup = new Guna.UI2.WinForms.Guna2TextBox();
            this.lbl4FA = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleConfirmNewPasswordVisibility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleNewPasswordVisibility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleCurrentPasswordVisibility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2FA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2FA);
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabControl1.ItemSize = new System.Drawing.Size(200, 40);
            this.tabControl1.Location = new System.Drawing.Point(0, 5);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(2524, 1773);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AllowDrop = true;
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(84)))), ((int)(((byte)(43)))));
            this.tabPage1.Location = new System.Drawing.Point(8, 48);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(2508, 1717);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = " Đổi mật khẩu";
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.panel3.Controls.Add(this.pbToggleConfirmNewPasswordVisibility);
            this.panel3.Controls.Add(this.pbToggleNewPasswordVisibility);
            this.panel3.Controls.Add(this.pbToggleCurrentPasswordVisibility);
            this.panel3.Controls.Add(this.txtCurrentPassword);
            this.panel3.Controls.Add(this.txtNewPassword);
            this.panel3.Controls.Add(this.txtConfirmNewPassword);
            this.panel3.Controls.Add(this.btnConfirmChange);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.lblUsername);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Location = new System.Drawing.Point(4, 5);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2500, 1708);
            this.panel3.TabIndex = 13;
            // 
            // pbToggleConfirmNewPasswordVisibility
            // 
            this.pbToggleConfirmNewPasswordVisibility.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbToggleConfirmNewPasswordVisibility.Image = global::GUI.Properties.Resources.eye_closed;
            this.pbToggleConfirmNewPasswordVisibility.ImageRotate = 0F;
            this.pbToggleConfirmNewPasswordVisibility.Location = new System.Drawing.Point(1731, 820);
            this.pbToggleConfirmNewPasswordVisibility.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbToggleConfirmNewPasswordVisibility.Name = "pbToggleConfirmNewPasswordVisibility";
            this.pbToggleConfirmNewPasswordVisibility.Size = new System.Drawing.Size(40, 38);
            this.pbToggleConfirmNewPasswordVisibility.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbToggleConfirmNewPasswordVisibility.TabIndex = 74;
            this.pbToggleConfirmNewPasswordVisibility.TabStop = false;
            this.pbToggleConfirmNewPasswordVisibility.Click += new System.EventHandler(this.guna2PictureBox2_Click);
            // 
            // pbToggleNewPasswordVisibility
            // 
            this.pbToggleNewPasswordVisibility.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbToggleNewPasswordVisibility.Image = global::GUI.Properties.Resources.eye_closed;
            this.pbToggleNewPasswordVisibility.ImageRotate = 0F;
            this.pbToggleNewPasswordVisibility.Location = new System.Drawing.Point(1731, 580);
            this.pbToggleNewPasswordVisibility.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbToggleNewPasswordVisibility.Name = "pbToggleNewPasswordVisibility";
            this.pbToggleNewPasswordVisibility.Size = new System.Drawing.Size(40, 38);
            this.pbToggleNewPasswordVisibility.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbToggleNewPasswordVisibility.TabIndex = 73;
            this.pbToggleNewPasswordVisibility.TabStop = false;
            this.pbToggleNewPasswordVisibility.Click += new System.EventHandler(this.pbToggleConfirmNewPasswordVisibility_Click);
            // 
            // pbToggleCurrentPasswordVisibility
            // 
            this.pbToggleCurrentPasswordVisibility.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbToggleCurrentPasswordVisibility.Image = global::GUI.Properties.Resources.eye_closed;
            this.pbToggleCurrentPasswordVisibility.ImageRotate = 0F;
            this.pbToggleCurrentPasswordVisibility.Location = new System.Drawing.Point(1731, 339);
            this.pbToggleCurrentPasswordVisibility.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbToggleCurrentPasswordVisibility.Name = "pbToggleCurrentPasswordVisibility";
            this.pbToggleCurrentPasswordVisibility.Size = new System.Drawing.Size(40, 38);
            this.pbToggleCurrentPasswordVisibility.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbToggleCurrentPasswordVisibility.TabIndex = 72;
            this.pbToggleCurrentPasswordVisibility.TabStop = false;
            this.pbToggleCurrentPasswordVisibility.Click += new System.EventHandler(this.pbToggleCurrentPasswordVisibility_Click);
            // 
            // txtCurrentPassword
            // 
            this.txtCurrentPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.txtCurrentPassword.BorderRadius = 30;
            this.txtCurrentPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCurrentPassword.DefaultText = "";
            this.txtCurrentPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCurrentPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCurrentPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCurrentPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCurrentPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCurrentPassword.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            this.txtCurrentPassword.ForeColor = System.Drawing.Color.Black;
            this.txtCurrentPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCurrentPassword.Location = new System.Drawing.Point(862, 316);
            this.txtCurrentPassword.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtCurrentPassword.Name = "txtCurrentPassword";
            this.txtCurrentPassword.PlaceholderText = "Nhập mật khẩu hiện tại";
            this.txtCurrentPassword.SelectedText = "";
            this.txtCurrentPassword.Size = new System.Drawing.Size(944, 95);
            this.txtCurrentPassword.TabIndex = 60;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.txtNewPassword.BorderRadius = 30;
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
            this.txtNewPassword.Location = new System.Drawing.Point(862, 558);
            this.txtNewPassword.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PlaceholderText = "Nhập mật khẩu mới";
            this.txtNewPassword.SelectedText = "";
            this.txtNewPassword.Size = new System.Drawing.Size(944, 95);
            this.txtNewPassword.TabIndex = 59;
            this.txtNewPassword.TextChanged += new System.EventHandler(this.txtNewPassword_TextChanged);
            // 
            // txtConfirmNewPassword
            // 
            this.txtConfirmNewPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.txtConfirmNewPassword.BorderRadius = 30;
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
            this.txtConfirmNewPassword.Location = new System.Drawing.Point(862, 795);
            this.txtConfirmNewPassword.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtConfirmNewPassword.Name = "txtConfirmNewPassword";
            this.txtConfirmNewPassword.PlaceholderText = "Nhập lại mật khẩu mới";
            this.txtConfirmNewPassword.SelectedText = "";
            this.txtConfirmNewPassword.Size = new System.Drawing.Size(944, 95);
            this.txtConfirmNewPassword.TabIndex = 58;
            // 
            // btnConfirmChange
            // 
            this.btnConfirmChange.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.btnConfirmChange.BorderRadius = 25;
            this.btnConfirmChange.BorderThickness = 1;
            this.btnConfirmChange.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirmChange.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirmChange.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConfirmChange.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnConfirmChange.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(140)))), ((int)(((byte)(47)))));
            this.btnConfirmChange.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnConfirmChange.ForeColor = System.Drawing.Color.White;
            this.btnConfirmChange.Location = new System.Drawing.Point(1184, 1038);
            this.btnConfirmChange.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConfirmChange.Name = "btnConfirmChange";
            this.btnConfirmChange.Size = new System.Drawing.Size(291, 103);
            this.btnConfirmChange.TabIndex = 57;
            this.btnConfirmChange.Text = "Xác nhận";
            this.btnConfirmChange.Click += new System.EventHandler(this.btnConfirmChange_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(854, 702);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(419, 50);
            this.label4.TabIndex = 32;
            this.label4.Text = "Nhập lại mật khẩu mới";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(854, 467);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(264, 50);
            this.label3.TabIndex = 31;
            this.label3.Text = "Mật khẩu mới";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblUsername.ForeColor = System.Drawing.Color.Black;
            this.lblUsername.Location = new System.Drawing.Point(854, 225);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(326, 50);
            this.lblUsername.TabIndex = 28;
            this.lblUsername.Text = "Mật khẩu hiện tại";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(854, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(988, 139);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vui lòng nhập mật khẩu hiện tại và mật khẩu mới của bạn để đảm bảo an toàn tài kh" +
    "oản!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(818, 155);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1044, 1006);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 75;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage2FA
            // 
            this.tabPage2FA.Controls.Add(this.label7);
            this.tabPage2FA.Controls.Add(this.label6);
            this.tabPage2FA.Controls.Add(this.lbl2FAStatus);
            this.tabPage2FA.Controls.Add(this.pbQRCode);
            this.tabPage2FA.Controls.Add(this.switchEnable2FA);
            this.tabPage2FA.Controls.Add(this.lblTOTPCodeSetup);
            this.tabPage2FA.Controls.Add(this.btnSubmit2FA);
            this.tabPage2FA.Controls.Add(this.txtTOTPCodeSetup);
            this.tabPage2FA.Controls.Add(this.lbl4FA);
            this.tabPage2FA.Controls.Add(this.label5);
            this.tabPage2FA.Controls.Add(this.label2);
            this.tabPage2FA.Controls.Add(this.pictureBox2);
            this.tabPage2FA.Location = new System.Drawing.Point(8, 48);
            this.tabPage2FA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2FA.Name = "tabPage2FA";
            this.tabPage2FA.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2FA.Size = new System.Drawing.Size(2508, 1717);
            this.tabPage2FA.TabIndex = 1;
            this.tabPage2FA.Text = "Xác thực 2 yếu tố";
            this.tabPage2FA.UseVisualStyleBackColor = true;
            this.tabPage2FA.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.Location = new System.Drawing.Point(66, 430);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1036, 75);
            this.label7.TabIndex = 75;
            this.label7.Text = "Bước 2: Cài đặt ứng dụng Google Authenticator";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label6.Location = new System.Drawing.Point(66, 298);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(813, 62);
            this.label6.TabIndex = 74;
            this.label6.Text = "Bước 1: Bật nút Tạo khóa bí mật 2FA\r\n";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // lbl2FAStatus
            // 
            this.lbl2FAStatus.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lbl2FAStatus.Location = new System.Drawing.Point(69, 748);
            this.lbl2FAStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl2FAStatus.Name = "lbl2FAStatus";
            this.lbl2FAStatus.Size = new System.Drawing.Size(756, 103);
            this.lbl2FAStatus.TabIndex = 72;
            this.lbl2FAStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbQRCode
            // 
            this.pbQRCode.ImageRotate = 0F;
            this.pbQRCode.Location = new System.Drawing.Point(1516, 213);
            this.pbQRCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbQRCode.Name = "pbQRCode";
            this.pbQRCode.Size = new System.Drawing.Size(530, 591);
            this.pbQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbQRCode.TabIndex = 71;
            this.pbQRCode.TabStop = false;
            // 
            // switchEnable2FA
            // 
            this.switchEnable2FA.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.switchEnable2FA.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.switchEnable2FA.CheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.switchEnable2FA.CheckedState.InnerColor = System.Drawing.Color.White;
            this.switchEnable2FA.Location = new System.Drawing.Point(76, 605);
            this.switchEnable2FA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.switchEnable2FA.Name = "switchEnable2FA";
            this.switchEnable2FA.Size = new System.Drawing.Size(141, 47);
            this.switchEnable2FA.TabIndex = 70;
            this.switchEnable2FA.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.switchEnable2FA.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.switchEnable2FA.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.switchEnable2FA.UncheckedState.InnerColor = System.Drawing.Color.White;
            // 
            // lblTOTPCodeSetup
            // 
            this.lblTOTPCodeSetup.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTOTPCodeSetup.Location = new System.Drawing.Point(1312, 142);
            this.lblTOTPCodeSetup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTOTPCodeSetup.Name = "lblTOTPCodeSetup";
            this.lblTOTPCodeSetup.Size = new System.Drawing.Size(682, 66);
            this.lblTOTPCodeSetup.TabIndex = 68;
            this.lblTOTPCodeSetup.Text = "Bước 3: Mở ứng dụng và quét mã vạch\r\n";
            this.lblTOTPCodeSetup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSubmit2FA
            // 
            this.btnSubmit2FA.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.btnSubmit2FA.BorderRadius = 25;
            this.btnSubmit2FA.BorderThickness = 1;
            this.btnSubmit2FA.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSubmit2FA.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSubmit2FA.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSubmit2FA.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSubmit2FA.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(140)))), ((int)(((byte)(47)))));
            this.btnSubmit2FA.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnSubmit2FA.ForeColor = System.Drawing.Color.White;
            this.btnSubmit2FA.Location = new System.Drawing.Point(1659, 1102);
            this.btnSubmit2FA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSubmit2FA.Name = "btnSubmit2FA";
            this.btnSubmit2FA.Size = new System.Drawing.Size(264, 103);
            this.btnSubmit2FA.TabIndex = 67;
            this.btnSubmit2FA.Text = "Gửi";
            this.btnSubmit2FA.Click += new System.EventHandler(this.btnSubmit2FA_Click);
            // 
            // txtTOTPCodeSetup
            // 
            this.txtTOTPCodeSetup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(87)))), ((int)(((byte)(24)))));
            this.txtTOTPCodeSetup.BorderRadius = 30;
            this.txtTOTPCodeSetup.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTOTPCodeSetup.DefaultText = "";
            this.txtTOTPCodeSetup.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTOTPCodeSetup.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTOTPCodeSetup.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTOTPCodeSetup.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTOTPCodeSetup.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTOTPCodeSetup.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTOTPCodeSetup.ForeColor = System.Drawing.Color.Black;
            this.txtTOTPCodeSetup.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTOTPCodeSetup.Location = new System.Drawing.Point(1323, 939);
            this.txtTOTPCodeSetup.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtTOTPCodeSetup.Name = "txtTOTPCodeSetup";
            this.txtTOTPCodeSetup.PlaceholderText = "Nhập mã xác thực";
            this.txtTOTPCodeSetup.SelectedText = "";
            this.txtTOTPCodeSetup.Size = new System.Drawing.Size(855, 114);
            this.txtTOTPCodeSetup.TabIndex = 50;
            // 
            // lbl4FA
            // 
            this.lbl4FA.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lbl4FA.Location = new System.Drawing.Point(1312, 836);
            this.lbl4FA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl4FA.Name = "lbl4FA";
            this.lbl4FA.Size = new System.Drawing.Size(1114, 80);
            this.lbl4FA.TabIndex = 39;
            this.lbl4FA.Text = "Bước 4: Nhập mã xác thực do ứng dụng cung cấp";
            this.lbl4FA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.Location = new System.Drawing.Point(66, 139);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1168, 130);
            this.label5.TabIndex = 17;
            this.label5.Text = "Để kích hoạt xác thực hai yếu tố (2FA) trên tài khoản của bạn.\r\nBạn cần làm theo " +
    "các bước sau:\r\n";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(136)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(46, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1400, 86);
            this.label2.TabIndex = 16;
            this.label2.Text = "Xác thực hai yếu tố với Google Authenticator";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(734, 142);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(831, 953);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 73;
            this.pictureBox2.TabStop = false;
            // 
            // ChangePasswordAnd2FASettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ChangePasswordAnd2FASettings";
            this.Size = new System.Drawing.Size(2758, 1350);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleConfirmNewPasswordVisibility)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleNewPasswordVisibility)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbToggleCurrentPasswordVisibility)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2FA.ResumeLayout(false);
            this.tabPage2FA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2FA;
        private System.Windows.Forms.Panel panel3;
        private Guna.UI2.WinForms.Guna2TextBox txtCurrentPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtNewPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmNewPassword;
        private Guna.UI2.WinForms.Guna2Button btnConfirmChange;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl4FA;
        private Guna.UI2.WinForms.Guna2TextBox txtTOTPCodeSetup;
        private Guna.UI2.WinForms.Guna2Button btnSubmit2FA;
        private System.Windows.Forms.Label lblTOTPCodeSetup;
        private Guna.UI2.WinForms.Guna2PictureBox pbQRCode;
        private Guna.UI2.WinForms.Guna2ToggleSwitch switchEnable2FA;
        private System.Windows.Forms.Label lbl2FAStatus;
        private Guna.UI2.WinForms.Guna2PictureBox pbToggleConfirmNewPasswordVisibility;
        private Guna.UI2.WinForms.Guna2PictureBox pbToggleNewPasswordVisibility;
        private Guna.UI2.WinForms.Guna2PictureBox pbToggleCurrentPasswordVisibility;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}
