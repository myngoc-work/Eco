using BLL;
using BLL.Utils;
using DTO;
using GUI.Authentication;
using GUI.Authentication.ResetPassword;
using GUI.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GUI.Authentication
{
    public partial class LoginForm : Form
    {
        private readonly UserBLL _userBLL;
        private char defaultPasswordChar;
        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            //new CustomTitleBar(this, "Đăng nhập", Properties.Resources.logo);
            _userBLL = new UserBLL();
            defaultPasswordChar = txtPassword.PasswordChar;
            pbTogglePassword.Image = Properties.Resources.eye_closed;
            pbTogglePassword.Cursor = Cursors.Hand;
        }
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi phương thức AuthenticateUser từ BLL
            UserDTO authenticatedUser = _userBLL.AuthenticateUser(username, password);

            if (authenticatedUser != null)
            {
                if (authenticatedUser.IsTwoFactorEnabled)
                {
                    // Người dùng đã bật 2FA, hiển thị form TOTP để xác thực
                    using (TOTPForm totpForm = new TOTPForm(authenticatedUser))
                    {
                        DialogResult result = totpForm.ShowDialog();

                        if (result == DialogResult.OK && totpForm.IsTOTPVerified)
                        {
                            // Lưu user vào UserSession
                            UserSession.Instance.Login(authenticatedUser);
                            MessageBox.Show($"Chào mừng, {authenticatedUser.FullName}! (Đăng nhập 2FA)", "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HomeForm homeForm = new HomeForm(authenticatedUser);
                            this.Hide();
                            homeForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Xác thực 2 yếu tố thất bại hoặc bị hủy. Đăng nhập bị từ chối.", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    // Người dùng chưa bật 2FA, cho phép đăng nhập ngay
                    // Lưu user vào UserSession
                    UserSession.Instance.Login(authenticatedUser);
                    MessageBox.Show($"Chào mừng, {authenticatedUser.FullName}!", "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HomeForm homeForm = new HomeForm(authenticatedUser);
                    this.Hide();
                    homeForm.Show();
                }
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private byte[] HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPasswordForm forgotPasswordForm = new ForgotPasswordForm();
            forgotPasswordForm.ShowDialog();

        }

        private void txtPassword_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pbTogglePassword_Click(object sender, EventArgs e)
        {
            if (txtPassword.PasswordChar == '\0')
            {
                txtPassword.PasswordChar = defaultPasswordChar;
                pbTogglePassword.Image = Properties.Resources.eye_closed;
            }
            else
            {
                txtPassword.PasswordChar = '\0';
                
                pbTogglePassword.Image = Properties.Resources.eye_open;
            }
        }
    }
}