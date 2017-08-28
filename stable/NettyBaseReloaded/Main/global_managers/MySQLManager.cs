using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace NettyBaseReloaded.Main.global_managers
{
    class MySQLManager
    {
        public static string SERVER = "213.32.95.48";
        public static string UID = "remote";
        public static string PWD = "fuckuberorbit";
        public static string DB = "do_server_ge1";

        public MySqlConnection Connection()
        {
            return new MySqlConnection("server=" + SERVER + ";uid=" + UID + ";pwd=" + PWD + ";database=" + DB + ";");
        }

        /// <summary>
        /// Executes the passed MysqlCommand
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public MySqlDataReader Execute(MySqlCommand query)
        {
            try
            {
                var connection = Connection();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string queryType = query.CommandText.Split(' ')[0];

                    //Adds the connection to the command
                    query.Connection = connection;

                    switch (queryType)
                    {
                        case "SELECT":
                            MySqlDataAdapter adap = new MySqlDataAdapter(query);
                            DataSet set = new DataSet();

                            return query.ExecuteReader();

                        default:
                            query.ExecuteNonQuery();
                            break;
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Something went wrong executing the query (" + query.CommandText + ")", "ERROR",
                    ConsoleColor.Red);

                Debug.WriteLine(e.StackTrace, "Debug Error", ConsoleColor.Red);

                Out.WriteLine("Refreshing connection...");
            }

            return null;
        }
    }
}
