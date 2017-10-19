using accouting_system_manager.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accouting_system_manager.DB
{
    public static class DBManager
    {
        private static SqlConnection connection;

        public delegate void QueryResultCallback(SqlDataReader r);

        public delegate void QueryExecResultCallback(bool success = true);

        public delegate void AddFieldToInsert(dynamic el);


        public static bool IsConnectionOpen()
        {
            return connection == null ? false : connection.State == ConnectionState.Open;
        }

        public static bool Init()
        {
            if (Connect(Properties.Settings.Default.SERVER_NAME, Properties.Settings.Default.DB_NAME, Properties.Settings.Default.DB_USER, Properties.Settings.Default.DB_PASSWORD))
            {
                PrepareDB();

                return true;
            }

            return false;
        }

        public static bool Connect(string serverName, string dbName, string userName, string userPassword)
        {
            var connectionStr = string.Format("Data Source={0};Initial Catalog={1};", serverName, dbName);

            if (serverName == string.Empty || dbName == string.Empty) return false;

            if (userName == null || userName == string.Empty)
            {
                connectionStr += "persist security info=False;integrated security=SSPI;";
            }
            else
            {
                connectionStr += string.Format("User ID={0};Password={1}", userName, userPassword);
            }

            try
            {
                Connect(connectionStr);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Connect(string connectionString) {

            connection = new SqlConnection(connectionString);

            connection.Open();
        }

        public static int ExecuteQuery(string q, bool withErrors = false) {
            SqlCommand command = GetCommand(q);

            try
            {
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LoggerSingleton.GetInstance.Error(string.Format("query: {0}, message:{1}", q, e.Message));

                if (withErrors) throw e;

                return -1;
            }

        }

        public static void ExecuteQueries(List<string> queries, QueryExecResultCallback callback = null)
        {
            foreach (string q in queries)
            {
                bool success = ExecuteQuery(q) > 0;

                callback?.Invoke(success);
            }
        }

        public static void RunQueryResults(string query, QueryResultCallback callbk)
        {
            var cmd = GetCommand(query);

            SqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    callbk(reader);
                }

                reader.Close();
            }
            catch(Exception e)
            {
                LoggerSingleton.GetInstance.Error(string.Format("query:{0}, message:{1}", query, e.Message));
                reader?.Close();
            }
        }
        

        public static List<string> ExplodeQueryInMany<T1, T2>(string baseQuery, Dictionary<T1, T2> elements, params string[] otherFields)
        {
            var query = string.Empty;
            var queries = new List<string>();
            var i = 0;
            bool newQuery = true;

            foreach (dynamic el in elements.Keys)
            {
                i++;

                if (newQuery)
                {
                    query = baseQuery;
                    newQuery = false;
                }
                else if (i > 1)
                {
                    query += ",";
                }

                if (el is string)
                {
                    query += string.Format("('{0}', {1}", el, elements[el]);
                }
                else
                {
                    query += string.Format("({0}, {1}", el, elements[el]);
                }

                foreach (var otherVal in otherFields) {
                    query += ", " + otherVal;
                }

                query += ")";

                if (i % 1000 == 0 || i == elements.Count)
                {
                    newQuery = true;
                    queries.Add(query);
                }
            }

            return queries;
        }

        public static bool Backup(string dbName, string path) {
            
            var query = string.Format("BACKUP DATABASE [{0}] TO DISK='{1}'", dbName, path);

            try
            {
                ExecuteQuery(query, true);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string WhereQuery(string q, List<string> filters)
        {
            if (filters == null || filters.Count == 0) return q;

            var finalQ = q;
            bool hasWhere = false;

            if (!finalQ.ToLower().Contains("where")) finalQ += " WHERE";
            else hasWhere = true;

            foreach (var filter in filters) {
                finalQ += (hasWhere ? " AND " : " ") +  filter + " ";

                hasWhere = true;
            }

            return finalQ;
        }

        private static SqlCommand GetCommand(string q)
        {
            CheckConnection();

            var command = new SqlCommand(q, connection);
            command.CommandTimeout = 0;

            return command;
        }

        private static void PrepareDB()
        {
            ExecuteQuery(@" ");
            try
            {
                GetCommand(string.Format("select TOP 1 invno from arcashd")).ExecuteReader().Close();
            }
            catch
            {
                CreateTables();
            }
        }
        
        private static void CreateTables()
        {

            CreateTableFrom("arcash", "arcashd");
            CreateTableFrom("artran", "artrand");
            CreateTableFrom("ictran", "ictrand");
            CreateTableFrom("armaster", "armasterd");

            try
            {
                GetCommand("select top 1 * from artrantmp").ExecuteReader().Close();
            }
            catch
            {
                ExecuteQuery("CREATE TABLE artrantmp(id [decimal](18, 0) IDENTITY(1,1) NOT NULL, invno [bigint] NULL);");
            }

            try
            {
                GetCommand("select top 1 * from invnotmp").ExecuteReader().Close();
            }
            catch
            {
                ExecuteQuery("CREATE TABLE invnotmp(invno [bigint] NULL, new_invno [bigint] NULL);");
            }

            try
            {
                GetCommand("select top 1 * from itemtrantmp").ExecuteReader().Close();
            }
            catch
            {
                ExecuteQuery(@"SELECT SERVERPROPERTY ('Collation'); 
                               CREATE TABLE itemtrantmp(itemcode [nvarchar](25) COLLATE Croatian_BIN, 
                                                        qtyorder [decimal](18, 6),  
                                                        qtystock [decimal](18, 6), 
                                                        totalqtyorder [decimal](18, 6) NULL, 
                                                        lastsale [datetime] NULL, 
                                                        cost [decimal](18, 6) NULL,
                                                        is_rollback integer NULL);");

                ExecuteQuery("CREATE TABLE logs(id [decimal](18, 0) IDENTITY(1,1) NOT NULL, message varchar(255));");

                ExecuteQuery(Properties.Resources.stored_proc_reloadstock);
                ExecuteQuery(Properties.Resources.trigger_itemtrantmp);
            }

            ExecuteQuery("CREATE TABLE artrandate([invno] [bigint] NULL, [createdat][datetime] NULL default CURRENT_TIMESTAMP);");
            ExecuteQuery(Properties.Resources.trigger_arcashd);
        }

        private static bool CreateTableFrom(string fromTable, string tableName)
        {
            try
            {
                GetCommand(string.Format("select TOP 1 * from {0}", tableName)).ExecuteReader().Close();
            }
            catch
            {
                try
                {
                    CopyTableInto(fromTable, tableName);
                }
                catch(Exception e)
                {
                    LoggerSingleton.GetInstance.Error(e.Message);

                    connection.Close();

                    return false;
                }
            }

            return true;
        }

        private static void CheckConnection()
        {
            if (!(connection is SqlConnection)) {
                Init();
            }

            if (connection != null && connection?.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private static void CopyTableInto(string fromTable, string toTable)
        {
            var cmd = GetCommand(string.Format("SELECT " +
                                "c.name 'name', " +
                                "t.Name 'type', " +
                                "c.max_length 'max', " +
                                "c.precision, " +
                                "c.scale, " +
                                "c.is_nullable, " +
                                "ISNULL(i.is_primary_key, 0) 'Primary Key' " +
                                "FROM " +
                                "sys.columns c " +
                                "INNER JOIN " +
                                "sys.types t ON c.user_type_id = t.user_type_id " +
                                "LEFT OUTER JOIN " +
                                "sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id " +
                                "LEFT OUTER JOIN " +
                                "sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id " +
                                "WHERE " +
                                "c.object_id = OBJECT_ID('{0}')", fromTable));

            string fields = string.Empty;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                var i = 0;
                List<string> columns = new List<string>();

                // while there is another record present
                while (reader.Read())
                {
                    if (!columns.Contains(reader["name"].ToString()))
                    {
                        if (i++ > 0)
                        {
                            fields += ", ";
                        }

                        fields += GetFieldInfoForCreate(reader);

                        columns.Add(reader["name"].ToString());
                    }
                    
                }
            }

            ExecuteQuery(string.Format("CREATE TABLE {0} ({1})", toTable, fields));
        }

        private static string GetFieldInfoForCreate(SqlDataReader reader) {

            string field = string.Empty;

            int max, scale, precision;
            bool nullable;

            int.TryParse(reader["max"].ToString(), out max);
            int.TryParse(reader["scale"].ToString(), out scale);
            int.TryParse(reader["precision"].ToString(), out precision);
            bool.TryParse(reader["is_nullable"].ToString(), out nullable);
            
            switch ((string)reader["type"])
            {
                case "char":
                case "nchar":
                case "varchar":
                    field += string.Format("{0} {1}({2})", reader["name"], reader["type"], max > 0 ? max.ToString() : "MAX");
                    break;
                case "nvarchar":
                    field += string.Format("{0} {1}({2})", reader["name"], reader["type"], max > 0 ? (max / 2).ToString() : "MAX");
                    break;

                case "decimal":
                    field += string.Format("{0} {1}({2}, {3})", reader["name"], reader["type"], precision, scale);
                    break;
                default:
                    field += string.Format("{0} {1}", reader["name"], reader["type"]);
                    break;
            }

            field += nullable ? " NULL" : " NOT NULL";

            return field;
        }

    }
}
