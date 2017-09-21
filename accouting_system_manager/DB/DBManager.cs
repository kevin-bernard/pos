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

        public static int ExecuteQuery(string q) {
            SqlCommand command = GetCommand(q);

            return command.ExecuteNonQuery();
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
                LoggerSingleton.GetInstance.Error(e.Message);
                reader?.Close();
            }
        }
        
        private static SqlCommand GetCommand(string q)
        {
            CheckConnection();

            return new SqlCommand(q, connection);
        }

        private static void PrepareDB()
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
