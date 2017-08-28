using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.controllers
{
    class ModController : AbstractCharacterController
    {
        private Moderator Moderator { get; }

        public ModController(Character character) : base(character)
        {
            Moderator = (Moderator) character;
        }

        public void Tick()
        {
            
        }

        public void Login()
        {
            //ChatClient.SendToAll(Moderator, Constants.CMD_UPDATE_LOGIN_LOGOUT + "%" + Moderator.Id + "@" + DateTime.Now + "@" + Moderator.LastLogout + "@" + Convert.ToInt32(Moderator.Online) + "@" + Convert.ToInt32(Moderator.Invisible) + "#", false);
            Logger.Logger.WritingManager.Write("Moderator [" + Moderator.Name + ", " + Moderator.Id + "] has logged into Chat Tool.");
        }
    }
}
