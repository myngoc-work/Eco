using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Admin
{
    public partial class AdminManagementUI : UserControl
    {
        public AdminManagementUI()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose các tab controls trước để tránh lỗi
                try
                {
                    if (tabControlMain != null && !tabControlMain.IsDisposed && tabControlMain.IsHandleCreated)
                    {
                        foreach (TabPage tabPage in tabControlMain.TabPages)
                        {
                            foreach (Control control in tabPage.Controls)
                            {
                                if (control is IDisposable disposable)
                                {
                                    try
                                    {
                                        disposable.Dispose();
                                    }
                                    catch { }
                                }
                            }
                            tabPage.Controls.Clear();
                        }
                    }
                }
                catch { }
                
                // Dispose components
                try
                {
                    if (components != null)
                    {
                        components.Dispose();
                        components = null;
                    }
                }
                catch { }
            }
            base.Dispose(disposing);
        }

        private void AdminManagementUI_Load(object sender, EventArgs e)
        {
            // Tải tab đầu tiên mặc định
            LoadTabContent(tabControlMain.SelectedTab);
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ tải nội dung khi tab được chọn
            LoadTabContent(tabControlMain.SelectedTab);
        }

        private void LoadTabContent(TabPage selectedPage)
        {
            UserControl content = null;

            if (selectedPage == tabPageUserManagement)
            {
                // Nếu tab đã có control, refresh dữ liệu
                if (selectedPage.Controls.Count > 0)
                {
                    var existingControl = selectedPage.Controls[0] as AdminEmployeeManagementContentUI;
                    if (existingControl != null)
                    {
                        existingControl.RefreshData();
                        return;
                    }
                }
                content = new AdminEmployeeManagementContentUI();
            }
            else if (selectedPage == tabPageCustomerManagement)
            {
                // Nếu tab đã có control, refresh dữ liệu
                if (selectedPage.Controls.Count > 0)
                {
                    var existingControl = selectedPage.Controls[0] as AdminCustomerManagementContentUI;
                    if (existingControl != null)
                    {
                        existingControl.RefreshData();
                        return;
                    }
                }
                content = new AdminCustomerManagementContentUI();
            }
            else if (selectedPage == tabPageOrderManagement)
            {
                // Nếu tab đã có control, refresh dữ liệu
                if (selectedPage.Controls.Count > 0)
                {
                    var existingControl = selectedPage.Controls[0] as AdminOrderManagementContentUI;
                    if (existingControl != null)
                    {
                        existingControl.RefreshData();
                        return;
                    }
                }
                content = new AdminOrderManagementContentUI();
            }
            if (content != null)
            {
                content.Dock = DockStyle.Fill;
                selectedPage.Controls.Add(content);
            }
        }

        private void tabControlMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl == null) return;
            Graphics g = e.Graphics;
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var pen = new Pen(Color.FromArgb(28, 99, 63), 2))
            {
                // Xóa nền của toàn bộ control
                using (var backBrush = new SolidBrush(this.BackColor))
                {
                    g.FillRectangle(backBrush, this.ClientRectangle);
                }

                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    Rectangle tabRect = tabControl.GetTabRect(i);
                    bool isSelected = (i == e.Index);

                    using (var textBrush = new SolidBrush(isSelected ? Color.FromArgb(28, 99, 63) : Color.Gray))
                    {
                        g.DrawString(tabControl.TabPages[i].Text, tabControl.Font, textBrush, tabRect, sf);
                    }

                    if (isSelected)
                    {
                        g.DrawLine(pen, tabRect.Left, tabRect.Bottom - 1, tabRect.Right, tabRect.Bottom - 1);
                    }
                }
            }
        }
    }
}