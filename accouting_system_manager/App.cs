using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace accouting_system_manager
{
    public class App : ApplicationContext
    {
        public App() : base()
        {
            var loginForm = new FormLogin();
            var mainForm = new MainForm();

            loginForm.OnLoginSuccess += () =>
            {
                mainForm.Show();
            };

            loginForm.OnClose += () =>
            {
                if(!mainForm.Visible) Application.Exit();
            };

            loginForm.Show();
        }
    }
}
