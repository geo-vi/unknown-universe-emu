using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.WebSocks.objects;
using Newtonsoft.Json;

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
            int tries = 0;
            TRY:
            try
            {
                SqlDatabaseManager.Initialize();
                SqlDatabaseManager.Log.Write("Successfully connected to database");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("MYSQL Connection failed.");
                SqlDatabaseManager.Log.Write("MySQL Connection failed");
                new ExceptionLog("mysql", "MYSQL Connection failed", e);
                if (tries < 6)
                {
                    Console.WriteLine("Trying to reconnect in .. " + tries + " seconds.");
                    Thread.Sleep(tries * 1000);
                    tries++;
                    goto TRY;
                }
            }
            return false;
        }

        public void LoadClans()
        {
            Global.StorageManager.Clans.Add(0, new Clan(0, "", "",0));
            //Global.StorageManager.Clans.Add(1, new Clan(1, "Administrators", "ADM",0));
            //Global.StorageManager.Clans.Add(2, new Clan(2, "Developers", "DEV",0));
            foreach (var clan in Global.StorageManager.Clans)
                clan.Value.LoadDiplomacy();
        }

        public void SaveAll()
        {

        }

        public List<Server> GetServers()
        {
            List<Server> servers = new List<Server>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetGlobalClient())
                {
                    var queryTable =
                        mySqlClient.ExecuteQueryTable(
                            $"SELECT * FROM server_infos");

                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = Convert.ToInt32(row["ID"].ToString());
                        var region = row["REGION"].ToString();
                        var shortcut = row["SHORTCUT"].ToString();
                        var name = row["NAME"].ToString();
                        var ip = row["SERVER_IP"].ToString();
                        var open = Convert.ToBoolean(Convert.ToInt32(row["OPEN"]));
                        servers.Add(new Server{Id = id, Ip = ip, Name = name, Open = open, Region = region, Shortcut = shortcut});
                    }
                    mySqlClient.Dispose();
                }
            }
            catch (Exception)
            {

            }
            return servers;
        }

        public User GetUser(int id)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow($"SELECT PLAYER_NAME, RANK FROM player_data WHERE PLAYER_ID={id}");
                    var user = new User
                    {
                        Admin = Convert.ToInt32(queryRow["RANK"]) == 21,
                        Id = id,
                        Name = queryRow["NAME"].ToString()
                    };
                    mySqlClient.Dispose();
                    return user;
                }
            }
            catch (Exception)
            {
                
            }
            return null;
        }
    }
}
