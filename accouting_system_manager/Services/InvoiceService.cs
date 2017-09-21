using accouting_system_manager.DB;
using accouting_system_manager.DB.Entities;
using accouting_system_manager.Log;
using accouting_system_manager.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static accouting_system_manager.Util.ReportAction;

namespace accouting_system_manager.Services
{
    public static class InvoiceService
    {
        const string DATE_DB_FORMAT = "yyyy-MM-dd";

        private delegate void Inserted(bool success, string keyValue);

        public static Dictionary<string, List<Artran>> GetRemovedInvoices(DateTime from, DateTime to)
        {
            Dictionary<string, List<Artran>> invoices = new Dictionary<string, List<Artran>>();

            DBManager.RunQueryResults(string.Format("SELECT * FROM artrand WHERE invdate BETWEEN '{0} 00:00:00' AND '{1} 23:59:59' ORDER BY invdate", GetStrDateDbFormat(from), GetStrDateDbFormat(to)), (SqlDataReader reader) =>
            {
                var invno = reader["invno"].ToString();

                if (!invoices.ContainsKey(invno))
                {
                    invoices[invno] = new List<Artran>();
                }

                invoices[invno].Add(new Artran()
                {
                    invno = invno,
                    fprice = reader["fprice"].ToString(),
                    invdate = reader["invdate"].ToString(),
                    itemcode = reader["itemcode"].ToString(),
                    qtyorder = reader["qtyorder"].ToString(),
                });
                
            });

            return invoices;
        }

        public static Dictionary<string, double> GetCa(DateTime from, DateTime to)
        {
            Dictionary<string, double> ca = new Dictionary<string, double>();

            DBManager.RunQueryResults(string.Format("SELECT currency, SUM(paidamt) total FROM arcash WHERE invdate BETWEEN '{0} 00:00:00' AND '{1} 23:59:59' GROUP BY currency", GetStrDateDbFormat(from), GetStrDateDbFormat(to)), (System.Data.SqlClient.SqlDataReader reader) =>
            {
                ca.Add(reader["currency"].ToString(), double.Parse(reader["total"].ToString()));
            });

            return ca;
        }
        
        public static void RestoreInvoices(List<Artran> invoices, RunAction callbackProgress) {

            int max = 1;

            callbackProgress?.Invoke(new RunActionProgress() { Cursor = 0, NbData = invoices.Count, Message = "Prepare invoices" });

            DBManager.RunQueryResults("SELECT MAX(invno) invno FROM artran", (SqlDataReader r) =>
            {
                int.TryParse(r["invno"].ToString(), out max);
            });

            int i = 0;

            Dictionary<string, string> numbers = new Dictionary<string, string>();

            foreach (var invoice in invoices) {

                i++;
                int no = 0;

                if (int.TryParse(invoice.invno, out no))
                {
                    string newNo = (max + no).ToString();

                    SetRemovedInvoiceNumber(invoice.invno, newNo);

                    invoice.invno = newNo;
                }
                
                callbackProgress?.Invoke(new RunActionProgress() { Cursor = i, NbData = invoices.Count, Message = "Prepare invoices" });
            }

            var keys = string.Join(",", invoices.Select(invoice => invoice.invno));
            
            callbackProgress?.Invoke(new RunActionProgress() { Cursor = 0, NbData = keys.Length, Message = "Restore invoices" });

            if (SwitchDataFromTableToTable("artrand", "artran", keys))
            {
                SwitchDataFromTableToTable("arcashd", "arcash", keys);
                SwitchDataFromTableToTable("ictrand", "ictran", keys, "docno");
                SwitchDataFromTableToTable("armasterd", "armaster", keys);
            }

            callbackProgress?.Invoke(new RunActionProgress() { Cursor = keys.Length, NbData = keys.Length, Message = "Restore invoices" });
            
            ReloadStockFromInvoices(invoices, callbackProgress);

            ReloadInvoiceNumbers(callbackProgress);
        }

        public static int RemoveNbPercent(decimal percent, DateTime from, DateTime to, RunAction callbackProgress)
        {
            var nbData = GetNbData(from, to);

            decimal total = 0;

            try
            {
                if (nbData > 0) total = (nbData * (percent / 100));
            }
            catch
            {
                total = 0;
            }

            total = Math.Round(total, 0);

            return RemoveNbData((int)total, from, to, callbackProgress);
        }

        public static void RemoveUntilCaReached(int minCa, DateTime from, DateTime to, RunAction callbackProgress = null)
        {
            var invoices = new List<Artran>();
            var caData = GetCa(from, to);

            DBManager.RunQueryResults(string.Format("SELECT artran.*, arcash.paidamt, arcash.currency FROM arcash inner join artran on artran.invno = arcash.invno WHERE artran.invdate BETWEEN '{0} 00:00:00' AND '{1} 23:59:59'", GetStrDateDbFormat(from), GetStrDateDbFormat(to)), (SqlDataReader reader) =>
            {
                double paidamt;
                
                if (double.TryParse(reader["paidamt"].ToString(), out paidamt) && caData.ContainsKey(reader["currency"].ToString())) {
                    if (caData[reader["currency"].ToString()] > minCa && caData[reader["currency"].ToString()] - paidamt >= minCa)
                    {
                        caData[reader["currency"].ToString()] -= paidamt;

                        invoices.Add(new Artran()
                        {
                            invno = reader["invno"].ToString(),
                            cost = reader["cost"].ToString(),
                            fprice = reader["fprice"].ToString(),
                            invdate = reader["invdate"].ToString(),
                            itemcode = reader["itemcode"].ToString(),
                            qtyorder = reader["qtyorder"].ToString(),
                        });
                    }
                    
                }

            });

            RemoveData(invoices, callbackProgress);
        }

        public static int GetNbData(DateTime from, DateTime to)
        {
            var count = 0;

            DBManager.RunQueryResults(string.Format("SELECT COUNT(*) total FROM artran WHERE invdate BETWEEN '{0} 00:00:00' AND '{1} 23:59:59'", GetStrDateDbFormat(from), GetStrDateDbFormat(to)), (System.Data.SqlClient.SqlDataReader reader) =>
            {
                count = int.Parse(reader["total"].ToString());
            });

            return count;
        }

        private static int RemoveNbData(int nbData, DateTime from, DateTime to, RunAction callbackProgress = null)
        {
            var query = string.Format("SELECT TOP {0} * FROM artran where invdate BETWEEN '{1} 00:00:00' AND '{2} 23:59:59' ORDER BY invdate DESC", nbData, GetStrDateDbFormat(from), GetStrDateDbFormat(to));
            List<Artran> invoices = new List<Artran>();

            DBManager.RunQueryResults(query, (SqlDataReader reader) =>
            {
                invoices.Add(new Artran() {
                    invno = reader["invno"].ToString(),
                    cost = reader["cost"].ToString(),
                    fprice = reader["fprice"].ToString(),
                    invdate = reader["invdate"].ToString(),
                    itemcode = reader["itemcode"].ToString(),
                    qtyorder = reader["qtyorder"].ToString(),
                }); 
            });

            return RemoveData(invoices, callbackProgress);
        }

        private static int RemoveData(List<Artran> invoices, RunAction callbackProgress = null)
        {
            var i = 0;

            var keys = string.Join(",", invoices.Select(invoice => invoice.invno));

            callbackProgress?.Invoke(new RunActionProgress() { Cursor = 0, NbData = keys.Length, Message = "Delete invoices" });

            if (SwitchDataFromTableToTable("artran", "artrand", keys))
            {
                SwitchDataFromTableToTable("arcash", "arcashd", keys);
                SwitchDataFromTableToTable("ictran", "ictrand", keys, "docno");
                SwitchDataFromTableToTable("armaster", "armasterd", keys);
            }

            callbackProgress?.Invoke(new RunActionProgress() { Cursor = keys.Length, NbData = keys.Length, Message = "Delete invoices" });
            
            ReloadStockFromInvoices(invoices, callbackProgress);

            ReloadInvoiceNumbers(callbackProgress);
            
            return i;
        }

        private static bool SwitchDataFromTableToTable(string from, string to, string ids, string key = "invno")
        {
            var success = true;

            try
            {
                DBManager.ExecuteQuery(string.Format("SET IDENTITY_INSERT {0} ON;", to));
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            var cols = GetTableColumns(from);
            cols = cols.Equals(string.Empty) ? GetTableColumns(to) : cols;

            try
            {
                DBManager.ExecuteQuery(string.Format("INSERT INTO {0} ({1}) SELECT * FROM {2} WHERE {3} IN ({4});", to, cols, from, key, ids));
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                success = false;
            }
            
            try
            {
                DBManager.ExecuteQuery(string.Format("SET IDENTITY_INSERT {0} OFF;", to));
            }
            catch { }

            if(success) DBManager.ExecuteQuery(string.Format("DELETE FROM {0} WHERE {1} IN ({2})", from, key, ids));

            return success;
        }

        private static void ReloadStockFromInvoices(List<Artran> invoices, RunAction callbackProgress = null)
        {
            var productsConsumptions = new Dictionary<string, List<Artran>>();
            int i = 0;

            foreach (var invoice in invoices)
            {
                if (!productsConsumptions.ContainsKey(invoice.itemcode) && invoice.itemcode != null) productsConsumptions[invoice.itemcode] = new List<Artran>();

                productsConsumptions[invoice.itemcode].Add(invoice);
            }

            callbackProgress?.Invoke(new RunActionProgress() { Cursor =  0, NbData = productsConsumptions.Keys.Count, Message = "Reload Stock" });
            
            foreach (var itemcode in productsConsumptions.Keys)
            {
                ReloadStockForItem(itemcode);
                i++;
                callbackProgress?.Invoke(new RunActionProgress() { Cursor = i, NbData = productsConsumptions.Keys.Count, Message = "Reload Stock" });
            }
        }

        private static void ReloadStockForItem(string itemcode)
        {
            double totalQtyOrdered = 0, onHand = 0, cost = 0;
            
            string invdate = string.Empty;
            
            DBManager.RunQueryResults(string.Format("select MAX(invdate) invdate, SUM(qtyorder) qtyorder, SUM(cost * qtyorder) cost, (SELECT MAX(stockqty) FROM ictran where itemcode='{0}' AND trantype='R') - (CASE WHEN SUM(qtyorder) IS NOT NULL THEN SUM(qtyorder) ELSE 0 END) onHand FROM artran WHERE itemcode='{0}';", itemcode), (SqlDataReader reader) =>
            {
                double.TryParse(reader["cost"].ToString(), out cost);
                invdate = reader["invdate"].ToString();
                double.TryParse(reader["qtyorder"].ToString(), out totalQtyOrdered);
                double.TryParse(reader["onHand"].ToString(), out onHand);
            });
            
            DBManager.ExecuteQuery(string.Format("UPDATE icitemlocation SET ytdsaleqty={0}, ptdsaleqty={0}, loconhand={1}, ptdsalevalue={2}, ytdsalevalue={2}, lastsale='{3}', edtdatetime='{3}' WHERE itemcode='{4}';", totalQtyOrdered, onHand, cost, invdate, itemcode));
            DBManager.ExecuteQuery(string.Format("UPDATE icitemqty SET qonhand={0}, issuedate='{1}', edtdatetime='{1}', salesynqty={2} WHERE itemcode='{3}';", onHand, invdate, totalQtyOrdered, itemcode));
            DBManager.ExecuteQuery(string.Format("UPDATE iccost SET conhand={0} WHERE itemcode='{1}';", onHand, itemcode));
            DBManager.ExecuteQuery(string.Format("UPDATE icitemmaster SET ytdsaleqty={0}, ptdsaleqty={0}, ytdsalevalue={1}, lastsale='{2}', qtyonhand={3} WHERE itemcode='{4}';", totalQtyOrdered, cost, invdate, onHand, itemcode));
        }

        private static void ReloadInvoiceNumbers(RunAction callbackProgress)
        {
            var progress = new RunActionProgress() { Cursor = 0, NbData = 5, Message = "Reset invoice numbering" };

            callbackProgress?.Invoke(progress);
            //
            DBManager.ExecuteQuery("DELETE FROM artrantmp");
            DBManager.ExecuteQuery("DBCC CHECKIDENT (artrantmp, RESEED, 0)");

            //arcash, ictran (reference=Invoice {id}, docno), armaster, 
            DBManager.ExecuteQuery("INSERT INTO artrantmp(invno) SELECT invno from artran group by invno order by MAX(invdate)");
                
            progress.Cursor = 1;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("update artran SET artran.invno=artrantmp.id from artran inner join artrantmp on artran.invno=artrantmp.invno");

            progress.Cursor = 2;
            callbackProgress?.Invoke(progress);
                
            DBManager.ExecuteQuery("update arcash SET arcash.invno=artrantmp.id,  ponum='Payment on invoice ' + CAST(artrantmp.id AS VARCHAR), octn='R' + CAST(artrantmp.id AS VARCHAR), applyto='l' + CAST(artrantmp.id AS VARCHAR) from arcash inner join artrantmp on arcash.invno=artrantmp.invno");

            progress.Cursor = 3;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("update ictran SET ictran.docno=artrantmp.id, reference='Invoice ' + CAST(artrantmp.id AS VARCHAR) from ictran inner join artrantmp on ictran.docno=artrantmp.invno");

            progress.Cursor = 4;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("update armaster SET armaster.invno=artrantmp.id, octn='l' + CAST(artrantmp.id AS VARCHAR) from armaster inner join artrantmp on armaster.invno=artrantmp.invno");

            int total = 0;
            double ca = 0;

            DBManager.RunQueryResults("SELECT MAX(invno) total FROM artran", (SqlDataReader reader) =>
            {
                int.TryParse(reader["total"].ToString(), out total);
            });

            DBManager.RunQueryResults("SELECT (CASE WHEN SUM(paidamt) IS NOT NULL THEN SUM(paidamt) ELSE 0 END) + (select MAX(startamt) from arshiftclose) ca FROM arcash;", (SqlDataReader r) =>
            {
                double.TryParse(r["ca"].ToString(), out ca);
            });

            DBManager.ExecuteQuery(string.Format("update arshiftclose SET invcount={0}, totalvoucher={0}, totalamt={1};", total, ca));
            DBManager.ExecuteQuery(string.Format("update sysdata SET int1={0}, int2={1}, int3={1} WHERE sysid='AR';", total, total + 1));
            DBManager.ExecuteQuery(string.Format("update sysdata SET int1={0} WHERE sysid='GL';", total + 1));
            
            progress.Cursor = 5;
            callbackProgress?.Invoke(progress);
        }

        private static void SetRemovedInvoiceNumber(string baseInvno, string newInvno)
        {
            DBManager.ExecuteQuery(string.Format("update artrand SET invno={0} WHERE invno={1}", newInvno, baseInvno));
            DBManager.ExecuteQuery(string.Format("update arcashd SET invno={0}, ponum='Payment on invoice {0}', octn='R{0}', applyto='l{0}' WHERE invno={1}", newInvno, baseInvno));
            DBManager.ExecuteQuery(string.Format("update ictrand SET docno={0}, reference='Invoice {0}' WHERE docno={1}", newInvno, baseInvno));
            DBManager.ExecuteQuery(string.Format("update armasterd SET invno={0},  octn='l{0}' WHERE invno={1}", newInvno, baseInvno));
        }

        private static string GetTableColumns(string table)
        {
            string cols = string.Empty;

            DBManager.RunQueryResults(string.Format("SELECT TOP 1 * FROM {0}", table), (SqlDataReader reader) =>
            {
                cols = reader.GetColumnsCommaSeparated();
            });

            return cols;
        }

        private static string GetStrDateDbFormat(DateTime date)
        {
            return date.ToString(DATE_DB_FORMAT);
        }
    }
}
