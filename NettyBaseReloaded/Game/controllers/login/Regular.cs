using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.pets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers.login
{
    class Regular : ILogin
    {
        public Regular(GameSession gameSession) : base(gameSession)
        {
        }

        public override void Execute()
        {
            SendSettings();
            Spawn();
            SendLegacy();
            //AddCBS();
        }

        public void Spawn()
        {
            var player = GameSession.Player;
            if (!player.Spacemap.Entities.ContainsKey(player.Id))
            {
                player.Spacemap.Entities.Add(player.Id, player);
            }
            else { Out.WriteLine("Player #" + player.Id + " already exists on the Spacemap (LoginController)", "ERROR", ConsoleColor.Red); }

            Packet.Builder.ShipInitializationCommand(GameSession);
            Packet.Builder.DronesCommand(GameSession, GameSession.Player);
        }

        public void AddCBS()
        {
            var center = GameSession.Player.Position;
            var m1Pos = new Vector(center.X - 413, center.Y - 98);
            var m2Pos = new Vector(center.X - 171, center.Y - 236);
            var m3Pos = new Vector(center.X + 170, center.Y + 236);
            var m4Pos = new Vector(center.X + 412, center.Y - 98);
            var m5Pos = new Vector(center.X + 412, center.Y + 97);
            var m6Pos = new Vector(center.X + 170, center.Y - 235);
            var m7Pos = new Vector(center.X - 171, center.Y + 235);
            var m8Pos = new Vector(center.X - 413, center.Y + 97);

            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111111, "CBS-01", AssetTypeModule.BATTLESTATION, 3, "", 1111111, 0, 0, center, 0, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111112, "M-01", AssetTypeModule.SATELLITE, 3, "BETA", 1111112, 3, 0, m1Pos, 0, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111113, "M-02", AssetTypeModule.SATELLITE, 3, "BETA", 1111113, 4, 0, m2Pos, 0, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111114, "M-03", AssetTypeModule.SATELLITE, 3, "BETA", 1111114, 5, 0, m3Pos, 0, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111115, "M-04", AssetTypeModule.SATELLITE, 3, "BETA", 1111115, 6, 0, m4Pos, 0, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111116, "M-05", AssetTypeModule.SATELLITE, 3, "BETA", 1111116, 7, 0, m5Pos, 0, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111117, "M-06", AssetTypeModule.SATELLITE, 3, "BETA", 1111117, 8, 0, m6Pos, 0, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111118, "M-07", AssetTypeModule.SATELLITE, 3, "BETA", 1111118, 9, 0, m7Pos, 0, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111119, "M-08", AssetTypeModule.SATELLITE, 3, "BETA", 1111119, 10, 0, m8Pos, 0, false, false, true));

        }
    }
}
