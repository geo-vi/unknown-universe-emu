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
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main;
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
            InitiateEvents();
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
                player.Spacemap.AddEntity(player);
            }
            else
            {
                player.Range.Clear();
                player.Storage.Clean();
            }

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

            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111111, "CBS-01", AssetTypes.BATTLESTATION, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111112, "M-01", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111113, "M-02", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111114, "M-03", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111115, "M-04", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111116, "M-05", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111117, "M-06", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111118, "M-07", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
            Packet.Builder.AssetCreateCommand(GameSession, new Asset(1111119, "M-08", AssetTypes.SATELLITE, Faction.VRU, Global.StorageManager.Clans[0], 0, 0, center, false, false, true));
        }
    }
}
