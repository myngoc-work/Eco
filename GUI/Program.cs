using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Authentication;
using GUI.Main;
using GUI.Main.User;
namespace GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
            //Application.Run(new TwoFactorForm());
            //Application.Run(new ChangePasswordForm ());
            //Application.Run(new HomeForm());

        }
    }

}
