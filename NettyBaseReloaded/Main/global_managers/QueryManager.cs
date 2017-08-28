using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.global_managers
{
    class QueryManager
    {

        public void Load()
        {
            if (!CheckConnection())
            {
                throw new Exception("Couldn't establish MYSQL connection");
            }
            LoadClans();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>If connection is established</returns>
        public bool CheckConnection()
        {
            var mySql = new MySQLManager();
            var connection = mySql.Connection();
            connection.Open();
            var ping = connection.Ping();
            connection.Close();
            return ping;
        }

        public void LoadClans()
        {
            Global.StorageManager.Clans.Add(0, new Clan(0, "", ""));
        }

        public void SaveAll()
        {
            
        }
    }
}
