using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using NettyBaseReloaded.Game;
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
                Out.WriteDbLog("Successfully connected to database");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("MySql Error: " + e.Message);
                Console.WriteLine("MYSQL Connection failed.");
                Out.WriteDbLog("MySQL Connection failed");
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
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable =
                        mySqlClient.ExecuteQueryTable(
                            $"SELECT * FROM server_clans");

                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = Convert.ToInt32(row["ID"].ToString());
                        var name = row["NAME"].ToString();
                        var tag = row["TAG"].ToString();
                        var rank = Convert.ToInt32(row["RANK"]);
                        Global.StorageManager.Clans.Add(id, new Clan(id, name, tag, rank));
                    }

                    Global.StorageManager.Clans.Add(0, new Clan(0, "", "", 0));

                    var reader  =
                        mySqlClient.ExecuteQueryReader(
                            $"SELECT player_data.PLAYER_ID, player_data.CLAN_ID, player_data.PLAYER_NAME FROM server_clans_members,player_data WHERE player_data.PLAYER_ID=server_clans_members.PLAYER_ID");
                    while (reader.Read())
                    {
                        var id = Convert.ToInt32(reader["PLAYER_ID"].ToString());
                        var clanId = Convert.ToInt32(reader["CLAN_ID"]);
                        
                        Global.StorageManager.Clans[clanId].Members.TryAdd(id, new ClanMember(id, reader["PLAYER_NAME"].ToString()));
                    }
                    reader.Close();

                    queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_clans_diplomacy");
                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = Convert.ToInt32(row["ID"]);
                        var clanId = Convert.ToInt32(row["CLAN_ID"].ToString());
                        var diplomacy = (Diplomacy)Convert.ToInt32(row["DIPLOMACY"].ToString());
                        var targetId = Convert.ToInt32(row["TARGET_CLAN_ID"].ToString());

                        var diplo = new ClanDiplomacy();
                        diplo.Diplomacy = diplomacy;
                        diplo.Clans.Add(Global.StorageManager.GetClan(clanId));
                        diplo.Clans.Add(Global.StorageManager.GetClan(targetId));

                        Global.StorageManager.ClanDiplomacys.Add(id, diplo);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SaveAll()
        {
            UpdateOnlinePlayers();
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
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                        Name = queryRow["PLAYER_NAME"].ToString()
                    };
                    
                    return user;
                }
            }
            catch (Exception)
            {
                
            }
            return null;
        }

        public Clan GetClan(int id)
        {
            RefreshClans();
            return Global.StorageManager.Clans[id];
        }

        public void UpdateOnlinePlayers()
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetGlobalClient())
                {
                    mySqlClient.ExecuteNonQuery("UPDATE server_infos SET ONLINE_PLAYERS=" + World.StorageManager.GameSessions.Count + " WHERE ID=" + Global.StorageManager.ServerId);
                }
            }
            catch (Exception)
            {

            }
        }

        public void RefreshClans()
        {
            Global.StorageManager.Clans.Clear();
            LoadClans();
        }
    }
}
