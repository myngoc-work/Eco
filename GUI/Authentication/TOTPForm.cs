using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OtpNet;

namespace GUI
{
    public partial class TOTPForm : Form
    {
        private readonly UserBLL _userBLL;

        private UserDTO _authenticatedUser;
        public bool IsTOTPVerified { get; private set; } = false;

        public TOTPForm(UserDTO authenticatedUser)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _authenticatedUser = authenticatedUser;
            _userBLL = new UserBLL();
            IsTOTPVerified = false;
            this.Text = "Xác thực hai yếu tố (TOTP)";
            this.Load += TOTPForm_Load;
        }
        private void TOTPForm_Load(object sender, EventArgs e) 
        {
            txtTOTPInput.Focus(); 
            if (_authenticatedUser == null) 
            {
                MessageBox.Show("Không có thông tin người dùng. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Abort;
                this.Close();
                return;
            }
        }


        private void Button_CancelOTP_Click(object sender, EventArgs e)
        {
            IsTOTPVerified = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string enteredCode = txtTOTPInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(enteredCode))
            {
                MessageBox.Show("Vui lòng nhập mã TOTP.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_authenticatedUser == null) 
            {
                MessageBox.Show("Không có thông tin người dùng. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

         
            bool isValid = _userBLL.VerifyTwoFactorCode(_authenticatedUser.Company_UserId, enteredCode);

            if (isValid)
            {
                MessageBox.Show("Xác thực TOTP thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsTOTPVerified = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Mã TOTP không hợp lệ. Vui lòng kiểm tra và thử lại.", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTOTPInput.Clear();
                txtTOTPInput.Focus();
            }
        
        }
    }
}
