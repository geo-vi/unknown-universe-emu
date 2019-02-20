using System;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
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

            if (player is Moderator)
                Packet.Builder.SystemMessage(ChatSession, Properties.Chat.MOD_LOGIN_MSG);
            else Packet.Builder.SystemMessage(ChatSession, Properties.Chat.USER_LOGIN_MSG);

            LoadAnnouncementRoom();
        }

        private void LoadAnnouncementRoom()
        {
            
        }
    }
}