using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.objects.chat.players;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.objects
{
    class ChatSession
    {
        public ChatClient Client { get; set; }

        public Player Player { get; set; }

        public ChatSession(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Gets the GameSession of the player
        /// </summary>
        /// <returns></returns>
        public GameSession GetEquivilentGameSession()
        {
            var id = Player.Id;
            var sessionId = Player.SessionId;
            var worldSession = World.StorageManager.GetGameSession(id);
            if (worldSession != null && worldSession.Player.Id == id && worldSession.Player.SessionId == sessionId)
            {
                return worldSession;
            }

            return null;
        }

        public void Ban(string reason, DateTime expiry, int issuedBy = 0)
        {
            var issue = new ChatIssue(-1, ChatIssueTypes.BAN, issuedBy, DateTime.Now, DateTime.Now, reason);
            MessageController.System(Player, "You've been banned by " + issue.GetIssuer().Name + " for " + reason + "\nExpires at: " + expiry);
            Chat.DatabaseManager.AddChatIssue(Player, issue);
        }

        public void Mute(string reason, DateTime expiry, int issuedBy = 0)
        {
            var issue = new ChatIssue(-1, ChatIssueTypes.MUTE, issuedBy, DateTime.Now, expiry, reason);
            MessageController.System(Player, "You've been muted by " + issue.GetIssuer().Name + " for " + reason + "\nExpires at: " + expiry);
            Chat.DatabaseManager.AddChatIssue(Player, issue);
        }

        public void Kick(string reason, int issuedBy = 0)
        {
            var issue = new ChatIssue(-1, ChatIssueTypes.KICK, issuedBy, DateTime.Now, DateTime.Now, reason);
            MessageController.System(Player, "You've been kicked by " + issue.GetIssuer().Name + " for " + reason);
            Chat.DatabaseManager.AddChatIssue(Player, issue);
            Close();
        }

        public void Close()
        {
            Player.DisconnectRooms();
            Client.Disconnect();
        }
    }
}
