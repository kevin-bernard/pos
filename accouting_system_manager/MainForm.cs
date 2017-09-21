using accouting_system_manager.DB;
using accouting_system_manager.License.Exceptions;
using accouting_system_manager.Panels;
using accouting_system_manager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace accouting_system_manager
{
    public partial class MainForm : Form
    {
        private int childFormNumber = 0;

        private bool isEnabled = false;

        private DateTime from = DateTime.Now;
         
        private DateTime to = DateTime.Now;

        bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                pnlContent.Enabled = isEnabled;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "TLic Files (*.lic)|";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {

            if (DBManager.Init())
            {
                DisplayRemoveInvoiceForm();
                btnRemoveInvoices.SelectItem();

                ReloadCa();
            }
            else
            {
                MessageBox.Show("can't connect to DB, please check your credentials", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pnlContent.Controls.Clear();
                var frm = new FormConfigureDB();

                frm.OnLoginAttempt += (bool isSuccess) => {
                    btnRemoveInvoices.Enabled = isSuccess;

                    if (isSuccess)
                    {
                        ReloadCa();
                    }
                };

                pnlContent.Controls.Add(frm.GetPanel);

                btnConfigureDB.SelectItem();
                btnRemoveInvoices.UnSelectItem();
                btnRemoveInvoices.Enabled = false;
            }

            License.LicenseManager.Load(Encoding.UTF8.GetString(Properties.Resources.lic));

            Timer t = new Timer();

            t.Tick += checkLicenseState;

            t.Interval = ((1000 * 60) * 60);//1000 * 60;

            t.Start();

            checkLicenseState(null, EventArgs.Empty);
        }

        private void checkLicenseState(object sender, EventArgs e)
        {
            IsEnabled = License.LicenseManager.IsValid;
        }

        private void btnLicense_Click(object sender, EventArgs e)
        {

        }

        private void btnRemoveInvoices_onItemClicked(object sender, EventArgs e)
        {
            if (!btnRemoveInvoices.IsSelected)
            {
                DisplayRemoveInvoiceForm();
            }

            btnRemoveInvoices.SelectItem();
            btnConfigureDB.UnSelectItem();
        }

        private void btnConfigureDB_onItemClicked(object sender, EventArgs e)
        {
            if (!btnConfigureDB.IsSelected)
            {
                pnlContent.Controls.Clear();
                pnlContent.Controls.Add(new FormConfigureDB().GetPanel);
            }

            btnRemoveInvoices.UnSelectItem();
            btnConfigureDB.SelectItem();
        }
        
        private void ReloadCa()
        {
            var ca = InvoiceService.GetCa(from, to);

            lblCa.Text = "CA: ";
            var i = 0;

            foreach (var currency in ca.Keys)
            {
                if (i++ > 0) lblCa.Text += "\r\n";

                lblCa.Text += string.Format("{0} ({1})", ca[currency].ToString(), currency);
            }

            if (ca.Count == 0) lblCa.Text += "0";
        }

        private void OnRefreshedLstInvoices() {
            ReloadCa();
        }

        private void DisplayRemoveInvoiceForm()
        {
            pnlContent.Controls.Clear();

            var frm = new FormCommands();
            frm.OnDateTimeUpdated += (DateTime f, DateTime t) => {
                from = f;
                to = t;

                ReloadCa();
            };

            frm.OnWorkerStarted += () =>
            {
                pnlButtons.Enabled = false;
            };

            frm.OnWorkerDone += () =>
            {
                pnlButtons.Enabled = true;
            };

            from = frm.From;
            to = frm.To;

            frm.OnListViewReloaded += OnRefreshedLstInvoices;

            ReloadCa();

            pnlContent.Controls.Add(frm.GetPanel);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
