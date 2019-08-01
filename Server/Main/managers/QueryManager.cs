using Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using Server.Game;
using Server.Game.objects.enums;
using Server.Main.objects;

namespace Server.Main.managers
{
    class QueryManager
    {
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

        public void Initiate()
        {
            LoadServerClans();
        }

        /// <summary>
        /// Loads the server's clans
        /// </summary>
        private void LoadServerClans()
        {
            Global.StorageManager.Clans.TryAdd(0, new Clan(0, "NO CLAN", "NOCLAN", Factions.NONE, ""));
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_clans");
                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = Convert.ToInt32(row["ID"]);
                        var name = row["NAME"].ToString();
                        var tag = row["TAG"].ToString();
                        var faction = (Factions) (Convert.ToInt32(row["FACTION"]));
                        var news = row["NEWS"].ToString();
                        
                        Global.StorageManager.Clans.TryAdd(id, new Clan(id, name, tag, faction, news));
                    }

                    queryTable.Dispose();

                    foreach (var clan in Global.StorageManager.Clans)
                    {
                        queryTable =
                            mySqlClient.ExecuteQueryTable(
                                "SELECT * FROM server_clans_members WHERE CLAN_ID=" + clan.Key);
                        foreach (DataRow row in queryTable.Rows)
                        {
                            var playerId = Convert.ToInt32(row["PLAYER_ID"]);
                            clan.Value.AssignedMemberIds.Add(playerId);
                        }
                    }

                }
                
                Out.QuickLog("Successfully " + Global.StorageManager.Clans.Count + " added clans", LogKeys.DATABASE_LOG);
            }
            catch (Exception)
            {
                
            }
        }


        public void UpdateClanMembers(Clan clan)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable =
                        mySqlClient.ExecuteQueryTable(
                            "SELECT * FROM server_clans_members WHERE CLAN_ID=" + clan.Id);
                    var clanMembers = new List<int>();
                    foreach (DataRow row in queryTable.Rows)
                    {
                        var playerId = Convert.ToInt32(row["PLAYER_ID"]);
                        clanMembers.Add(playerId);
                    }

                    if (!clanMembers.Equals(clan.AssignedMemberIds))
                    {
                        clan.AssignedMemberIds = clanMembers;
                    }
                }

                Out.QuickLog("Successfully updated clan members", LogKeys.DATABASE_LOG);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
