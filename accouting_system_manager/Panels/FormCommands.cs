using accouting_system_manager.DB.Entities;
using accouting_system_manager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static accouting_system_manager.Util.ReportAction;

namespace accouting_system_manager
{
    public partial class FormCommands : Form
    {
        public delegate void ListViewReloaded();

        public event ListViewReloaded OnListViewReloaded;

        public delegate void DateTimeUpdated(DateTime from, DateTime to);

        public event DateTimeUpdated OnDateTimeUpdated;

        public delegate void WorkerStarted();

        public event WorkerStarted OnWorkerStarted;

        public delegate void WorkerDone();

        public event WorkerDone OnWorkerDone;

        public Panel GetPanel { get { return this.panel1; } }

        public DateTime From { get { return dtPickerFrom.Value; } }

        public DateTime To { get { return dtPickerTo.Value; } }

        private double TotalPriceDeletedInvoice { get; set; }

        public FormCommands()
        {
            InitializeComponent();

            ReloadListView();
            DisplayHidePanelsRemoveInvoices();
            
            dtPickerFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtPickerTo.Value = dtPickerFrom.Value.Date.AddMonths(1).AddDays(-1);

            bgWorker.DoWork += WorkOnDeletion;
            bgWorker.ProgressChanged += WorkerReportProgress;
            bgWorker.RunWorkerCompleted += WorkerCompleted;
        }
        
        private void lstViewOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isEnabled = lstViewOrders.SelectedItems.Count > 0;

            btnRecover.Enabled = isEnabled;
        }

        private void ReloadListView()
        {
            var invoices = InvoiceService.GetRemovedInvoices(From, To);
            lstViewOrders.Items.Clear();

            TotalPriceDeletedInvoice = 0;

            foreach (var invno in invoices.Keys)
            {

                InvoiceListViewItem item = new InvoiceListViewItem(invno);

                string qty = string.Empty,
                       invdate = string.Empty,
                       name = string.Empty;
                
                item.SubItems.Add(invoices[invno].First().invdate);

                item.SubItems.Add(invoices[invno].First().deletedat);

                item.SubItems.Add(invoices[invno].First().totalprice.ToString());

                item.Invoices = invoices[invno];

                lstViewOrders.Items.Add(item);

                TotalPriceDeletedInvoice += invoices[invno].First().totalprice;
            }

            lblInfo.Text = string.Format("Total price: {0}, Nb Invoices: {1}", Math.Round(TotalPriceDeletedInvoice, 3).ToString(), invoices.Count);
            lblInfo.Visible = true;

            OnListViewReloaded?.Invoke();
        }

        private void numUpDownPercent_ValueChanged(object sender, EventArgs e)
        {
            btnRemoveByPercent.Enabled = numUpDownPercent.Value > 0;
        }
        
        private void rdbtnPercent_CheckedChanged(object sender, EventArgs e)
        {
            DisplayHidePanelsRemoveInvoices();
            
        }

        private void rdbtnMinMax_CheckedChanged(object sender, EventArgs e)
        {
            DisplayHidePanelsRemoveInvoices();
        }

        private void DisplayHidePanelsRemoveInvoices()
        {
            pnlRemoveByMinMax.Visible = rdbtnMinMax.Checked;
            pnlRemoveByPercent.Visible = rdbtnPercent.Checked;
        }
        
        private void dtPickerFrom_ValueChanged(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void dtPickerTo_ValueChanged(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            ReloadListView();
            OnDateTimeUpdated?.Invoke(dtPickerFrom.Value, dtPickerTo.Value);
        }

        private void btnRemoveByMinMax_Click(object sender, EventArgs e)
        {
            numericUpDown1.BackColor = Color.White;
            numericUpDown2.BackColor = Color.White;

            if (numericUpDown1.Value < numericUpDown2.Value)
            {
                var value = (new Random()).Next((int)numericUpDown1.Value, (int)numericUpDown2.Value);

                StartWorker(new RemoveInvoiceArgs()
                {
                    OperationType = RemoveInvoiceArgs.Type.DELETE_BY_MIN_MAX,
                    From = dtPickerFrom.Value,
                    To = dtPickerTo.Value,
                    Value = value
                });
            }
            else
            {
                numericUpDown1.BackColor = Color.OrangeRed;
                numericUpDown2.BackColor = Color.OrangeRed;
            }
        }

        private void btnRemoveByPercent_Click(object sender, EventArgs e)
        {
            if (numUpDownPercent.Value > 0)
            {
                StartWorker(new RemoveInvoiceArgs()
                {
                    OperationType = RemoveInvoiceArgs.Type.DELETE_BY_PERCENT,
                    From = dtPickerFrom.Value,
                    To = dtPickerTo.Value,
                    Value = numUpDownPercent.Value
                });
            }
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            List<Artran> invoices = new List<Artran>();

            foreach (InvoiceListViewItem item in lstViewOrders.SelectedItems)
            {
                invoices.AddRange(item.Invoices);
            }

            StartWorker(new RemoveInvoiceArgs()
            {
                OperationType = RemoveInvoiceArgs.Type.RECOVER,
                Invoices = invoices
            });
        }

        private void ResetPrgBarre()
        {
            var t = new System.Windows.Forms.Timer();
            t.Interval = 1000;

            t.Tick += (object sender, EventArgs e) =>
            {
                prgBarre.Value = 0;
                t.Stop();
            };

            t.Start();
        }

        private void PreparePrgBarre()
        {
            prgBarre.Maximum = 100;
            prgBarre.Value = 0;

            prgBarre.Visible = true;
        }

        private void StartWorker(RemoveInvoiceArgs args)
        {
            OnWorkerStarted?.Invoke();

            EnableOrDisableControls(false);

            lblInfo.Text = "Start...";
            lblInfo.Visible = true;

            bgWorker.RunWorkerAsync(args);
        }

        private void WorkOnDeletion(object sender, DoWorkEventArgs e) {

            var arg = e.Argument as RemoveInvoiceArgs;

            if (arg != null)
            {
                switch (arg.OperationType)
                {
                    case RemoveInvoiceArgs.Type.DELETE_BY_PERCENT:
                        InvoiceService.RemoveNbPercent(arg.Value, arg.From, arg.To, UpdateWorker);
                        break;
                    case RemoveInvoiceArgs.Type.DELETE_BY_MIN_MAX:

                        InvoiceService.RemoveUntilCaReached((int)arg.Value, arg.From, arg.To, UpdateWorker);

                        break;
                    case RemoveInvoiceArgs.Type.RECOVER:

                        InvoiceService.RestoreInvoices(arg.Invoices, UpdateWorker);

                        break;
                }
            }
        }

        private void UpdateWorker(RunActionProgress progress)
        {
            double percent = progress.NbData > 0 ? ((double)progress.Cursor / progress.NbData) * 100 : 100;

            bgWorker.ReportProgress((int)percent, progress);

            Thread.Sleep(1000);
        }

        private void WorkerReportProgress(object sender, ProgressChangedEventArgs e)
        {
            prgBarre.Value = e.ProgressPercentage;

            if (e.UserState as RunActionProgress != null)
            {
                lblInfo.Text = ((RunActionProgress)e.UserState).Message;
            }
            
        }

        private void WorkerCompleted(object sender, EventArgs e)
        {
            if(!bgWorker.IsBusy)
            {
                ReloadListView();

                ResetPrgBarre();

                EnableOrDisableControls();

                btnRecover.Enabled = lstViewOrders.SelectedItems.Count > 0;

                OnWorkerDone?.Invoke();
            }
        }

        private void btnPreviousMonth_Click(object sender, EventArgs e)
        {
            var startDate = new DateTime(dtPickerTo.Value.Date.Year, dtPickerTo.Value.Date.Month, 1).AddMonths(-1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            dtPickerFrom.Value = startDate;
            dtPickerTo.Value = endDate;
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            var startDate = new DateTime(dtPickerTo.Value.Date.Year, dtPickerTo.Value.Date.Month, 1).AddMonths(1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            dtPickerFrom.Value = startDate;
            dtPickerTo.Value = endDate;
        }

        private void EnableOrDisableControls(bool enable = true)
        {
            btnRecover.Enabled = enable;
            btnRemoveByMinMax.Enabled = enable;
            btnRemoveByPercent.Enabled = enable;
            btnNextMonth.Enabled = enable;
            btnPreviousMonth.Enabled = enable;
            lstViewOrders.Enabled = enable;
            dtPickerFrom.Enabled = enable;
            dtPickerTo.Enabled = enable;
        }
    }

    class RemoveInvoiceArgs
    {

        public enum Type
        {
            DELETE_BY_PERCENT = 1,
            DELETE_BY_MIN_MAX = 2,
            RECOVER = 3
        }

        public Type OperationType { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public decimal Value { get; set; }

        public List<Artran> Invoices { get; set; }
    }


    class InvoiceListViewItem : ListViewItem
    {
        public List<Artran> Invoices { get; set; }

        public string Invno { get; set; }

        public InvoiceListViewItem(string invno) : base(invno)
        {
            Invno = invno;
        }
    }

}
