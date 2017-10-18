using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.controllers.login
{
    abstract class ILogin
    {
        public GameSession GameSession;


        protected ILogin(GameSession gameSession)
        {
            GameSession = gameSession;
        }

        /// <summary>
        /// Executable for login
        /// </summary>
        public abstract void Execute();

        public void SendSettings()
        {
            Packet.Builder.HotkeysCommand(GameSession);
            Packet.Builder.UserSettingsCommand(GameSession);
            Packet.Builder.SendUserSettings(GameSession);
        }

        public void SendLegacy()
        {
            Packet.Builder.LegacyModule(GameSession, "0|n|t|" + GameSession.Player.Id + "|222|most_wanted");

            //Packet.Builder.LegacyModule(GameSession, "0|A|ITM|" + GameSession.Player.GetConsumablesPacket());
            Packet.Builder.LegacyModule(GameSession, "0|A|BK|0"); //green booty
            Packet.Builder.LegacyModule(GameSession, "0|A|BKR|0"); //red booty
            Packet.Builder.LegacyModule(GameSession, "0|A|BKB|0"); //blue booty
            Packet.Builder.LegacyModule(GameSession, "0|TR");
            Packet.Builder.LegacyModule(GameSession, "0|A|CC|" + GameSession.Player.CurrentConfig);
            Packet.Builder.LegacyModule(GameSession, "0|ps|nüscht|");
            Packet.Builder.LegacyModule(GameSession, "0|ps|blk|0");
            Packet.Builder.LegacyModule(GameSession, "0|n|w|-1"); //enemy warning
            Packet.Builder.LegacyModule(GameSession, "0|g|a|b,1000,1,10000.0,C,2,500.0,U,3,1000.0,U,5,1000.0,U|r,100,1,10000,C,2,50000,C,3,500.0,U,4,700.0,");

            Packet.Builder.VideoWindowCreateCommand(GameSession, 1, "c", true, new List<string> { "login_dialog_1", "login_dialog_2" }, 0, 1);
            //Packet.Builder.MineCreateCommand(GameSession, "asdf", 6, GameSession.Player.Position, false);
            Packet.Builder.PetInitializationCommand(GameSession, GameSession.Player.Pet);
        }
    }
}
