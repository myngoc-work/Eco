using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class ucContractItem : UserControl
    {
        public ucContractItem()
        {
            InitializeComponent();
            // This makes the entire control clickable, not just the labels
            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => this.OnClick(e);
            }
            // Rounded corners for the contract card
            this.Resize += (s, e) => ApplyRoundedRegion(12);
            this.Load += (s, e) => ApplyRoundedRegion(12);
        }

        public void SetData(string contractID, string companyName, string dueDate)
        {
            lblContractID.Text = contractID;
            lblCompanyName.Text = companyName;
            lblDueDate.Text = "Due Date: " + dueDate;
        }

        private void ApplyRoundedRegion(int radius)
        {
            try
            {
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                int w = this.Width - 1, h = this.Height - 1;
                int r = radius * 2;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(w - r, 0, r, r, 270, 90);
                path.AddArc(w - r, h - r, r, r, 0, 90);
                path.AddArc(0, h - r, r, r, 90, 90);
                path.CloseAllFigures();
                this.Region = new System.Drawing.Region(path);
            }
            catch { }
        }

        private void ucContractItem_Load(object sender, EventArgs e)
        {

        }
    }
}