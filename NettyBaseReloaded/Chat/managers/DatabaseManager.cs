using System;
using System.Data;
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
                new ExceptionLog("dbmanager", "Failed to load chat moderators...", e);
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
            catch (Exception e)
            {
            }
            return mod;
        }

        public Character LoadCharacter(int id)
        {
            Character character = null;
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    //mySqlClient.ExecuteQueryRow()
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Failed to load character...", e);
            }
            return character;
        }
    }
}