using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;

namespace NettyBaseReloaded.Main.commands
{
    class UnbugCommand : Command
    {
        public UnbugCommand() : base("unbug", "Unbugs a user", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            throw new NotImplementedException();
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            //if (args?.Length < 2)
            //{
            //    Console.WriteLine("Possible unbugs: [map/ui]");
            //    return;
            //}

            var gameSession = session.GetEquivilentGameSession();
            if (gameSession == null) return;
            //switch (args[1])
            //{
            //    case "map":
            gameSession.Player.MoveToMap(gameSession.Player.Spacemap, gameSession.Player.Position, 0);
            //        break;
            //    case "ui":
            var qs = new Game.netty.commands.old_client.QualitySettingsModule(false, 3, 3, 3, true, 3, 3, 3, 3, 3,
                3);
            var asm = new Game.netty.commands.old_client.AudioSettingsModule(false, false, false);
            var ws = new Game.netty.commands.old_client.WindowSettingsModule(false, 0,
                "0,444,-1,0,1,1057,329,1,20,39,530,0,3,1021,528,1,5,-10,-6,0,24,463,15,0,10,101,307,0,36,100,400,0,13,315,122,0,23,1067,132,0",
                "5,240,150,20,300,150,36,260,175,", 11, "313,480", "23,0,24,0,25,1,26,0,27,0", "313,451", "0",
                "313,500", "0");
            var gm = new Game.netty.commands.old_client.GameplaySettingsModule(false, true, true, true, true, true,
                true, true);
            var ds = new Game.netty.commands.old_client.DisplaySettingsModule(false, true, true, true, true, true,
                false, true, true, true, true, true, true, true, true, true);

            var settings =
                new Game.netty.commands.old_client.UserSettingsCommand(qs, ds, asm, ws, gm);

            gameSession.Player.Settings.OldClientUserSettingsCommand = settings;

            gameSession.Player.Settings.OldClientShipSettingsCommand = new Game.netty.commands.old_client.ShipSettingsCommand("", "", 0, 0, 0);
            gameSession.Player.Settings.SaveSettings();

            Packet.Builder.UserSettingsCommand(gameSession);
            //        break;
            //}
        }
    }
}
