using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.objects.chat.players;
using NettyBaseReloaded.Chat.objects.chat.rooms;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world.players.informations;
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
            LoadModerators();
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
            Chat.StorageManager.Rooms.Add(0, new GlobalChatRoom(0));
            Chat.StorageManager.Rooms.Add(1, new Room(1, "FR", 1, ChatRoomTypes.NORMAL_ROOM, 250, false, "fr"));
            Chat.StorageManager.Rooms.Add(2, new Room(2, "TR", 2, ChatRoomTypes.NORMAL_ROOM, 250, false, "tr"));
            Chat.StorageManager.Rooms.Add(3, new Room(3, "FIN", 3, ChatRoomTypes.NORMAL_ROOM, 250, false, "fin"));
            Chat.StorageManager.Rooms.Add(4, new Room(4, "DE", 4, ChatRoomTypes.NORMAL_ROOM, 250, false, "de"));
        }

        public void LoadModerators()
        {
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable($"SELECT * FROM server_chat_moderators");
                    foreach (DataRow queryRow in queryTable.Rows)
                    {
                        var id = intConv(queryRow["PLAYER_ID"]);
                        //Informations
                        string name = queryRow["NAME"].ToString();
                        var lvl = (ModeratorLevelTypes)(intConv(queryRow["LEVEL"]));
                        ////add to Storage
                        var mod = new Moderator(
                            id,
                            name,
                            "",
                            Main.Global.StorageManager.Clans[0],
                            lvl
                        );

                        Chat.StorageManager.ChatModerators.Add(id, mod);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public Player LoadPlayer(int id)
        {
            Player player = null;
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT * FROM player_data WHERE player_data.PLAYER_ID=" + id);
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

        public Dictionary<int, ChatIssue> LoadChatIssues(Player player)
        {
            var issues = new Dictionary<int, ChatIssue>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM player_chat_issues WHERE PLAYER_ID=" + player.Id);
                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = intConv(row["ID"]);
                        var issueType = (ChatIssueTypes) intConv(row["ISSUE_TYPE"]);
                        var issuedTime = DateTime.Parse(row["ISSUED_AT"].ToString());
                        var reason = row["REASON"].ToString();
                        var expireTime = DateTime.Parse(row["EXPIRY"].ToString());
                        var issuedBy = intConv(row["ISSUED_BY"]);
                        var chatIssue = new ChatIssue(id, issueType, issuedBy, issuedTime, expireTime, reason);
                        issues.Add(id, chatIssue);
                    }
                }
            }
            catch (Exception e)
            {
                Out.QuickLog("load Issue with chat");
                Out.QuickLog(e);
            }
            return issues;
        }

        public void AddChatIssue(Player player, ChatIssue issue)
        {
            try
            {
                var userId = 0;
                userId = player.GetSession().GetEquivilentGameSession().Player.GlobalId;
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"INSERT INTO player_chat_issues (USER_ID, PLAYER_ID, ISSUE_TYPE, ISSUED_BY, ISSUED_AT, EXPIRY, REASON) VALUES ('{userId}', '{player.Id}', '{Convert.ToInt32(issue.IssueType)}', '{issue.IssuedBy}', '{issue.IssuedAt:yyyy-MM-dd H:mm:ss}', '{issue.Expiry:yyyy-MM-dd H:mm:ss}', '{issue.Reason}')");
                    player.Issues = LoadChatIssues(player);
                }
            }
            catch (Exception e)
            {
                Out.QuickLog("add Issue with chat exception");
                Out.QuickLog(e);
            }
        }
    }
}