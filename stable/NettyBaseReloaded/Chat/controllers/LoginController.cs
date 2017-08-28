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
                else RegularLogin();
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

        public void RegularLogin()
        {
            var character = ChatSession.Character;

            character.ConnectToRoom(0);

            Packet.Builder.Legacy(ChatSession, "bv%" + character.Id);

            if (character is Moderator)
                Packet.Builder.SystemMessage(ChatSession, Properties.Chat.MOD_LOGIN_MSG);
            else Packet.Builder.SystemMessage(ChatSession, Properties.Chat.USER_LOGIN_MSG);
        }
    }
}