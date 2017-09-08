using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Storage : PlayerBaseClass
    {
        public Dictionary<int, Object> LoadedObjects = new Dictionary<int, Object>();

        public Dictionary<string, POI> LoadedPOI = new Dictionary<string, POI>();

        public Storage(Player player) : base(player)
        {
        }

        public void LoadPOI(POI poi)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            LoadedPOI.Add(poi.Id, poi);
            Packet.Builder.MapAddPOICommand(gameSession, poi);
        }

        public void LoadStation(Station station)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            LoadedObjects.Add(station.Id, station);
            Packet.Builder.StationCreateCommand(gameSession, station);
        }

        public void LoadPortal(Jumpgate portal)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
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
            LoadedObjects.Add(asset.Id, asset);
            Packet.Builder.AssetCreateCommand(gameSession, asset);
        }

        public void LoadCollectable(Collectable collectable)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            LoadedObjects.Add(collectable.Id, collectable);
            Packet.Builder.CreateBoxCommand(gameSession, collectable);
        }

        public void LoadResource(Ore ore)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            LoadedObjects.Add(ore.Id, ore);
            Packet.Builder.AddOreCommand(gameSession, ore);
        }

        public void Clean()
        {
            LoadedObjects.Clear();
            LoadedPOI.Clear();
        }
    }
}
