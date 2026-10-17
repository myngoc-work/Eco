using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using UserControl = System.Windows.Forms.UserControl;

namespace GUI.Admin
{
    public partial class ucCustomerItem : UserControl
    {
        private readonly Color _defaultBackColor = Color.FromArgb(236, 250, 236);
        private readonly Color _selectedBackColor = Color.FromArgb(28, 99, 63);
        private readonly Color _defaultForeColor = Color.Black;
        private readonly Color _selectedForeColor = Color.White;

        private string _id;
        private string _companyName;
        private string _contactName;
        private string _contactPhone;
        private bool _isHidden = false;

        public ucCustomerItem()
        {
            InitializeComponent();
            this.BackColor = _defaultBackColor; // Đặt màu nền mặc định

            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => this.OnClick(e);
            }

            this.Resize += (s, e) => ApplyRoundedRegion(12);
            this.Load += (s, e) => ApplyRoundedRegion(12);
        }

        /// <summary>
        /// Cập nhật dữ liệu hiển thị trên control
        /// </summary>
        public void SetData(string id, string companyName, string contactName, string contactPhone)
        {
            _id = id;
            _companyName = companyName;
            _contactName = contactName;
            _contactPhone = contactPhone;
            this.Tag = id;
            UpdateDisplay();
        }

        public void SetHidden(bool isHidden)
        {
            _isHidden = isHidden;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_isHidden)
            {
                // Ẩn: chỉ hiển thị mã khách hàng, làm nhạt màu
                lblCompanyName.Text = _id ?? "";
                lblCompanyName.ForeColor = Color.FromArgb(180, 180, 180);
                lblContactName.Text = "";
                lblContactPhone.Text = "";
            }
            else
            {
                // Hiện: hiển thị đầy đủ
                string companyText = string.IsNullOrWhiteSpace(_companyName) ? "(Chưa có tên công ty)" : _companyName;
                string contactText = string.IsNullOrWhiteSpace(_contactName) ? "(Chưa có người liên hệ)" : _contactName;
                string phoneText = string.IsNullOrWhiteSpace(_contactPhone) ? "(Chưa có số điện thoại)" : _contactPhone;

                lblCompanyName.Text = companyText;
                lblCompanyName.ForeColor = _defaultForeColor;
                lblContactName.Text = contactText;
                lblContactName.ForeColor = _defaultForeColor;
                lblContactPhone.Text = phoneText;
                lblContactPhone.ForeColor = SystemColors.ControlDarkDark;

                toolTip.SetToolTip(this, companyText);
                toolTip.SetToolTip(lblCompanyName, companyText);
                toolTip.SetToolTip(lblContactName, companyText);
                toolTip.SetToolTip(lblContactPhone, companyText);
            }
        }

        /// <summary>
        /// Đặt trạng thái được chọn (selected) cho control, thay đổi cả màu nền và màu chữ
        /// </summary>
        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                this.BackColor = _selectedBackColor;
                lblCompanyName.ForeColor = _selectedForeColor;
                lblContactName.ForeColor = _selectedForeColor;
                lblContactPhone.ForeColor = _selectedForeColor;
            }
            else
            {
                this.BackColor = _defaultBackColor;
                // Áp dụng lại màu theo trạng thái ẩn/hiện
                UpdateDisplay();
            }
        }


        private void ApplyRoundedRegion(int radius)
        {
            try
            {
                var path = new GraphicsPath();
                Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
                int diameter = radius * 2;
                Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));
                path.AddArc(arc, 180, 90);
                arc.X = bounds.Right - diameter;
                path.AddArc(arc, 270, 90);
                arc.Y = bounds.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                arc.X = bounds.Left;
                path.AddArc(arc, 90, 90);
                path.CloseFigure();
                this.Region = new Region(path);
            }
            catch { }
        }

        private void ucCustomerItem_Load(object sender, EventArgs e)
        {

        }
    }
}