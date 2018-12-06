using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Game.controllers.player;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.handlers;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players.statistics;
using NettyBaseReloaded.Main.objects;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;
using Range = NettyBaseReloaded.Game.objects.world.characters.Range;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Storage : PlayerBaseClass
    {
        public Dictionary<int, Object> LoadedObjects = new Dictionary<int, Object>();

        public Dictionary<string, POI> LoadedPOI = new Dictionary<string, POI>();

        public ConcurrentDictionary<int, LogMessage> LogMessages = new ConcurrentDictionary<int, LogMessage>();

        public double DistancePassed = 0;

        public Dictionary<int, Group> GroupInvites = new Dictionary<int, Group>();

        public bool BlockedGroupInvites { get; set; }

        public Task<bool> RemoveTask { get; internal set; }

        public bool EnergyLeechActivated = false;
        public bool BattleRepairRobotActivated = false;
        public bool PrecisionTargeterActivated = false;

        public bool SentinelFortressActive = false;

        public Storage(Player player) : base(player)
        {
            player.Ticked += Ticked;
        }

        private void Ticked(object sender, EventArgs eventArgs)
        {
            if (DistancePassed > 1000)
                World.DatabaseManager.SavePlayerHangar(Player);
            CleanInvites();
        }

        private DateTime LastInviteClean = new DateTime();
        public void CleanInvites()
        {
            if (LastInviteClean.AddSeconds(30) < DateTime.Now)
            {
                foreach (var invite in GroupInvites.ToList())
                {
                    if (World.StorageManager.GetGameSession(invite.Key) == null ||
                        invite.Value == null)
                    {
                        GroupInvites.Remove(invite.Key);
                        var gHandler = new GroupSystemHandler();
                        gHandler.Error(Player.GetGameSession(), ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NOT_AVAILABLE);
                        gHandler.DeleteInvitation(World.StorageManager.GetGameSession(invite.Key)?.Player, Player);
                    }
                }
                LastInviteClean = DateTime.Now;
            }
        }

        public void LoadPOI(POI poi)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (!LoadedPOI.ContainsKey(poi.Id))
                LoadedPOI.Add(poi.Id, poi);
            Packet.Builder.MapAddPOICommand(gameSession, poi);
        }

        public void LoadStation(Station station)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (!LoadedObjects.ContainsKey(station.Id))
                LoadedObjects.Add(station.Id, station);
            Packet.Builder.StationCreateCommand(gameSession, station);
        }

        public void LoadPortal(Jumpgate portal)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);

            if (!LoadedObjects.ContainsKey(portal.Id))
                LoadedObjects.Add(portal.Id, portal);

            if (portal.Owner != null && portal.Owner != Player) return;
            Packet.Builder.JumpgateCreateCommand(gameSession, portal);
        }

        public void LoadAsteroid(Asteroid asteroid)
        {
            LoadAsset(asteroid);
            //Packet.Builder.BattleStationNoClanUiInitializationCommand(World.StorageManager.GetGameSession(Player.Id), asteroid);
        }

        public void LoadAsset(Asset asset)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (!LoadedObjects.ContainsKey(asset.Id))
                LoadedObjects.Add(asset.Id, asset);
            Packet.Builder.AssetCreateCommand(gameSession, asset);
        }

        public void LoadCollectable(Collectable collectable)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (!LoadedObjects.ContainsKey(collectable.Id))
                LoadedObjects.Add(collectable.Id, collectable);
            Packet.Builder.CreateBoxCommand(gameSession, collectable);
        }

        public void UnLoadCollectable(Collectable collectable)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (LoadedObjects.ContainsKey(collectable.Id))
                LoadedObjects.Remove(collectable.Id);
            Packet.Builder.DisposeBoxCommand(gameSession, collectable);
        }

        public void LoadResource(Ore ore)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (!LoadedObjects.ContainsKey(ore.Id))
                LoadedObjects.Add(ore.Id, ore);
            Packet.Builder.AddOreCommand(gameSession, ore);
        }

        public void UnloadAsset(Asset asset)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (LoadedObjects.ContainsKey(asset.Id))
                LoadedObjects.Remove(asset.Id);
            Packet.Builder.AssetRemoveCommand(gameSession, asset);
        }

        public void Clean()
        {
            LoadedObjects = new Dictionary<int, Object>();
            LoadedPOI = new Dictionary<string, POI>();
        }

        public void LoadBillboard(Billboard billboard)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (!LoadedObjects.ContainsKey(billboard.Id))
                LoadedObjects.Add(billboard.Id, billboard);
            Packet.Builder.MapAssetAddBillboardCommand(gameSession, billboard);
        }

        public void LoadMine(Mine mine)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            if (!LoadedObjects.ContainsKey(mine.Id))
                LoadedObjects.Add(mine.Id, mine);
            Packet.Builder.MineCreateCommand(gameSession, mine.Hash, mine.MineType, mine.Position, mine.PulseActive);
        }
    }
}
