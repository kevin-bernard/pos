﻿using accouting_system_manager.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace accouting_system_manager.Panels
{
    public partial class FormConfigureDB : Form
    {

        public Panel GetPanel { get { return this.pnlMain; } }

        public delegate void LoginAttempt(bool isSuccess);

        public event LoginAttempt OnLoginAttempt;

        public FormConfigureDB()
        {
            InitializeComponent();

            txtServerName.Text = Properties.Settings.Default.SERVER_NAME;
            txtDbName.Text = Properties.Settings.Default.DB_NAME;
            txtUsername.Text = Properties.Settings.Default.DB_USER;
            txtPassword.Text = Properties.Settings.Default.DB_PASSWORD;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (DBManager.Connect(txtServerName.Text, txtDbName.Text, txtUsername.Text, txtPassword.Text))
            {
                Properties.Settings.Default.SERVER_NAME = txtServerName.Text;
                Properties.Settings.Default.DB_NAME = txtDbName.Text;
                Properties.Settings.Default.DB_USER = txtUsername.Text;
                Properties.Settings.Default.DB_PASSWORD = txtPassword.Text;
                Properties.Settings.Default.Save();

                DBManager.Init();

                MessageBox.Show("Succes login!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                OnLoginAttempt?.Invoke(true);
            }
            else {
                MessageBox.Show("authentication error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                OnLoginAttempt?.Invoke(false);
            }
        }
    }
}
