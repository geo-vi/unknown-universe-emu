using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Checkers : IAbstractCharacter, ITick
    {
        public int VisibilityRange { get; set; }

        //public int PacketSendRange => 1000;

        public bool InVisibleZone => !Character.Range.Zones.Any(x => x.Value is PalladiumZone);

        public Checkers(AbstractCharacterController controller) : base(controller)
        {
            VisibilityRange = 2000;//900
            Character.Spacemap.EntityAdded += AddedToSpacemap;
            Character.Spacemap.EntityRemoved += RemovedFromSpacemap;
        }

        public void Start()
        {
            //Global.TickManager.Add(this);
        }

        private DateTime LastTick = new DateTime();
        public override void Tick()
        {
            //if (Character is Npc || Character is Pet) return;
            //var pos = MovementController.ActualPosition(Character);
            //if (pos != Character.Position)
            //    MovementController.Move(Character, pos);
            Parallel.Invoke(() =>
            {
                SpacemapChecker();
                RangeChecker();
                ZoneChecker();
                ObjectChecker();
            });
            //Console.WriteLine("VISIBILITY:" + InVisibleZone);

            LastTick = DateTime.Now;
        }

        public override void Stop()
        {
            //Controller.StopController = true;
            //Global.TickManager.Remove(this);
            Character.Spacemap.EntityAdded -= AddedToSpacemap;
            Character.Spacemap.EntityRemoved -= RemovedFromSpacemap;
        }

        #region Character related
        private void AddedToSpacemap(object sender, CharacterArgs args)
        {
            //if (!InVisibleZone && args.Character.InRange(Character, VisibilityRange) || InVisibleZone && args.Character.Controller.Checkers.InVisibleZone)
            AddCharacter(Character, args.Character);
        }

        private void RemovedFromSpacemap(object sender, CharacterArgs args)
        {
            RemoveCharacter(args.Character, Character);
        }

        private void AddCharacter(Character main, Character entity)
        {
            if (!main.Controller.Active) return;
            if (main.Range.AddEntity(entity))
            {
                if (!(main is Player) || entity is Pet) return;
                var gameSession = World.StorageManager.GameSessions[main.Id];

                //Packet.Builder.LegacyModule(gameSession, $"0|A|STD|AddCharacter {entity.Position}");
                //Draws the entity ship for character
                Packet.Builder.ShipCreateCommand(gameSession, entity);
                Packet.Builder.DronesCommand(gameSession, entity);
                                
                //Send movement
                var timeElapsed = (DateTime.Now - entity.MovementStartTime).TotalMilliseconds;
                Packet.Builder.MoveCommand(gameSession, entity, (int)(entity.MovementTime - timeElapsed));

                TitleCheck(gameSession.Player, entity);
            }
        }

        private void RemoveCharacter(Character main, Character entity)
        {
            if (!entity.Controller.Active) return;
            if (entity.Range.RemoveEntity(main))
            {
                if (!(entity is Player)) return;
                var gameSession = World.StorageManager.GameSessions[entity.Id];

                //Packet.Builder.LegacyModule(gameSession, "0|A|STD|RemoveCharacter");
                Packet.Builder.ShipRemoveCommand(gameSession, main);
                if (main.Selected != null && main.Selected.Id == entity.Id)
                {
                    Packet.Builder.ShipSelectionCommand(gameSession, null);
                    main.Selected = null;
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
            if (entity == Character)
                return;

            if ((entity.Spacemap != Character.Spacemap || entity.Controller.StopController) && entity.Range.Entities.ContainsKey(Character.Id))
            {
                RemoveCharacter(entity, Character);
                return;
            }

            if (entity is Pet)
            {
                var pet = entity as Pet;
                if (pet.GetOwner() == Character)
                    return;
            }
            if (GetForSelection(entity)) return;

            if (Character.InRange(entity, VisibilityRange))
            {
                if (!Character.Range.Entities.ContainsKey(entity.Id))
                    AddCharacter(Character, entity);
            }
            else
            {
                if (Character.Range.Entities.ContainsKey(entity.Id))
                    RemoveCharacter(entity, Character);
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
                    }
                    else
                    {
                        if (Character.Range.Zones.ContainsKey(zone.Id)) Character.Range.Zones.Remove(zone.Id);
                    }
                }
            }
            catch (Exception e)
            {
                if (Character.Position == null || Character.Spacemap == null) return;
                new ExceptionLog("checkers", "Zone Checker", e);
            }
        }
        #endregion
        #region Object related
        private void ObjectChecker()
        {
            //if (!(Character is Player)) return;
            try
            {
                foreach (var obj in Character.Spacemap.Objects.Values)
                {
                    if (obj == null) continue;
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
                if (Character.Range.Objects.Count != Character.Spacemap.Objects.Count)
                {
                    var diff = Character.Range.Objects.Except(Character.Spacemap.Objects).Concat(Character.Spacemap.Objects.Except(Character.Range.Objects));
                    foreach (var objDiff in diff)
                    {
                        if (objDiff.Value == null) continue;
                        if (objDiff.Value.Position == null)
                        {
                            Character.Range.RemoveObject(objDiff.Value);
                            (Character as Player)?.ClickableCheck(objDiff.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (Character?.Position == null || Character?.Spacemap == null) return;
                //new ExceptionLog("checkers", "Object Checker", e);
                //Error in checkers->Disconnecting player
               // World.StorageManager.GetGameSession(Character.Id)?.Disconnect(GameSession.DisconnectionType.ERROR);
            }
        }
        #endregion
    }
}
