using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.global_managers
{
    static class SqlDatabaseManager
    {
        public static DebugLog Log = new DebugLog("mysql");

        public static string SERVER = "213.32.95.48";
        public static string UID = "remote";
        public static string PWD = "Fuckuberorbit";
        public static string DB = "do_server_ge1";
        public static string DB_EXT = "do_system";

        public static void Initialize()
        {
            GenerateConnectionString();
            GetClient().ExecuteNonQuery("SELECT 1");
        }

        public static SqlDatabaseClient GetClient()
        {
            MySqlConnection Connection = new MySqlConnection(GenerateConnectionString());
            Connection.Open();

            return new SqlDatabaseClient(Connection);
        }

        public static SqlDatabaseClient GetGlobalClient()
        {
            MySqlConnection Connection = new MySqlConnection(GenerateGlobalConnectionString());
            Connection.Open();
            return new SqlDatabaseClient(Connection);
        }

        public static string GenerateGlobalConnectionString()
        {
            if (GlobalConnectionString == "")
            {
                MySqlConnectionStringBuilder ConnectionStringBuilder = new MySqlConnectionStringBuilder();
                ConnectionStringBuilder.Server = SERVER;
                ConnectionStringBuilder.Port = 3306;
                ConnectionStringBuilder.UserID = UID;
                ConnectionStringBuilder.Password = PWD;
                ConnectionStringBuilder.Database = DB_EXT;
                ConnectionStringBuilder.MinimumPoolSize = 10; //I've been using 10-100, but you can play with them
                ConnectionStringBuilder.MaximumPoolSize = 100;
                ConnectionStringBuilder.Pooling = true;
                GlobalConnectionString = ConnectionStringBuilder.ToString();
            }
            return GlobalConnectionString;
        }

        public static string GlobalConnectionString = "";

        public static string GenerateConnectionString()
        {
            if (ConnectionString == "")
            {
                MySqlConnectionStringBuilder ConnectionStringBuilder = new MySqlConnectionStringBuilder();
                ConnectionStringBuilder.Server = SERVER;
                ConnectionStringBuilder.Port = 3306;
                ConnectionStringBuilder.UserID = UID;
                ConnectionStringBuilder.Password = PWD;
                ConnectionStringBuilder.Database = DB;
                ConnectionStringBuilder.MinimumPoolSize = 10; //I've been using 10-100, but you can play with them
                ConnectionStringBuilder.MaximumPoolSize = 100;
                ConnectionStringBuilder.Pooling = true;
                ConnectionString = ConnectionStringBuilder.ToString();
            }
            return ConnectionString;
        }

        public static string ConnectionString = "";

    }
}
