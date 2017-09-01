using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.storages.playerStorages;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Utils
    {
        private Player Player { get; }

        public Utils(Player player)
        {
            Player = player;
        }

        public void LoadObject(Object obj)
        {
            if (obj is Station) LoadStation(obj as Station);
            else if (obj is Jumpgate) LoadPortal(obj as Jumpgate);
            else if (obj is Asteroid) LoadAsteroid(obj as Asteroid);
            else if (obj is Asset) LoadAsset(obj as Asset);
            else if (obj is Collectable) LoadCollectable(obj as Collectable);
            else if (obj is Ore) LoadResource(obj as Ore);
        }

        public void LoadPOI(POI poi)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            Player.EntitiesStorage.LoadedPOI.Add(poi.Id, poi);
            Packet.Builder.MapAddPOICommand(gameSession, poi);
        }

        private void LoadStation(Station station)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            Player.EntitiesStorage.LoadedObjects.Add(station.Id, station);
            Packet.Builder.StationCreateCommand(gameSession, station);
        }

        private void LoadPortal(Jumpgate portal)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            Player.EntitiesStorage.LoadedObjects.Add(portal.Id, portal);
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
            Player.EntitiesStorage.LoadedObjects.Add(asset.Id, asset);
            Packet.Builder.AssetCreateCommand(gameSession, asset);
        }

        public void LoadCollectable(Collectable collectable)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            Player.EntitiesStorage.LoadedObjects.Add(collectable.Id, collectable);
            Packet.Builder.CreateBoxCommand(gameSession, collectable);
        }

        public void LoadResource(Ore ore)
        {
            var gameSession = World.StorageManager.GetGameSession(Player.Id);
            Player.EntitiesStorage.LoadedObjects.Add(ore.Id, ore);
            Packet.Builder.AddOreCommand(gameSession, ore);
        }
    }
}
