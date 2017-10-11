using accouting_system_manager.DB;
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
using System.Windows.Forms;
using static accouting_system_manager.Util.ReportAction;

namespace tests
{
    public partial class Form1 : Form
    {
        private int CurrentStep = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnTests_Click(object sender, EventArgs e)
        {
            if (DBManager.Init())//DBManager.Connect(".\\SQL", "ESQTESTS", string.Empty, string.Empty))
            {
                StartTests();
            }
            else
            {
                MessageBox.Show("Error login to DB");
            }
        }

        private void StartTests()
        {
            CurrentStep = 0;

            lblInfo.Text = "Test 1 - Invoice Remove Nb Percent";

            backgroundWorker1.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {
                if (++CurrentStep == 1)
                {
                    lblInfo.Text = "Test 2 - Restore invoices";
                    backgroundWorker1.RunWorkerAsync(2);
                }
                else if (CurrentStep == 2)
                {
                    
                } 
            };

            backgroundWorker1.RunWorkerAsync(0);

            //try
            //{
            //    //RunTestRemoveData();
            //    RunTestinvoiceNumbers();
            //
            //    MessageBox.Show("Success");
            //}
            //catch (Exception e)
            //{
            //    lblInfo.Text = e.Message;
            //}

        }

        private void RunTestInvoiceNumbers()
        {
            int nbData = InvoiceService.GetMaxInvoiceNum();

            var nums = InvoiceService.GetTransactionNoList();
            var firstInvno = nums.First();

            for (var i = firstInvno; i < firstInvno + nbData; i++)
            {
                if (!nums.Contains(i))
                {
                    throw new Exception(string.Format("invoice n{0} is missing", i.ToString()));
                }
            }
        }

        private void RunTestRemoveData()
        {
            var from = new DateTime(2017, 7, 1);
            var to = new DateTime(2017, 7, from.AddMonths(1).AddDays(-1).Day); ;

            var nbData = InvoiceService.GetNbData(from, to);

            TestRemoveNbPercent(70, from, to);
        }

        private void RunTestRestoreData()
        {
            var from = new DateTime(2017, 7, 1);
            var to = new DateTime(2017, 7, from.AddMonths(1).AddDays(-1).Day); ;
            
            TestRestoreData(from, to);
        }

        private void TestRemoveNbPercent(decimal nb, DateTime from, DateTime to)
        {
            var nbData = InvoiceService.GetNbData(from, to);

            InvoiceService.RemoveNbPercent(nb, from, to, UpdateWorker);

            var newNbData = InvoiceService.GetNbData(from, to);
            int expected = (int)(nbData - (nbData * (nb / 100)));

            if (newNbData != expected && ((newNbData - expected) > 1) && ((expected - newNbData) > 1)) {
                throw new Exception(string.Format("Nb data removed {0}, expected: {1}", newNbData, expected));
            }
        }

        private void TestRestoreData(DateTime from, DateTime to)
        {
            var nbData = InvoiceService.GetNbData(from, to);

            var removedData = InvoiceService.GetRemovedInvoices(from, to);

            var lstData = new List<Artran>();

            InvoiceService.RestoreInvoices(removedData.Values.First(), UpdateWorker);

            var newNbData = InvoiceService.GetNbData(from, to);

            if (newNbData != nbData + 1)
            {
                throw new Exception(string.Format("Nb data removed {0}, expected: {1}", newNbData, nbData + 1));
            }

            var invoices = new List<Artran>();

            foreach (var key in removedData.Keys) {
                invoices.AddRange(removedData[key]);
            }
            
            InvoiceService.RestoreInvoices(invoices, UpdateWorker);

            newNbData = InvoiceService.GetNbData(from, to);

            if (newNbData != nbData + removedData.Keys.Count)
            {
                throw new Exception(string.Format("Nb data removed {0}, expected: 0", newNbData));
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var action = e.Argument as int?;
            
            if (action == 0)
            {
                RunTestRemoveData();
                RunTestInvoiceNumbers();
            }
            else
            {
                RunTestRestoreData();
                RunTestInvoiceNumbers();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblInfo.Text = e.ProgressPercentage + " / 100";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Success");
        }

        private void UpdateWorker(RunActionProgress progress)
        {
            double percent = progress.NbData > 0 ? ((double)progress.Cursor / progress.NbData) * 100 : 100;

            backgroundWorker1.ReportProgress((int)percent, progress);

            Thread.Sleep(1000);
        }
    }
}
