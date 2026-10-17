using BLL;
using DTO;
using GUI.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Profile : UserControl
    {
        private UserDTO _currentUser;
        private UserBLL _userBLL;
        private string _avatarFolderPath;

        public Profile()
        {
            InitializeComponent();
            _userBLL = new UserBLL();
            
            string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Application.StartupPath)));
            _avatarFolderPath = Path.Combine(projectPath, "GUI", "Resources", "Avatars");
            
            if (!Directory.Exists(_avatarFolderPath))
            {
                Directory.CreateDirectory(_avatarFolderPath);
            }
            ApplyLanguage(Thread.CurrentThread.CurrentUICulture.Name);


            // Removed realtime validation highlights per request; validation occurs on Save only
        }
        //  ngôn ngữ
        public void ApplyLanguage(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            ResourceManager rm = new ResourceManager("GUI.Resources.Lang", typeof(HomeForm).Assembly);

            lblEmployeeInfo_Title.Text = rm.GetString("EmployeeInfo_Title");
            lblEmployeeID.Text = rm.GetString("EmployeeID");
            lblFullName.Text = rm.GetString("FullName");
            lblEmail.Text = rm.GetString("Email");
            lblDateOfBirth.Text = rm.GetString("DateOfBirth");
            lblDepartment.Text = rm.GetString("Department");
            lblPhoneNumber.Text = rm.GetString("PhoneNumber");
            lblPosition.Text = rm.GetString("Position");
            lblGender.Text = rm.GetString("Gender");
            lblCurrentAddress.Text = rm.GetString("Address");
            btnSaveProfile.Text = rm.GetString("SaveProfile");
            btnChangePhoto.Text = rm.GetString("ChangePhoto");

        }   
        public Profile(UserDTO user) : this()
        {
            _currentUser = user;
            LoadUserProfile();
        }

        public void UpdateUser(UserDTO user)
        {
            _currentUser = user;
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Không có thông tin người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // hiện thông tin
                txtID.Text = _currentUser.Company_UserId;
                txtFullName.Text = _currentUser.FullName;
                txtEmail.Text = _currentUser.Email ?? "";
                txtPhoneNumber.Text = _currentUser.Phone ?? "";
                
                // phòng ban - vô hiệu hóa không cho sửa
                txtDepartment.Text = _currentUser.DepartmentName ?? "";
                txtDepartment.Enabled = false;
                txtDepartment.ReadOnly = true;
                
                // vai trò
                txtPosition.Text = _currentUser.RoleName ?? "";
                
                 // ngày sinh,gtinh,địachỉ
                 txtDateOfBirth.Text = _currentUser.DateOfBirth?.ToString("dd/MM/yyyy") ?? "";
                 txtGender.Text = _currentUser.Gender ?? "";
                 txtCurrentAddress.Text = _currentUser.Address ?? "";
                 
                 // hiển thị ảnh đại diện
                 LoadUserAvatar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadUserAvatar()
        {
            try
            {
                if (!string.IsNullOrEmpty(_currentUser.AvatarPath) && File.Exists(_currentUser.AvatarPath))
                {
                    guna2PictureBox1.Image = Image.FromFile(_currentUser.AvatarPath);
                }
                else
                {
                    guna2PictureBox1.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ảnh đại diện: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2PictureBox1.Image = null;
            }
        }

        private void btnChangePhoto_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                    openFileDialog.Title = "Chọn ảnh đại diện";
                    openFileDialog.Multiselect = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFilePath = openFileDialog.FileName;
                        
                        // tạo tên file mới dựa trên UserID
                        string fileName = $"{_currentUser.Company_UserId}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(selectedFilePath)}";
                        string newFilePath = Path.Combine(_avatarFolderPath, fileName);

                        // copy ảnh vào thư mục Resources/Avatars
                        File.Copy(selectedFilePath, newFilePath, true);

                        // Cập nhật database
                        if (_userBLL.UpdateUserAvatar(_currentUser.Company_UserId, newFilePath))
                        {

                            _currentUser.AvatarPath = newFilePath;
                            
                            //hiện ảnh mới
                            guna2PictureBox1.Image = Image.FromFile(newFilePath);
                            
                            MessageBox.Show("Cập nhật ảnh đại diện thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật ảnh đại diện. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            
                            // Xóa file đã copy nếu cập nhật database thất bại
                            if (File.Exists(newFilePath))
                            {
                                File.Delete(newFilePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đổi ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentUser == null)
                {
                    MessageBox.Show("Không có thông tin người dùng để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate họ tên (không chứa số)
                if (!string.IsNullOrWhiteSpace(txtFullName.Text) && txtFullName.Text.Any(char.IsDigit))
                {
                    MessageBox.Show("Họ tên không được chứa số.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFullName.Focus();
                    txtFullName.SelectAll();
                    return;
                }

                // Validate số điện thoại
                if (!string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
                {
                    string cleanPhone = txtPhoneNumber.Text.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
                    if (!(cleanPhone.Length == 10 && cleanPhone.StartsWith("0") && cleanPhone.All(char.IsDigit)))
                    {
                        MessageBox.Show("Số điện thoại phải có 10 chữ số và bắt đầu từ số 0.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPhoneNumber.Focus();
                        txtPhoneNumber.SelectAll();
                        return;
                    }
                }

                // Validate email
                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    try
                    {
                        var addr = new MailAddress(txtEmail.Text);
                        if (addr.Address != txtEmail.Text)
                        {
                            throw new FormatException();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Email không đúng định dạng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        txtEmail.SelectAll();
                        return;
                    }
                }

                // Parse ngày sinh (chấp nhận dd/MM/yyyy hoặc yyyy-MM-dd)
                DateTime? parsedDob = null;
                if (!string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
                {
                    var raw = txtDateOfBirth.Text.Trim();
                    string[] fmts = new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd" };
                    DateTime dob;
                    if (DateTime.TryParseExact(raw, fmts, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dob))
                    {
                        parsedDob = dob;
                    }
                    else
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ. Định dạng hợp lệ: dd/MM/yyyy hoặc yyyy-MM-dd.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDateOfBirth.Focus();
                        txtDateOfBirth.SelectAll();
                        return;
                    }
                }

                // Cập nhật thông tin từ các trường có thể chỉnh sửa
                _currentUser.FullName = txtFullName.Text;
                _currentUser.DateOfBirth = parsedDob;
                // Không cập nhật DepartmentName vì đã vô hiệu hóa ô phòng ban
                // _currentUser.DepartmentName = txtDepartment.Text;
                _currentUser.Phone = txtPhoneNumber.Text;
                _currentUser.Email = txtEmail.Text;
                _currentUser.Address = txtCurrentAddress.Text;

                // Gọi BLL để cập nhật thông tin
                bool success = _userBLL.UpdateUserProfile(_currentUser);

                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi cập nhật thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
