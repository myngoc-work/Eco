using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Authentication;


namespace GUI
{
    public partial class LoadScreen : Form
    {
       
        private void LoadScreen_Load(object sender, EventArgs e)
        {
            timer1.Interval = 5;
            timer1.Start();
        }
    }
}