using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI.Admin
{
    public partial class ucUserItem : UserControl
    {
        private readonly Color _defaultBackColor = Color.FromArgb(236, 250, 236);
        private readonly Color _selectedBackColor = Color.FromArgb(28, 99, 63);
        private readonly Color _defaultForeColor = Color.Black;
        private readonly Color _selectedForeColor = Color.White;

        public ucUserItem()
        {
            InitializeComponent();
            this.BackColor = _defaultBackColor;

            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => this.OnClick(e);
            }

            this.Resize += (s, e) => ApplyRoundedRegion(12);
            this.Load += (s, e) => ApplyRoundedRegion(12);
        }

        private string _fullName;
        private string _roleName;
        private string _userId;
        private bool _isHidden = false;

        public void SetData(string line1, string line2)
        {
            _fullName = line1; // Họ tên
            _roleName = line2; // "Chức vụ (Mã)"
            
            // Parse để lấy mã số từ line2 (format: "Chức vụ (Mã)")
            if (line2.Contains("(") && line2.Contains(")"))
            {
                int startIdx = line2.LastIndexOf("(") + 1;
                int endIdx = line2.LastIndexOf(")");
                if (startIdx > 0 && endIdx > startIdx)
                {
                    _userId = line2.Substring(startIdx, endIdx - startIdx);
                }
            }
            
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
                // Ẩn: chỉ hiển thị tên và mã số, làm nhạt màu
                lblLine1.Text = _fullName;
                lblLine1.ForeColor = Color.FromArgb(180, 180, 180); // Màu nhạt
                lblLine2.Text = _userId ?? "";
                lblLine2.ForeColor = Color.FromArgb(180, 180, 180); // Màu nhạt
            }
            else
            {
                // Hiện: hiển thị đầy đủ
                lblLine1.Text = _fullName;
                lblLine1.ForeColor = _defaultForeColor;
                lblLine2.Text = _roleName;
                lblLine2.ForeColor = SystemColors.ControlDarkDark;
            }
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                this.BackColor = _selectedBackColor;
                lblLine1.ForeColor = _selectedForeColor;
                lblLine2.ForeColor = _selectedForeColor;
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

        private void ucUserItem_Load(object sender, EventArgs e)
        {

        }
    }
}