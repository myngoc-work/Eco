using GUI.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GUI
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            Label lbl = new Label();
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            this.Controls.Add(lbl);
            lbl.Text = "Công ty TNHH EcoGreen\n\nĐơn vị tiên phong trong giám sát và bảo vệ môi trường.";
            ApplyLanguage(Thread.CurrentThread.CurrentUICulture.Name);
        }
        public void ApplyLanguage(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);

            ResourceManager rm = new ResourceManager("GUI.Resources.Lang", typeof(HomeForm).Assembly);

            lblTitle.Text = rm.GetString("About_Title");
            lblParagraph.Text = rm.GetString("About_Paragraph");
            lblSlogan.Text = rm.GetString("About_Slogan");
        }

        private Image CreateTransparentImage(Image original, float opacity)
        {
            Bitmap transparent = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(transparent))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity; // 0.0f = trong suốt, 1.0f = rõ nét
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(original,
                    new Rectangle(0, 0, transparent.Width, transparent.Height),
                    0, 0, original.Width, original.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }
            return transparent;
        }

        private void guna2GradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientPanel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lblSlogan_Click(object sender, EventArgs e)
        {

        }
    }
}
