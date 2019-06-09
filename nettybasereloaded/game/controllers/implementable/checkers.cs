using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;
using Object = System.Object;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Checkers : IAbstractCharacter
    {
        public readonly object ThreadLock = new object();

        public int VisibilityRange { get; set; }

        public bool InVisibleZone => !Character.Range.Zones.Any(x => x.Value is PalladiumZone);

        public Checkers(AbstractCharacterController controller) : base(controller)
        {
            VisibilityRange = 2000;//900
            //Character.Spacemap.EntityAdded += AddedToSpacemap;
            //Character.Spacemap.EntityRemoved += RemovedFromSpacemap;
            Character.Spacemap.RemovedObject += SpacemapOnRemovedObject;
        }

        public void Start()
        {
        }

        public override void Tick()
        {
            if (Controller.Character is Player player)
            {
                if (!player.IsLoaded)
                {
                    return;
                }
            }
            SpacemapChecker();
            RangeChecker();
            ZoneChecker();
            ObjectChecker();
            POIChecker();
        }

        public override void Stop()
        {
            //Character.Spacemap.EntityAdded -= AddedToSpacemap;
            //Character.Spacemap.EntityRemoved -= RemovedFromSpacemap;
        }
        
        #region Character related
        private void AddCharacter(Character main, Character entity)
        {
            lock (ThreadLock)
            {
                if (entity.Id != main.Id && main.Range.AddEntity(entity))
                {
                    if (!(main is Player)) return;
                    var gameSession = World.StorageManager.GetGameSession(main.Id);
                    if (gameSession == null || gameSession.Player.Pet == entity) return;
                    //Packet.Builder.LegacyModule(gameSession, $"0|A|STD|AddCharacter {entity.Position}");
                    //Draws the entity ship for character
                    Packet.Builder.ShipCreateCommand(gameSession, entity);
                    Packet.Builder.DronesCommand(gameSession, entity);
                    //Send movement
                    var timeElapsed = (DateTime.Now - entity.MovementStartTime).TotalMilliseconds;
                    Packet.Builder.MoveCommand(gameSession, entity, (int) (entity.MovementTime - timeElapsed));

                    TitleCheck(gameSession.Player, entity);
                }
            }
        }

        public void RemoveCharacter(Character main, Character entity)
        {
            //if (!entity.Controller.Active) return;

            lock (ThreadLock)
            {
                if (entity != null && main != null && entity.Id != main.Id && entity.Range.RemoveEntity(main))
                {
                    if (!(entity is Player)) return;
                    var gameSession = World.StorageManager.GetGameSession(entity.Id);
                    if (gameSession == null) return;

                    //Packet.Builder.LegacyModule(gameSession, "0|A|STD|RemoveCharacter");
                    Packet.Builder.ShipRemoveCommand(gameSession, main);
                    if (main.Selected != null && main.Selected.Id == entity.Id)
                    {
                        Packet.Builder.ShipSelectionCommand(gameSession, null);
                        main.Selected = null;
                    }
                }
            }
        }

        public void SpacemapChecker()
        {
            foreach (var entry in Character.Spacemap.Entities)
            {
                var entity = entry.Value;
                EntityCheck(entity);
            }
        }

        public void RangeChecker()
        {
            foreach (var entry in Character.Range.Entities)
            {
                var entity = entry.Value;
                EntityCheck(entity);
            }
        }

        private void EntityCheck(Character entity)
        {
            lock (ThreadLock)
            {
                if (entity == Character || entity == null)
                    return;

                if (entity is Player playerEntity)
                {
                    if (playerEntity.GetGameSession() == null)
                    {
                        RemoveCharacter(entity, Character);
                        return;
                    }
                }

                if (entity.Controller == null ||
                    (entity.Spacemap != Character.Spacemap || !Character.Spacemap.Entities.ContainsKey(entity.Id) ||
                     entity.Controller.StopController) && entity.Range.Entities.ContainsKey(Character.Id))
                {
                    RemoveCharacter(entity, Character);
                    return;
                }

                if (Character.InRange(entity, entity.RenderRange))
                {
                    if (!Character.Range.Entities.ContainsKey(entity.Id))
                    {
                        AddCharacter(Character, entity);
                    }
                }
                else
                {
                    if (Character.Range.Entities.ContainsKey(entity.Id))
                        RemoveCharacter(entity, Character);
                }
            }
        }

        private bool GetForSelection(Character entity)
        {
            if (Character.Spacemap.Entities.ContainsKey(entity.Id))
            {
                if (entity.Selected == Character && Character.Selected == entity)
                {
                    if (!Character.Range.Entities.ContainsKey(entity.Id))
                        Character.Range.AddEntity(entity);
                    if (!entity.Range.Entities.ContainsKey(Character.Id))
                        entity.Range.AddEntity(Character);
                    return true;
                }
            }
            else
            {

            }
            return false;
        }

        private void TitleCheck(Player main, Character entity)
        {
            var player = entity as Player;
            if (player?.Information.Title != null)
            {
                Packet.Builder.TitleCommand(main.GetGameSession(), player);
            }
        }

        public void RemoveEntity(Character target)
        {
            RemoveCharacter(target, Character);
        }

        #endregion
        #region Zone related
        private void ZoneChecker()
        {
            try
            {
                foreach (var zone in Character.Spacemap.Zones.Values.ToList())
                {
                    if ((Character.Position.X >= zone.TopLeft.X && Character.Position.X <= zone.BottomRight.X) &&
                        (Character.Position.Y <= zone.TopLeft.Y && Character.Position.Y >= zone.BottomRight.Y))
                    {
                        if (!Character.Range.Zones.ContainsKey(zone.Id))
                        {
                            Character.Range.Zones.Add(zone.Id, zone);
                        }

                        if (zone is PalladiumZone)
                        {
                            Character.Invisible = false;
                            Controller.Effects.UpdateCharacterVisibility();
                        }
                    }
                    else
                    {
                        if (Character.Range.Zones.ContainsKey(zone.Id)) Character.Range.Zones.Remove(zone.Id);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
            }
        }
        #endregion
        #region Object related
        private void ObjectChecker()
        {
            if (!(Character is Player || Character is Pet)) return;
            try
            {
                if (Character is Player player)
                {
                    var dicOne = player.Storage.LoadedObjects.ToList();
                    var dicTwo = player.Spacemap.Objects.Where(x => x.Value != null);

                    var diff = dicOne.Except(dicTwo).Concat(dicTwo.Except(dicOne));

                    foreach (var objDiff in diff)
                    {
                        if (!player.Storage.LoadedObjects.ContainsKey(objDiff.Key) &&
                            Character.Spacemap.Objects.ContainsKey(objDiff.Key))
                        {
                            player.LoadObject(objDiff.Value);
                        }
                        else if (player.Storage.LoadedObjects.ContainsKey(objDiff.Key) &&
                                 !Character.Spacemap.Objects.ContainsKey(objDiff.Key))
                        {
                            player.UnloadObject(objDiff.Value);
                        }
                    }
                }
                foreach (var obj in Character.Spacemap.Objects.Values)
                {
                    if (obj == null || obj.Position == null) continue;
                    if (Vector.IsInRange(obj.Position, Character.Position, obj.Range))
                    {
                        if (!Character.Range.Objects.ContainsKey(obj.Id))
                        {
                            Character.Range.AddObject(obj);
                            obj.execute(Character);
                            (Character as Player)?.ClickableCheck(obj);
                        }
                    }
                    else
                    {
                        if (Character.Range.Objects.ContainsKey(obj.Id))
                        {
                            Character.Range.RemoveObject(obj);
                            (Character as Player)?.ClickableCheck(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (Character?.Position == null || Character?.Spacemap == null) return;
                Console.WriteLine("object error");
                Console.WriteLine(e);
                //new ExceptionLog("checkers", "Object Checker", e);
                //Error in checkers->Disconnecting player
               // World.StorageManager.GetGameSession(Character.Id)?.Disconnect(GameSession.DisconnectionType.ERROR);
            }
        }

        private void SpacemapOnRemovedObject(object sender, objects.world.map.Object e)
        {
            if (Character is Player player)
            {
                player.UnloadObject(e);
            }
        }

        #endregion

        #region POI related

        public void POIChecker()
        {
            if (Character is Player player)
            {
                var dicOne = player.Storage.LoadedPOI.ToList();
                var dicTwo = player.Spacemap.POIs.Where(x => x.Value != null);

                var diff = dicOne.Except(dicTwo).Concat(dicTwo.Except(dicOne));

                foreach (var poiDiff in diff)
                {
                    if (!player.Storage.LoadedPOI.ContainsKey(poiDiff.Key) &&
                        Character.Spacemap.POIs.ContainsKey(poiDiff.Key))
                    {
                        player.Storage.LoadPOI(poiDiff.Value);
                    }
                    else if (player.Storage.LoadedPOI.ContainsKey(poiDiff.Key) &&
                             !Character.Spacemap.POIs.ContainsKey(poiDiff.Key))
                    {
                        player.Storage.UnloadPOI(poiDiff.Value);
                    }
                }
            }
        }

        #endregion
    }
}
