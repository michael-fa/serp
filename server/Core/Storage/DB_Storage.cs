using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace server.Core.Storage
{
    public class DatabaseConnection
    {
        public static MySqlConnection conn = null;
        public int Connect_ToDB(string host, string Database, string username, string password, int port)
        {
            String connString = "Server=" + host + "; Database=" + Database + "; Port=" + port + "; Uid=" + username + ";";

            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();
                utils.Log.WriteLine(conn.ConnectionString);
                utils.Log.WriteLine("[MYSQL] Connected to a new Database (" + host + "." + Database + ")");
            }
            catch (Exception ex)
            {
                //handle error
                utils.Log.WriteLine("[MYSQL] Error: \n" + ex.Message + "\n" + ex.Source, System.Drawing.Color.Red);
            }
            return 1;
        }

        /**
            * Description: Inserting into a MySQLTable. 
            * 
            */
        public static void Insert(string table, string field, string values, string where = "")
        {
            var cmd = new MySqlCommand();
            if (String.IsNullOrEmpty(where)) cmd.CommandText = "INSERT INTO " + table + "(" + field + ") VALUES(" + values + ")";
            else cmd.CommandText = "INSERT INTO " + table + "(" + field + ") VALUES(" + values + ")" + "WHERE" + where;

            utils.Log.WriteLine(cmd.CommandText);
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            return;
        }

        public static void Update(string table, string field, object value, string where = "")
        {
            var cmd = new MySqlCommand();


            if (String.IsNullOrEmpty(where)) cmd.CommandText = "UPDATE " + table + " SET " + field + " = " + value;
            else cmd.CommandText = "UPDATE " + table + " SET " + field + " = " + value + " WHERE " + where;

            utils.Log.WriteLine(cmd.CommandText);
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            return;
        }

        public static void SendCommand(string cmdText)
        {
            var cmd = new MySqlCommand();


            if (String.IsNullOrEmpty(cmdText)) return;

            cmd.CommandText = cmdText;

            utils.Log.WriteLine(cmd.CommandText);
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }
    }
}
