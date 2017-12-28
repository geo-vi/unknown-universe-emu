using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.player;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
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

        public Storage(Player player) : base(player)
        {
        }

        public void Tick()
        {
            if (DistancePassed > 1000)
                World.DatabaseManager.SavePlayerHangar(Player);
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
            Packet.Builder.JumpgateCreateCommand(gameSession, portal);
        }

        public void LoadAsteroid(Asteroid asteroid)
        {
            LoadAsset(asteroid);
            Packet.Builder.BattleStationNoClanUiInitializationCommand(World.StorageManager.GetGameSession(Player.Id), asteroid);
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

        public void Clean()
        {
            LoadedObjects = new Dictionary<int, Object>();
            LoadedPOI = new Dictionary<string, POI>();
        }
    }
}
