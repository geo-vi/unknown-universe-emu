using System;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.objects.chat.rooms;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Chat.managers
{
    class DatabaseManager : DBManagerUtils
    {
        public override void Initiate()
        {
            LoadAnnouncements();
            LoadLoginMsg();
            LoadRooms();
            LoadGlobalBans();
        }

        private void LoadAnnouncements()
        {
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_announcements");
                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            int id = intConv(reader["ID"]);
                            string text = reader["ANNOUNCEMENT"].ToString();

                            ////add to Storage
                            Chat.StorageManager.Announcements.Add(id, new Announcement(
                                id, text
                            ));
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed loading Chat Mods, " + e.Message);
            }
        }

        private void LoadLoginMsg()
        {
            //try
            //{
            //    using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
            //    {
            //        var queryRow = mySqlClient.ExecuteQueryRow("SELECT * FROM server_chat_login");
            //        if (queryRow != null)
            //        {

            //            Properties.Chat.USER_LOGIN_MSG = queryRow["USER_LOGIN_MSG"].ToString();
            //            Properties.Chat.MOD_LOGIN_MSG = queryRow["MODERATOR_LOGIN_MSG"].ToString();
            //        }

            //    }
            //}
            //catch (Exception e)
            //{
            //    new ExceptionLog("dbmanager", "Failed to load chat login messages...", e);
            //}
        }

        private void LoadRooms()
        {
            Chat.StorageManager.Rooms.Add(0, new Global(0));
        }

        private void LoadGlobalBans()
        {
        }

        public Moderator LoadModerator(int id)
        {
            Moderator mod = null;
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow($"SELECT * FROM server_chat_moderators WHERE PLAYER_ID={id}");
                    //Informations
                    string name = queryRow["NAME"].ToString();
                    Moderator.Level lvl = (Moderator.Level) intConv(queryRow["LEVEL"]);

                    ////add to Storage
                    mod = new Moderator(
                        id,
                        name,
                        "",
                        Main.Global.StorageManager.Clans[0],
                        lvl
                    );
                    
                }
            }
            catch (Exception)
            {
            }
            return mod;
        }

        public Player LoadPlayer(int id)
        {
            Player player = null;
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT * FROM player_data,server_chat_moderators WHERE player_data.PLAYER_ID=" + id);
                    string name = queryRow["PLAYER_NAME"].ToString();
                    string sessionId = queryRow["SESSION_ID"].ToString();
                    int clanId = intConv(queryRow["CLAN_ID"]);
                    var clan = Main.Global.StorageManager.GetClan(clanId);

                    player = new Player(id, name, sessionId, clan);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //new ExceptionLog("dbmanager", "Failed to load character...", e);
            }
            return player;
        }

        public void InsertChatLog(Character character, int roomId, string log, MessageType logType)
        {
            var playerId = 0;
            if (character != null) playerId = character.Id;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    MySqlCommand cmd =
                        new MySqlCommand(
                            $"INSERT INTO server_chat_logs (PLAYER_ID, ROOM_ID, LOG, LOG_TYPE, LOG_DATE) VALUES ('{playerId}', '{roomId}', @LOG, '{(int) logType}', NOW())",
                            mySqlClient.mConnection);
                    cmd.Parameters.AddWithValue("@LOG", log);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
            }
        }
    }
}