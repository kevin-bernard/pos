using cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace accouting_system_manager
{
    public partial class FormLogin : Form
    {
        public delegate void LoginSuccess();

        public event LoginSuccess OnLoginSuccess;

        public delegate void Close();

        public event Close OnClose;
        
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            var pass = StringCipher.Decrypt(Properties.Settings.Default.AUTH_PASS);
            var login = StringCipher.Decrypt(Properties.Settings.Default.AUTH_LOGIN);

            if (login.Trim() != string.Empty && pass.Trim() != string.Empty && txtLogin.Text.Trim().Equals(login) && txtPassword.Text.Equals(pass))
            {
                OnLoginSuccess?.Invoke();
                Close();
            }
            else
            {
                lblError.Visible = true;
            }
        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnClose?.Invoke();
        }
    }
}
