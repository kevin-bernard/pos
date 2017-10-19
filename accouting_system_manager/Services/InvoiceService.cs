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

            DBManager.RunQueryResults(string.Format(@"SELECT artrand.*, artrandate.createdat deletedat, arcashd.paidamt total 
                                                      FROM artrand inner join arcashd on artrand.invno=arcashd.invno 
                                                                  inner join artrandate on artrandate.invno=arcashd.invno 
                                                      WHERE artrand.invdate BETWEEN '{0} 00:00:00' AND '{1} 23:59:59' 
                                                      ORDER BY artrand.invdate", GetStrDateDbFormat(from), GetStrDateDbFormat(to)), (SqlDataReader reader) =>
            {
                var invno = reader["invno"].ToString();

                if (!invoices.ContainsKey(invno))
                {
                    invoices[invno] = new List<Artran>();
                }

                var inv = new Artran()
                {
                    invno = invno,
                    fprice = reader["fprice"].ToString(),
                    invdate = reader["invdate"].ToString(),
                    itemcode = reader["itemcode"].ToString(),
                    qtyorder = reader["qtyorder"].ToString(),
                    deletedat = reader["deletedat"].ToString()
                };
                double total = 0;

                double.TryParse(reader["total"].ToString(), out total);
                inv.totalprice = Math.Round(total, 3);

                invoices[invno].Add(inv);

            });

            return invoices;
        }
        
        public static int GetMaxInvoiceNum() {
            int no = 0;

            DBManager.RunQueryResults("SELECT MAX(invno) invno FROM arcash", (SqlDataReader r) =>
            {
                int.TryParse(r["invno"].ToString(), out no);
            });

            return no;
        }

        public static List<int> GetTransactionNoList()
        {
            List<int> nums = new List<int>();

            DBManager.RunQueryResults("SELECT arcash.* FROM arcash ORDER BY arcash.invno", (SqlDataReader reader) =>
            {
                int invno = 0;

                if (int.TryParse(reader["invno"].ToString(), out invno))
                {
                    nums.Add(invno);
                }
            });

            return nums;
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

            int fstNo = 0;

            DateTime fstDate = invoices.Min(i => i.GetInvdateToDateTime());

            DBManager.RunQueryResults(string.Format("SELECT MIN(invno) invno FROM arcash WHERE invdate>='{0}'", GetStrDateDbFormat(fstDate)), (SqlDataReader r) =>
            {
                int.TryParse(r["invno"].ToString(), out fstNo);
            });
            
            invoices = PrepareInvoiceNumbers(invoices, callbackProgress);

            var keys = string.Join(",", invoices.Select(invoice => invoice.invno));
            
            callbackProgress?.Invoke(new RunActionProgress() { Cursor = 0, NbData = keys.Length, Message = "Restore invoices" });

            if (SwitchDataFromTableToTable("artrand", "artran", keys))
            {
                SwitchDataFromTableToTable("arcashd", "arcash", keys);
                SwitchDataFromTableToTable("ictrand", "ictran", keys, "docno", new List<string>() { "trantype='I'" });
                SwitchDataFromTableToTable("armasterd", "armaster", keys);
            }

            callbackProgress?.Invoke(new RunActionProgress() { Cursor = keys.Length, NbData = keys.Length, Message = "Restore invoices" });
            
            ReloadStockFromInvoices(invoices, true, callbackProgress);

            ReloadInvoiceNumbersFrom(fstNo, callbackProgress);
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

            var ids = new List<string>();

            DBManager.RunQueryResults(string.Format("SELECT * FROM arcash WHERE arcash.invdate BETWEEN '{0} 00:00:00' AND '{1} 23:59:59' ORDER BY arcash.paidamt DESC", GetStrDateDbFormat(from), GetStrDateDbFormat(to)), (SqlDataReader reader) =>
            {
                double paidamt;

                if (double.TryParse(reader["paidamt"].ToString(), out paidamt) && caData.ContainsKey(reader["currency"].ToString()))
                {
                    if (caData[reader["currency"].ToString()] > minCa && caData[reader["currency"].ToString()] - paidamt >= minCa)
                    {
                        ids.Add(reader["invno"].ToString());
                        caData[reader["currency"].ToString()] -= paidamt;
                    }   
                }

            });

            var keys = string.Join(",", ids);

            DBManager.RunQueryResults(string.Format("SELECT * FROM artran  WHERE invno IN ({0})", keys), (SqlDataReader reader) =>
            {
                invoices.Add(new Artran()
                {
                    invno = reader["invno"].ToString(),
                    cost = reader["cost"].ToString(),
                    fprice = reader["fprice"].ToString(),
                    invdate = reader["invdate"].ToString(),
                    itemcode = reader["itemcode"].ToString(),
                    qtyorder = reader["qtyorder"].ToString(),
                });
            });

            RemoveData(invoices, callbackProgress);
        }

        public static int GetNbData(DateTime from, DateTime to)
        {
            var count = 0;

            DBManager.RunQueryResults(string.Format("SELECT COUNT(*) total FROM arcash WHERE invdate BETWEEN '{0} 00:00:00' AND '{1} 23:59:59'", GetStrDateDbFormat(from), GetStrDateDbFormat(to)), (System.Data.SqlClient.SqlDataReader reader) =>
            {
                count = int.Parse(reader["total"].ToString());
            });

            return count;
        }

        private static int RemoveNbData(int nbData, DateTime from, DateTime to, RunAction callbackProgress = null)
        {
            var query = string.Format("SELECT * FROM artran where invno IN (SELECT TOP {0} invno FROM arcash WHERE invdate BETWEEN '{1} 00:00:00' AND '{2} 23:59:59' ORDER BY invdate DESC)", nbData, GetStrDateDbFormat(from), GetStrDateDbFormat(to));
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
            if (invoices.Count == 0) return 0;

            int fstNo = invoices.Min(i => i.GetInvnoToInt());

            foreach (var inv in invoices) Console.WriteLine(inv.invno);

            invoices = PrepareInvoiceNumbers(invoices, callbackProgress, false);

            var keys = string.Join(",", invoices.Select(invoice => invoice.invno));

            callbackProgress?.Invoke(new RunActionProgress() { Cursor = 0, NbData = keys.Length, Message = "Delete invoices" });

            if (SwitchDataFromTableToTable("artran", "artrand", keys))
            {
                SwitchDataFromTableToTable("arcash", "arcashd", keys);
                SwitchDataFromTableToTable("ictran", "ictrand", keys, "docno", new List<string>() { "trantype='I'" });
                SwitchDataFromTableToTable("armaster", "armasterd", keys);
            }

            callbackProgress?.Invoke(new RunActionProgress() { Cursor = keys.Length, NbData = keys.Length, Message = "Delete invoices" });
            
            ReloadStockFromInvoices(invoices, false, callbackProgress);

            ReloadInvoiceNumbersFrom(fstNo, callbackProgress);
            
            return invoices.Count;
        }

        private static bool SwitchDataFromTableToTable(string from, string to, string ids, string key = "invno", List<string> filters = null)
        {
            var success = true;

            if (IsTableHasIdentityInsert(to))
            {
                DBManager.ExecuteQuery(string.Format("SET IDENTITY_INSERT {0} ON;", to));
            }

            var cols = GetTableColumns(from);
            cols = cols.Equals(string.Empty) ? GetTableColumns(to) : cols;

            if (DBManager.ExecuteQuery(DBManager.WhereQuery(string.Format("INSERT INTO {0} ({1}) SELECT * FROM {2} WHERE {3} IN ({4})", to, cols, from, key, ids), filters)) <= 0)
            {
                success = false;
            }

            if (IsTableHasIdentityInsert(to))
            {
                DBManager.ExecuteQuery(string.Format("SET IDENTITY_INSERT {0} OFF;", to));
            }

            if(success) DBManager.ExecuteQuery(DBManager.WhereQuery(string.Format("DELETE FROM {0} WHERE {1} IN ({2})", from, key, ids), filters));

            return success;
        }

        private static void ReloadStockFromInvoices(List<Artran> invoices, bool rollback, RunAction callbackProgress = null)
        {
            
            var consumptions = GetItemConsumptions(invoices);
            var progress = new RunActionProgress() { Cursor = 0, NbData = 5, Message = "Reload Stock" };

            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("ALTER TABLE itemtrantmp ALTER COLUMN itemcode [nvarchar](25) COLLATE Croatian_BIN");
            DBManager.ExecuteQuery("DELETE FROM itemtrantmp;");

            StoreOrderedItems(consumptions, rollback);

            progress.Cursor = 1;
            callbackProgress?.Invoke(progress);

            string op = rollback ? "+" : "-";

            DBManager.ExecuteQuery(string.Format(@"UPDATE itemtrantmp SET 
                                     totalqtyorder=(SELECT SUM(artran.qtyorder) FROM artran where artran.itemcode=itemtrantmp.itemcode), 
                                     lastsale=(select MAX(artran.invdate) FROM artran where artran.itemcode=itemtrantmp.itemcode), 
                                     cost=(SELECT SUM(artran.cost * artran.qtyorder) FROM artran where artran.itemcode=itemtrantmp.itemcode), 
                                     qtystock=(
                                    (
                                         SELECT SUM(stockqty)
                                         FROM ictran
                                         where ictran.itemcode=itemtrantmp.itemcode AND trantype='R'
                                    ) - 
                                    (
                                        SELECT (CASE WHEN SUM(artran.qtyorder) IS NOT NULL THEN SUM(artran.qtyorder) ELSE 0 END) 
	                                    FROM artran where artran.itemcode=itemtrantmp.itemcode)
                                    ) {0} itemtrantmp.qtyorder
                                    FROM itemtrantmp inner join artran on itemtrantmp.itemcode=artran.itemcode", op));

            /*if (rollback)
            {
                DBManager.ExecuteQuery("UPDATE ic SET ic.tranqty=(ic.tranqty + tmp.qtyorder), ic.stockqty=(ic.tranqty + tmp.qtyorder) FROM ictran ic inner join itemtrantmp tmp on tmp.itemcode=ic.itemcode WHERE trantype='R' AND tranno=(SELECT MAX(tranno) FROM ictran ic2 where ic2.itemcode=tmp.itemcode AND trantype='R')");
            }
            else
            {

                /*double tranqty;
                List<string> queries = new List<string>();
                Dictionary<string, int> stocks = new Dictionary<string, int>();

                DBManager.ExecuteQuery("UPDATE ic SET ic.tranqty=(CASE WHEN (ic.tranqty - tmp.qtyorder > 0) THEN (ic.tranqty - tmp.qtyorder) ELSE 0 END), ic.stockqty=(CASE WHEN (ic.tranqty - tmp.qtyorder > 0) THEN (ic.tranqty - tmp.qtyorder) ELSE 0 END) FROM ictran ic inner join itemtrantmp tmp on tmp.itemcode=ic.itemcode WHERE trantype='R' AND tranno=(SELECT MAX(tranno) FROM ictran ic2 where ic2.itemcode=tmp.itemcode AND trantype='R') AND (SELECT COUNT(*) FROM ictran ic3 where ic3.itemcode=tmp.itemcode AND trantype='R') <= 1;");

                DBManager.RunQueryResults("SELECT ic.*, (SELECT COUNT(*) FROM ictran ic2 WHERE ic2.itemcode = ic.itemcode AND ic2.trantype = 'R') total FROM ictran ic WHERE ic.trantype='R' AND ic.itemcode IN (SELECT itemcode FROM itemtrantmp) AND (SELECT COUNT(*) FROM ictran ic3 WHERE ic3.itemcode = ic.itemcode AND ic3.trantype = 'R') > 1 ORDER BY itemcode ASC, trandate DESC", (SqlDataReader r) =>
                {
                    int total = 0;
                    int.TryParse(r["total"].ToString(), out total);

                    if (!stocks.ContainsKey(r["itemcode"].ToString())) stocks.Add(r["itemcode"].ToString(), total);

                    if (consumptions.ContainsKey(r["itemcode"].ToString()) && consumptions[r["itemcode"].ToString()] > 0 && double.TryParse(r["tranqty"].ToString(), out tranqty))
                    {
                        if (tranqty > consumptions[r["itemcode"].ToString()])
                        {
                            var totalRemaining = tranqty - consumptions[r["itemcode"].ToString()];

                            queries.Add(string.Format("UPDATE ictran SET tranqty={0}, stockqty={0} WHERE tranno={1}", totalRemaining, r["tranno"].ToString()));

                            consumptions[r["itemcode"].ToString()] = 0;
                        }
                        else
                        {
                            if (stocks[r["itemcode"].ToString()] > 1)
                            {
                                queries.Add(string.Format("DELETE FROM ictran WHERE tranno={0}", r["tranno"].ToString()));
                                stocks[r["itemcode"].ToString()]--;
                                consumptions[r["itemcode"].ToString()] -= tranqty;
                            }
                            else
                            {
                                queries.Add(string.Format("UPDATE ictran SET tranqty=0, stockqty=0 WHERE tranno={0}", r["tranno"].ToString()));
                                consumptions[r["itemcode"].ToString()] = 0;
                            }
                        }
                    }
                });

                DBManager.ExecuteQueries(queries);
            }*/

            DBManager.ExecuteQuery("UPDATE icitemlocation SET ytdsaleqty=itemtrantmp.totalqtyorder, lastreceipt=(SELECT MAX(trandate) FROM ictran WHERE itemcode=icitemlocation.itemcode AND trantype='R'), ptdsaleqty=itemtrantmp.totalqtyorder, ptdsalevalue=itemtrantmp.cost, ytdsalevalue=itemtrantmp.cost, lastsale=itemtrantmp.lastsale, edtdatetime=itemtrantmp.lastsale FROM icitemlocation inner join itemtrantmp ON icitemlocation.itemcode=itemtrantmp.itemcode;");
            progress.Cursor = 2;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("UPDATE icitemqty SET issuedate=itemtrantmp.lastsale, edtdatetime=itemtrantmp.lastsale, salesynqty=itemtrantmp.totalqtyorder FROM icitemqty inner join itemtrantmp on itemtrantmp.itemcode=icitemqty.itemcode;");
            progress.Cursor = 3;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("UPDATE icitemmaster SET ytdsaleqty=itemtrantmp.totalqtyorder, lastreceipt=(SELECT MAX(trandate) FROM ictran WHERE itemcode=icitemmaster.itemcode AND trantype='R'), ptdsaleqty=itemtrantmp.totalqtyorder, ytdsalevalue=itemtrantmp.cost, lastsale=itemtrantmp.lastsale FROM icitemmaster inner join itemtrantmp on itemtrantmp.itemcode=icitemmaster.itemcode;");
            progress.Cursor = 4;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("ALTER TABLE itemtrantmp ALTER COLUMN itemcode [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS");
            progress.Cursor = 5;
            callbackProgress?.Invoke(progress);

            //DBManager.ExecuteQuery("UPDATE iccost SET orgqty=itemtrantmp.qtystock FROM iccost inner join itemtrantmp on itemtrantmp.itemcode=iccost.itemcode;");
            //progress.Cursor = 6;
            //callbackProgress?.Invoke(progress);
        }

        private static void StoreOrderedItems(Dictionary<string, double> productsConsumptions, bool rollback)
        {
            var queries = DBManager.ExplodeQueryInMany("INSERT INTO itemtrantmp(itemcode, qtyorder, is_rollback) VALUES ", productsConsumptions, rollback ? "1" : "0");

            DBManager.ExecuteQueries(queries);

            //int i = 0;
            //var query = ;
            //
            //foreach (var itemcode in productsConsumptions.Keys)
            //{
            //    if (i++ > 0)
            //    {
            //        query += ",";
            //    }
            //
            //    query += string.Format("('{0}', {1})", itemcode, productsConsumptions[itemcode]);
            //}
            //
            //DBManager.ExecuteQuery(query);
        }

        private static Dictionary<string, double> GetItemConsumptions(List<Artran> invoices)
        {
            var productsConsumptions = new Dictionary<string, double>();
            double qtyOrdered = 0;

            foreach (var invoice in invoices)
            {
                if (!productsConsumptions.ContainsKey(invoice.itemcode) && invoice.itemcode != null) productsConsumptions[invoice.itemcode] = 0;

                if (double.TryParse(invoice.qtyorder, out qtyOrdered))
                {
                    productsConsumptions[invoice.itemcode] += qtyOrdered;
                }
            }

            return productsConsumptions;
        }
        
        private static void ReloadInvoiceNumbersFrom(int fromNo, RunAction callbackProgress)
        {
            var progress = new RunActionProgress() { Cursor = 0, NbData = 5, Message = "Reset invoice numbering" };

            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("TRUNCATE TABLE artrantmp");
            
            DBManager.ExecuteQuery(string.Format("DBCC CHECKIDENT (artrantmp, RESEED, {0})", (fromNo)));

            DBManager.ExecuteQuery(string.Format("INSERT INTO artrantmp(invno) SELECT -invno from arcash WHERE invno >={0} GROUP BY -invno order by MAX(invdate)", fromNo));
            progress.Cursor = 1;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("update ar SET ar.invno=(-ar.invno) FROM artran ar where invno in (select (invno * -1) FROM artrantmp)");
            DBManager.ExecuteQuery("update ar SET ar.invno=artrantmp.id from artran ar inner join artrantmp on ar.invno=artrantmp.invno");
            progress.Cursor = 2;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("update ar SET ar.invno=-ar.invno FROM arcash ar where invno in (select (invno * - 1) FROM artrantmp)");
            DBManager.ExecuteQuery("update ar SET ar.invno=artrantmp.id,  ponum='Payment on invoice ' + CAST(artrantmp.id AS VARCHAR), octn='R' + CAST(artrantmp.id AS VARCHAR), applyto='l' + CAST(artrantmp.id AS VARCHAR), ar.receiptno=(artrantmp.id + 1) from arcash ar inner join artrantmp on ar.invno=artrantmp.invno");
            progress.Cursor = 3;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("update ic SET ic.docno=-ic.docno FROM ictran ic WHERE docno IN (SELECT (artrantmp.invno * -1) FROM artrantmp)");
            DBManager.ExecuteQuery("update ic SET ic.docno=artrantmp.id, reference='Invoice ' + CAST(artrantmp.id AS VARCHAR) from ictran ic inner join artrantmp on ic.docno=artrantmp.invno WHERE ic.trantype='l'");

            progress.Cursor = 4;
            callbackProgress?.Invoke(progress);

            DBManager.ExecuteQuery("update ar SET ar.invno=-ar.invno from armaster ar WHERE ar.invno IN(SELECT (artrantmp.invno * -1) FROM artrantmp)");
            DBManager.ExecuteQuery("update ar SET ar.invno=artrantmp.id, octn='l' + CAST(artrantmp.id AS VARCHAR) from armaster ar inner join artrantmp on ar.invno=artrantmp.invno");

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

            //DBManager.ExecuteQuery(string.Format("update arshiftclose SET invcount={0}, totalvoucher={0}, totalamt={1};", total, ca));
            DBManager.ExecuteQuery(string.Format("update sysdata SET int1={0}, int2={1}, int3={1} WHERE sysid='AR';", total, total + 1));
            DBManager.ExecuteQuery(string.Format("update sysdata SET int1={0} WHERE sysid='GL';", total + 1));
            
            progress.Cursor = 5;
            callbackProgress?.Invoke(progress);
        }

        private static List<Artran> PrepareInvoiceNumbers(List<Artran> invoices, RunAction callbackProgress, bool rollback = true)
        {
            
            callbackProgress?.Invoke(new RunActionProgress() { Cursor = 0, NbData = invoices.Count, Message = "Prepare invoices" });
            
            Dictionary<string, string> numbers = new Dictionary<string, string>();

            int i = 0,
                maxArtrand = GetMaxInvno("artrand"),
                maxArtran = GetMaxInvno();

            int max = maxArtran > maxArtrand ? maxArtran : maxArtrand;

            Dictionary<int, int> idSwitch = new Dictionary<int, int>();

            foreach (var invoice in invoices)
            {
                int no = 0;

                if (int.TryParse(invoice.invno, out no))
                {
                    int newNo = 0;

                    if (idSwitch.ContainsKey(no))
                    {
                        newNo = idSwitch[no];
                    }
                    else
                    {
                        i++;
                        newNo = max + i;
                        idSwitch.Add(no, newNo);
                    }

                    invoice.invno = newNo.ToString();
                }
            }

            SetRemovedInvoiceNumber(idSwitch, rollback);

            callbackProgress?.Invoke(new RunActionProgress() { Cursor = i, NbData = invoices.Count, Message = "Prepare invoices" });

            return invoices;
        }

        private static void SetRemovedInvoiceNumber(Dictionary<int, int> ids, bool rollback)
        {
            var tableSuffix = rollback ? "d" : string.Empty;

            var queries = DBManager.ExplodeQueryInMany("INSERT INTO invnotmp VALUES", ids);

            DBManager.ExecuteQueries(queries);

            // var query = string.Empty;
            // var i = 0;
            // bool newQuery = true;
            //
            // foreach (var id in ids.Keys)
            // {
            //     i++;
            //
            //     if (newQuery)
            //     {
            //         query = ;
            //         newQuery = false;
            //     }
            //     else if (i > 1)
            //     {
            //         query += ",";
            //     }
            //     
            //     query += string.Format("({0}, {1})", id, ids[id]);
            //
            //     if (i % 1000 == 0 || i == ids.Keys.Count) {
            //         newQuery = true;
            //         queries.Add(query);
            //     }
            // }
            //
            // foreach (string q in queries)
            // {
            //     DBManager.ExecuteQuery(q);
            // }
            
            DBManager.ExecuteQuery(string.Format("update ar SET ar.invno=invnotmp.new_invno FROM artran{0} ar inner join invnotmp ON ar.invno = invnotmp.invno", tableSuffix));
            DBManager.ExecuteQuery(string.Format("update ar SET ar.invno=invnotmp.new_invno, ponum='Payment on invoice ' + CAST(invnotmp.new_invno AS VARCHAR), octn='R' + CAST(invnotmp.new_invno AS VARCHAR), applyto='l' + CAST(invnotmp.new_invno AS VARCHAR) FROM arcash{0} ar inner join invnotmp ON ar.invno = invnotmp.invno", tableSuffix));
            DBManager.ExecuteQuery(string.Format("update ic SET docno=invnotmp.new_invno, reference='Invoice ' + CAST(invnotmp.new_invno AS VARCHAR) FROM ictran{0} ic inner join invnotmp ON ic.docno = invnotmp.invno ", tableSuffix));
            DBManager.ExecuteQuery(string.Format("update ar SET ar.invno=invnotmp.new_invno,  octn='l' + CAST(invnotmp.new_invno AS VARCHAR) FROM armaster{0} ar inner join invnotmp ON ar.invno = invnotmp.invno", tableSuffix));

            DBManager.ExecuteQuery("DELETE FROM invnotmp;");
        }

        private static int GetMaxInvno(string table = "artran")
        {
            int max = 1;

            DBManager.RunQueryResults(string.Format("SELECT MAX(invno) invno FROM {0}", table), (SqlDataReader r) =>
            {
                int.TryParse(r["invno"].ToString(), out max);
            });

            return max;
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

        private static bool IsTableHasIdentityInsert(string table) {
            return table == "artran" || table == "ictran";
        }
    }
}
