using MySql.Data.MySqlClient;
using Server.Main.objects;
using Server.Utils;

namespace Server.Main.managers
{
    static class SqlDatabaseManager
    {
        public static string SERVER = "127.0.0.1";
        public static string UID = "root";
        public static string PWD = "";
        public static string DB = "do_server_ge1";
        public static string DB_EXT = "do_system";

        /* test1 
        */

        public static void Initialize()
        {
            GenerateConnectionString();
            using (var client = GetClient())
            {
                client.ExecuteNonQuery("SELECT 1");
            }
        }

        //public static ConcurrentDictionary<int, MySqlConnection> Connections = new ConcurrentDictionary<int, MySqlConnection>();
        public static SqlDatabaseClient GetClient()
        {
            var Connection = new MySqlConnection(GenerateConnectionString());

            Connection.Open();
            //Connections.TryAdd(Connections.Count, Connection);
            Out.WriteDbLog("Client requested from " + Out.GetCaller());
            return new SqlDatabaseClient(Connection);

        }

        public static SqlDatabaseClient GetGlobalClient()
        {
            MySqlConnection Connection = new MySqlConnection(GenerateGlobalConnectionString());
            Connection.Open();
            //Connections.TryAdd(Connections.Count, Connection);
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
                ConnectionStringBuilder.ConvertZeroDateTime = true;
                ConnectionStringBuilder.Pooling = false;
                ConnectionStringBuilder.SslMode = MySqlSslMode.None;
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
                ConnectionStringBuilder.ConvertZeroDateTime = true;
                ConnectionStringBuilder.Pooling = true;
                ConnectionStringBuilder.MaximumPoolSize = 100;
                ConnectionStringBuilder.SslMode = MySqlSslMode.None;
                ConnectionString = ConnectionStringBuilder.ToString();
            }
            return ConnectionString;
        }

        public static string ConnectionString = "";

    }
}
