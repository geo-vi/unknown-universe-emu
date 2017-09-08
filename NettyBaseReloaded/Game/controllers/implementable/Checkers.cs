using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Checkers : IAbstractCharacter
    {
        public Checkers(AbstractCharacterController controller) : base(controller)
        {
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
            throw new NotImplementedException();
        }

        private void UpdateEntity(Character character)
        {
            if (!character.Position.Equals(character.Destination))
            {
                character.Position = MovementController.ActualPosition(character);
            }
        }

        private void AddCharacter(Character main, Character entity)
        {
            if (!main.RangeEntities.ContainsKey(entity.Id))
            {
                main.RangeEntities.Add(entity.Id, entity);
                if (!(main is Player)) return;
                //if (entity is Pet)
                //{
                //    var pet = (Pet)entity;
                //    if (main.Id == pet.OwnerId)
                //        return;
                //}

                var gameSession = World.StorageManager.GameSessions[main.Id];

                //Draws the entity ship for character
                Packet.Builder.ShipCreateCommand(gameSession, entity);
                Packet.Builder.DronesCommand(gameSession, entity);

                //Send movement
                var timeElapsed = (DateTime.Now - entity.MovementStartTime).TotalMilliseconds;
                Packet.Builder.MoveCommand(gameSession, entity, (int)(entity.MovementTime - timeElapsed));

            }
        }

        /// <summary>
        /// Removes entity for main character
        /// </summary>
        /// <param name="main"></param>
        /// <param name="entity"></param>
        private void RemoveCharacter(Character main, Character entity)
        {
            if (entity.RangeEntities.ContainsKey(main.Id))
            {
                entity.RangeEntities.Remove(main.Id);
                if (!(entity is Player)) return;
                //if (main is Pet)
                //{
                //    var pet = (Pet)main;
                //    if (entity.Id == pet.OwnerId)
                //        return;
                //}

                var gameSession = World.StorageManager.GameSessions[entity.Id];

                Packet.Builder.ShipRemoveCommand(gameSession, main);
                if (main.Selected != null && main.Selected.Id == entity.Id)
                {
                    Packet.Builder.ShipSelectionCommand(gameSession, null);
                    main.Selected = null;
                }
            }
        }

        public void CharacterChecker()
        {
            foreach (var entry in Character.Spacemap.Entities.ToList())
            {
                var entity = entry.Value;

                UpdateEntity(entity);
                //If i have the entity in range
                if (Character.InRange(entity))
                {
                    AddCharacter(Character, entity);
                }
                else
                {
                    RemoveCharacter(Character, entity);
                }

                //If the entity has me in range
                if (entity.InRange(Character))
                {
                    AddCharacter(entity, Character);
                }
                else
                {
                    //remove
                    RemoveCharacter(entity, Character);
                }
            }
        }

        private void ZoneChecker()
        {
            foreach (var zone in Character.Spacemap.Zones.Values)
            {
                if ((Character.Position.X >= zone.TopLeft.X && Character.Position.X <= zone.BottomRight.X) &&
                    (Character.Position.Y <= zone.TopLeft.Y && Character.Position.Y >= zone.BottomRight.Y))
                {
                    if (!Character.RangeZones.ContainsKey(zone.Id))
                    {
                        Character.RangeZones.Add(zone.Id, zone);
                    }
                }
                else
                {
                    if (Character.RangeZones.ContainsKey(zone.Id)) Character.RangeZones.Remove(zone.Id);
                }
            }

        }

        private void ObjectChecker()
        {
            if (!(Character is Player)) return;
            foreach (var obj in Character.Spacemap.Objects.Values)
            {
                try
                {
                    if (Vector.IsInRange(obj.Position, Character.Position, obj.Range))
                    {
                        if (!Character.RangeObjects.ContainsKey(obj.Id))
                        {
                            Character.RangeObjects.Add(obj.Id, obj);
                            obj.execute(Character);
                            (Character as Player)?.ClickableCheck(obj);
                        }
                    }
                    else
                    {
                        if (Character.RangeObjects.ContainsKey(obj.Id))
                        {
                            Character.RangeObjects.Remove(obj.Id);
                            var player = Character as Player;
                            player?.ClickableCheck(obj);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
