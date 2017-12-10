using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Checkers : IAbstractCharacter, ITick
    {
        public Checkers(AbstractCharacterController controller) : base(controller)
        {
        }

        public void Start()
        {
            Global.TickManager.Add(this);
        }

        public override void Tick()
        {
            UpdateEntity(Character);
            CharacterChecker();
            ZoneChecker();
            ObjectChecker();
        }

        public override void Stop()
        {
            Controller.StopController = true;
            Global.TickManager.Remove(this);
        }

        private void UpdateEntity(Character character)
        {
            try
            {
                if (!character.Position.Equals(character.Destination))
                {
                    character.Position = MovementController.ActualPosition(character);
                }
            }
            catch (Exception)
            {
                // Position is null
            }
        }

        private void AddCharacter(Character main, Character entity)
        {
            try
            {
                if (main.Range.AddEntity(entity))
                {
                    if (!(main is Player)) return;
                    var gameSession = World.StorageManager.GameSessions[main.Id];

                    Packet.Builder.LegacyModule(gameSession, $"0|A|STD|AddCharacter {entity.Position}");
                    //Draws the entity ship for character
                    Packet.Builder.ShipCreateCommand(gameSession, entity);
                    Packet.Builder.DronesCommand(gameSession, entity);

                    //Send movement
                    var timeElapsed = (DateTime.Now - entity.MovementStartTime).TotalMilliseconds;
                    Packet.Builder.MoveCommand(gameSession, entity, (int) (entity.MovementTime - timeElapsed));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Range Error Occured - Add/ Players: {main.Id}, {entity.Id}");
                Console.WriteLine(e.Message);
            }
        }

        private void RemoveCharacter(Character main, Character entity)
        {
            try
            {
                if (entity.Range.RemoveEntity(main))
                {
                    if (!(entity is Player)) return;
                    var gameSession = World.StorageManager.GameSessions[entity.Id];

                    Packet.Builder.LegacyModule(gameSession, "0|A|STD|RemoveCharacter");
                    Packet.Builder.ShipRemoveCommand(gameSession, main);
                    if (main.Selected != null && main.Selected.Id == entity.Id)
                    {
                        Packet.Builder.ShipSelectionCommand(gameSession, null);
                        main.Selected = null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Range Error Occured - Remove/ Players: {main.Id}, {entity.Id}");
                Console.WriteLine(e.Message);
            }
        }

        public void Update(Character targetCharacter)
        {
            UpdateEntity(targetCharacter);
            if (Character.InRange(targetCharacter))
            {
                AddCharacter(Character, targetCharacter);
            }
            else
            {
                RemoveCharacter(targetCharacter, Character);
            }
        }

        public void CharacterChecker() // Error here
        {
            try
            {
                foreach (var entry in Character.Spacemap.Entities)
                {
                    var entity = entry.Value;
                    if (entity.Position == null || entity.Spacemap != Character.Spacemap)
                    {
                        RemoveCharacter(entity, Character);
                        continue;
                    }

                    UpdateEntity(entity);

                    if (entity is Pet)
                        if (((Pet) entity).GetOwner().Id == Character.Id)
                            continue;
                    if (Character is Pet)
                        if (entity == ((Pet) Character).GetOwner())
                            continue;

                    if (GetForSelection(entity))
                        continue;

                    //If i have the entity in range
                    if (Character.InRange(entity))
                    {
                        AddCharacter(Character, entity);
                    }
                    else
                    {
                        RemoveCharacter(entity, Character);
                    }

                    //If the entity has me in range
                    if (entity.InRange(Character))
                    {
                        AddCharacter(entity, Character);
                    }
                    else
                    {
                        //remove
                        RemoveCharacter(Character, entity);
                    }
                }
                if (Character.Range.Entities.Count != Character.Spacemap.Entities.Count)
                {
                    var diff = Character.Range.Entities.Except(Character.Spacemap.Entities).Concat(Character.Spacemap.Entities.Except(Character.Range.Entities));
                    foreach (var playerDifferance in diff)
                    {
                        if (playerDifferance.Value == null) continue;
                        if (playerDifferance.Value.Spacemap != Character.Spacemap ||
                            playerDifferance.Value.Position == null)
                        {
                            RemoveCharacter(playerDifferance.Value, Character);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //if (Character.Position == null || Character.Spacemap == null)
                //     Stop();
            }
        }

        private bool GetForSelection(Character entity)
        {
            //if (Character.Spacemap.Entities.ContainsKey(entity.Id))
            //{
            //    if (entity.Selected == Character || Character.Selected == entity)
            //    {
            //        if (!Character.RangeEntities.ContainsKey(entity.Id))
            //            Character.RangeEntities.Add(entity.Id, entity);
            //        if (!entity.RangeEntities.ContainsKey(Character.Id))
            //            entity.RangeEntities.Add(Character.Id, Character);
            //        return true;
            //    }
            //}
            return false;
        }

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
                if (Character.Position == null || Character.Spacemap == null || Character.Id == null) return;
                new ExceptionLog("checkers", "Object Checker", e);
                //Error in checkers->Disconnecting player
                World.StorageManager.GetGameSession(Character.Id).Disconnect(GameSession.DisconnectionType.ERROR);
            }
        }
    }
}
