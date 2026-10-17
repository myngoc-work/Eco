using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Authentication.ResetPassword
{
    public partial class TypeNewPasswordForm : Form
    {
        private readonly UserBLL _userBLL;
        private readonly string _userId;
        private char defaultPasswordChar;

        public TypeNewPasswordForm(string userId)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            _userBLL = new UserBLL();
            _userId = userId;

            // Đặt thuộc tính UseSystemPasswordChar cho các TextBox mật khẩu
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmNewPassword.UseSystemPasswordChar = true;
            defaultPasswordChar = txtNewPassword.PasswordChar;
            pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_closed;
            pbToggleNewPasswordVisibility.Cursor = Cursors.Hand;
            txtNewPassword.PasswordChar = '\0';
            txtConfirmNewPassword.PasswordChar = '\0';

            pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_closed; 
            pbToggleNewPasswordVisibility.Cursor = Cursors.Hand;
            pbToggleConfirmPasswordVisibility.Image = Properties.Resources.eye_closed; 
            pbToggleConfirmPasswordVisibility.Cursor = Cursors.Hand;
        }



        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnConfirmReset_Click_1(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text;
            string confirmNewPassword = txtConfirmNewPassword.Text;

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu mới và xác nhận mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string errorMessage;
            bool success = _userBLL.ResetPassword(_userId, newPassword, confirmNewPassword, out errorMessage);

            if (success)
            {
                MessageBox.Show("Mật khẩu của bạn đã được đặt lại thành công. Vui lòng đăng nhập lại.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Đóng form đặt lại mật khẩu
            }
            else
            {
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelReset_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pbTogglePassword_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.PasswordChar == '\0')
            {
                txtNewPassword.PasswordChar = defaultPasswordChar;
                txtConfirmNewPassword.PasswordChar = defaultPasswordChar;
                pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_closed;


            }
            else
            {
                txtNewPassword.PasswordChar = '\0';
                txtConfirmNewPassword.PasswordChar = '\0';

                
                pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_open;
            }
        }

        private void pbToggleNewPasswordVisibility_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.UseSystemPasswordChar) 
            {
                txtNewPassword.UseSystemPasswordChar = false;
                pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_open;
            }
            else
            {
                txtNewPassword.UseSystemPasswordChar = true;
                pbToggleNewPasswordVisibility.Image = Properties.Resources.eye_closed;
            }
        }

        private void pbToggleConfirmPasswordVisibility_Click(object sender, EventArgs e)
        {
            if (txtConfirmNewPassword.UseSystemPasswordChar)
            {
                txtConfirmNewPassword.UseSystemPasswordChar = false;
                pbToggleConfirmPasswordVisibility.Image = Properties.Resources.eye_open;
            }
            else
            {
                txtConfirmNewPassword.UseSystemPasswordChar = true;
                pbToggleConfirmPasswordVisibility.Image = Properties.Resources.eye_closed;
            }
        }
    }
}