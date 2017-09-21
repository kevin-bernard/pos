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

namespace password_gen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            txtRes.Text = StringCipher.Encrypt(txtPass.Text);
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            btnGenerate.Enabled = txtPass.Text.Trim().Length > 0;
        }
    }
}
