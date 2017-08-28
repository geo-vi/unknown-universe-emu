using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.storages.playerStorages;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Game.controllers
{
    abstract class AbstractCharacterController : ITick
    {
        #region Class checker
        public List<IChecker> CheckedClasses = new List<IChecker>();
        public interface IChecker
        {
            void Check();
        }
        #endregion

        /// <summary>
        /// This is the main Character
        /// </summary>
        public Character Character { get; }

        public CooldownStorage CooldownStorage { get; }

        public int AttackRange = 700;

        public bool Attacking { get; set; }

        public bool Attacked { get; set; }

        public bool Dead { get; set; }

        public bool StopController { get; set; }

        public bool Targetable { get; set; }

        public AbstractCharacterController(Character character)
        {
            Character = character;
            CooldownStorage = new CooldownStorage();
        }

        /// <summary>
        /// Ticker
        /// </summary>
        public void Tick()
        {
            if (this is NpcController)
            {
                var npc = (Npc) Character;
                npc.Controller?.Tick();
                return;
            }

            if (this is PlayerController)
            {
                var player = (Player) Character;
                player.Controller?.Tick();
                return;
            }
        }

        /// <summary>
        /// Performing all the checks under 'one' function
        /// </summary>
        public void Checkers()
        {
            CharacterChecker();
        }


        /// <summary>
        /// This should check for any range entities and if there are any it will add them to RangeEntities list
        /// </summary>
        public void CharacterChecker()
        {
            foreach (var entry in Character.Spacemap.Entities)
            {
                try
                {
                    var entity = entry.Value;
                    if (Character != entity)
                    {
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
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Adds the character to range
        /// </summary>
        public void AddCharacter(Character main, Character entity)
        {
            if (main.RangeEntities.ContainsKey(entity.Id)) return;

            main.RangeEntities.Add(entity.Id, entity);

            if (main is Player)
            {
                var gameSession = World.StorageManager.GetGameSession(main.Id);
                Packet.Builder.ShipCreateCommand(gameSession, entity);
                Packet.Builder.DronesCommand(gameSession, entity);

                var timeElapsed = (DateTime.Now - entity.MovementStartTime).TotalMilliseconds;
                Packet.Builder.MoveCommand(gameSession, entity, (int)(entity.MovementTime - timeElapsed));
            }
        }

        public void RemoveCharacter(Character main, Character entity)
        {
            throw new NotImplementedException();
        }

        public Character GetAttacker()
        {
            throw new NotImplementedException();
        }

        public void LaserAttack()
        {
            throw new NotImplementedException();
        }

        public void Remove(Character targetCharacter)
        {
            throw new NotImplementedException();
        }

        public void Heal(int amount, int healerId = 0, HealType healType = HealType.HEALTH)
        {
            throw new NotImplementedException();
        }
    }
}