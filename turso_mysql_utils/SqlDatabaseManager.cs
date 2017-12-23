using System;

using MySql.Data.MySqlClient;

using OneUltimate.Utils.Config;

namespace OneUltimate.Utils.Storage
{
    public static class SqlDatabaseManager
    {
        public static void Initialize()
        {
            GenerateConnectionString();
        }

        public static SqlDatabaseClient GetClient()
        {
            MySqlConnection Connection = new MySqlConnection(GenerateConnectionString());
            Connection.Open();

            return new SqlDatabaseClient(Connection);
        }

        public static string GenerateConnectionString()
        {
            if (ConnectionString == "")
            {
                MySqlConnectionStringBuilder ConnectionStringBuilder = new MySqlConnectionStringBuilder();
                ConnectionStringBuilder.Server = "localhost";
                ConnectionStringBuilder.Port = 3306;
                ConnectionStringBuilder.UserID = "root";
                ConnectionStringBuilder.Password = "yourpass";
                ConnectionStringBuilder.Database = "dbname";
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
