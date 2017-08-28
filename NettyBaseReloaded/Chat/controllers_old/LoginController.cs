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

            if (AllowedToEnter() || chatSession.Character is Moderator)
            {
                if (Properties.Server.LOCKED) Locked();
                else if (ChatSession.Character is Moderator) ModLogin();
                else RegularLogin();
                LoadTickables();
            }
        }

        public bool AllowedToEnter()
        {
            var id = ChatSession.Character.Id;
            return (id == 544 || id == 495 || id == 498 || id == 497 || id == 697) ||
                   Properties.Server.PUBLIC_BETA_END > DateTime.Now;
        }

        public void Locked()
        {
            throw new NotImplementedException();
        }

        public void ModLogin()
        {
            throw new NotImplementedException();
        }

        public void RegularLogin()
        {
            var character = ChatSession.Character;

            character.ConnectToRoom(Chat.StorageManager.Rooms[0]); // TODO : Do some actual look for which rooms 2join

            ((PlayerController)character.Controller).SendRooms();
            Packet.Builder.Legacy(ChatSession, "bv%" + character.Id);

            if (character is Moderator)
                Packet.Builder.Legacy(ChatSession, "dq%" + Properties.Chat.MOD_LOGIN_MSG);
            else Packet.Builder.Legacy(ChatSession, "dq%" + Properties.Chat.USER_LOGIN_MSG);
        }

        public void LoadTickables()
        {
            var character = ChatSession.Character;
            if (!Global.TickManager.Exists(character.Controller))
                Global.TickManager.Tickables.Add(character.Controller);
        }
    }
}