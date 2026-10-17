using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI.Admin
{
    public partial class ucOrderItem : UserControl
    {
        private readonly Color _defaultBackColor = Color.FromArgb(236, 250, 236);
        private readonly Color _selectedBackColor = Color.FromArgb(28, 99, 63); 
        private readonly Color _defaultForeColor = Color.Black;
        private readonly Color _selectedForeColor = Color.White;

        private string _orderId;
        private string _customerId;
        private string _companyName;
        private string _customerName;
        private bool _isHidden = false;

        public ucOrderItem()
        {
            InitializeComponent();
            this.BackColor = _defaultBackColor;

            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => this.OnClick(e);
            }

            this.Load += (s, e) => ApplyRoundedRegion(12);
            this.Resize += (s, e) => ApplyRoundedRegion(12);
        }

        public void SetData(string orderId, string customerId, string companyName, string customerName)
        {
            _orderId = orderId;
            _customerId = customerId;
            _companyName = companyName;
            _customerName = customerName;
            this.Tag = orderId;
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
                // Ẩn: chỉ hiển thị mã đơn và mã khách, làm nhạt màu
                lblOrderId.Text = string.IsNullOrWhiteSpace(_orderId) ? "(Chưa có mã đơn)" : _orderId;
                lblOrderId.ForeColor = Color.FromArgb(180, 180, 180);
                lblCustomerId.Text = string.IsNullOrWhiteSpace(_customerId) ? "(Chưa có mã khách)" : _customerId;
                lblCustomerId.ForeColor = Color.FromArgb(180, 180, 180);
                lblCompanyName.Text = "";
                lblCustomerName.Text = "";
            }
            else
            {
                // Hiện: hiển thị đầy đủ
                lblOrderId.Text = string.IsNullOrWhiteSpace(_orderId) ? "(Chưa có mã đơn)" : _orderId;
                lblOrderId.ForeColor = _defaultForeColor;
                lblCustomerId.Text = string.IsNullOrWhiteSpace(_customerId) ? "(Chưa có mã khách)" : _customerId;
                lblCustomerId.ForeColor = SystemColors.ControlDarkDark;
                lblCompanyName.Text = string.IsNullOrWhiteSpace(_companyName) ? "(Chưa có tên công ty)" : _companyName;
                lblCompanyName.ForeColor = _defaultForeColor;
                lblCustomerName.Text = string.IsNullOrWhiteSpace(_customerName) ? "(Chưa có tên khách hàng)" : _customerName;
                lblCustomerName.ForeColor = _defaultForeColor;
            }
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                this.BackColor = _selectedBackColor;
                lblOrderId.ForeColor = _selectedForeColor;
                lblCustomerId.ForeColor = _selectedForeColor;
                lblCompanyName.ForeColor = _selectedForeColor;
                lblCustomerName.ForeColor = _selectedForeColor;
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
            catch (Exception)
            {
                // Bỏ qua lỗi nếu control quá nhỏ
            }
        }
    }
}