using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accouting_system_manager.DB
{
    public static class SqlDataReaderExtensions
    {
        public static string GetColumnsCommaSeparated(this SqlDataReader reader)
        {
            string cols = string.Empty;
            int count = reader.FieldCount;

            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    cols += ", ";
                }

                cols += reader.GetName(i);
            }

            return cols;
        }

        public static string GetValuesCommaSeparated(this SqlDataReader reader)
        {
            string values = string.Empty;

            int count = reader.FieldCount;
           
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    values += ", ";
                }

                string value = string.Empty;

                decimal myDec;

                if (string.IsNullOrEmpty(reader[i].ToString()))
                {
                    value = "NULL";
                }
                else if (decimal.TryParse(reader[i].ToString(), out myDec))
                {
                    value = reader[i].ToString();
                }
                else
                {
                    value = "'" + reader[i].ToString() + "'";
                }

                values += value;
            }

            return values;
        }
    }
}
