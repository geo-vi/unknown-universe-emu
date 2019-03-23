using System;
using System.Linq;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.objects.chat.players;
using NettyBaseReloaded.Chat.packet;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Chat.controllers
{
    class LoginController
    {
        public ChatSession ChatSession { get; }

        public LoginController(ChatSession chatSession)
        {
            ChatSession = chatSession;

            if (AllowedToEnter() || chatSession.Player is Moderator)
            {
                if (Properties.Server.LOCKED) Locked();
                else RegularLogin();
            }
        }

        public bool AllowedToEnter()
        {
            var id = ChatSession.Player.Id;
            return true;
        }

        public void Locked()
        {
            throw new NotImplementedException();
        }

        public void RegularLogin()
        {
            var player = ChatSession.Player;

            foreach (var room in Chat.StorageManager.Rooms)
                player.ConnectToRoom(room.Key);

            Packet.Builder.SendRooms(player.GetSession());

            Packet.Builder.Legacy(ChatSession, "bv%" + player.Id);

            if (player.Issues.Any(x => !x.Value.CanLogin()))
            {
                var issue = player.Issues.FirstOrDefault(x => !x.Value.CanLogin());
                player.GetSession().Kick("You've been banned until " + issue.Value.Expiry + " by " +
                                         issue.Value.GetIssuer().Name + " at " + issue.Value.IssuedAt + ".\nReason: " +
                                         issue.Value.Reason);
            }
            else if (player.Issues.Any(x => x.Value.Expiry > DateTime.Now))
            {
                foreach (var issue in player.Issues.Where(x => x.Value.Expiry > DateTime.Now))
                {
                    switch (issue.Value.IssueType)
                    {
                        case ChatIssueTypes.WARNING:
                            Packet.Builder.SystemMessage(ChatSession, "You've been warned by " + issue.Value.GetIssuer().Name + " for " + issue.Value.Reason + "\nWarning will expire on: " + issue.Value.Expiry);
                            break;
                        case ChatIssueTypes.MUTE:
                            Packet.Builder.SystemMessage(ChatSession, "You've been muted by " + issue.Value.GetIssuer().Name + " for " + issue.Value.Reason + "\nMute will expire on: " + issue.Value.Expiry);
                            break;
                    }
                }
            }
            else
            {
                if (player is Moderator)
                    Packet.Builder.SystemMessage(ChatSession, Properties.Chat.MOD_LOGIN_MSG);
                else Packet.Builder.SystemMessage(ChatSession, Properties.Chat.USER_LOGIN_MSG);
            }
        }
    }
}