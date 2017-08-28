using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.managers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using NettyBaseReloaded.Game.objects.world.storages.playerStorages;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Networking;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.controllers
{
    class AbstractCharacterController : ITick
    {
        #region Class checker
        public List<IChecker> CheckedClasses = new List<IChecker>();
        public interface IChecker
        {
            void Check();
        }
        #endregion

        public Character Character { get; }

//        public CooldownStorage CooldownStorage { get; set; }

        public bool Dead { get; set; }

        public bool StopController { get; set; }

        public bool Attacking { get; set; }

        public bool Attacked { get; set; }

        public bool Invincible { get; set; }

        public bool Targetable { get; set; }

        public int AttackRange = 700;

        public bool SlowedDown { get; set; }

        public DateTime LastTimeAttacked = new DateTime();

        public AbstractCharacterController(Character character)
        {
            Character = character;

            Dead = false;
            StopController = false;
            Attacking = false;
            Attacked = false;
            Invincible = false;
            Targetable = true;
            SlowedDown = false;

            //CooldownStorage = new CooldownStorage();
        }

        public void Tick()
        {
            if (StopController) return;
            if (Attacking) LaserAttack();

            if (LastTimeAttacked.AddSeconds(3) <= DateTime.Now) Attacked = false;

            if (this is PlayerController)
            {
                var player = (Player) Character;
                player.Controller.Tick();
            }
            else if (this is PetController)
            {
                var pet = (Pet)Character;
                pet.Controller.Tick();
            }
        }

        #region Checkers

        public void Checkers()
        {
            var player = Character as Player;
            if (player?.UserStorage.State != State.READY)
                return;
            UpdateEntity(Character);
            CharacterChecker();
            CollectableChecker();
            ZoneChecker();
            ObjectChecker();
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

        private void CollectableChecker()
        {
            //if (Character is Npc || Character.Spacemap.Collectables.Count <= 0)
            //    return;

            //foreach (var collectable in Character.Spacemap.Collectables)
            //{
            //    if (Vector.IsInRange(collectable.Value.Position, Character.Position, 2000))
            //    {
            //        if (Character.RangeCollectables.ContainsKey(collectable.Key))
            //            return;

            //        if (Character is Player)
            //        {
            //            ServerManager.GameSessions[Character.Id].Client.Send(PacketBuilder.LegacyModule(collectable.Value.ToString()));
            //        }

            //        Character.RangeCollectables.Add(collectable.Key, collectable.Value);
            //    }
            //    else
            //    {
            //        if (Character.RangeCollectables.ContainsKey(collectable.Key))
            //        {
            //            if (Character is Player)
            //            {
            //                World.StorageManager.GameSessions[Character.Id].Client.Send(PacketBuilder.LegacyModule("0|2|" + collectable.Key));
            //            }

            //            Character.RangeCollectables.Remove(collectable.Key);
            //        }
            //    }
            //}
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

        #endregion

        #region Range

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

        #endregion

        #region Attack

        public Character GetAttacker()
        {
            foreach (var entity in Character.RangeEntities.Values)
            {
                if (entity.Controller.Dead) return null;
                if (entity.Selected == Character && entity.Controller.Attacking)
                {
                    return entity;
                }
            }
            return null;
        }

        private DateTime LastLaserLoop = new DateTime();
        public void LaserAttack()
        {
            var enemy = Character.Selected;
            if (Dead || StopController || enemy == null || !Attacking) return;
            if (!Character.Spacemap.Entities.ContainsKey(enemy.Id))
            {
                Character.Selected = null;
                Attacking = false;
                return;
            }

            if (LastLaserLoop.AddSeconds(1) > DateTime.Now) return;
            if (!Character.InRange(enemy, AttackRange))
            {
                var pCharacter = Character as Player;
                if (pCharacter != null)
                {
                    var gameSession = World.StorageManager.GetGameSession(pCharacter.Id);
                    Packet.Builder.LegacyModule(gameSession, "0|A|STM|outofrange");
                }
                return;
            }

            var damage = Character.Damage;
            var absDamage = 0; //This variable will be used for ammo that absobrs shield too

            var laserColor = 0;

            if (Character is Player)
            {
                var gameSession = World.StorageManager.GetGameSession(Character.Id);
                if (gameSession.Player.LaserCount() == 0)
                {
                    Attacking = false; // Will stop attacking if there are no lasers equipped.
                    Packet.Builder.LegacyModule(gameSession, "0|A|STM|no_lasers_on_board");
                    return;
                }

                var pEnemy = enemy as Player;
                if (pEnemy != null)
                {
                    if (pEnemy.InDemiZone)
                        return;
                }

                bool isRsb = false;
                switch (gameSession.Player.Settings.CurrentAmmo)
                {
                    case "ammunition_laser_mcb-25":
                        damage *= 2;
                        laserColor = 1;
                        break;
                    case "ammunition_laser_mcb-50":
                        damage *= 3;
                        laserColor = 2;
                        break;
                    case "ammunition_laser_ucb-100":
                        damage *= 4;
                        laserColor = 3;
                        break;
                    case "ammunition_laser_sab-50":
                        absDamage = damage * 2;
                        damage = 0;
                        laserColor = 4;
                        break;
                    case "ammunition_laser_cbo-100":
                        absDamage = damage;
                        damage = damage * 2;
                        laserColor = 8;
                        break;
                    case "ammunition_laser_rsb-75":
                        damage *= 6;
                        laserColor = 6;
                        isRsb = true;
                        break;
                    case "ammunition_laser_job-100":
                        damage *= 4;
                        laserColor = 9;
                        break;
                }


                if (isRsb)
                {
                    if (Character.Cooldowns.Exists(cooldown => cooldown is RSBCooldown)) return;

                    var newCooldown = new RSBCooldown();
                    newCooldown.Send(gameSession);
                    Character.Cooldowns.Add(newCooldown);
                }
                else
                {
                    if (Character.Cooldowns.Exists(cooldown => cooldown is LaserCooldown)) return;

                    var newCooldown = new LaserCooldown();
                    Character.Cooldowns.Add(newCooldown);
                }
            }

            damage = RandomizeDamage(damage);
            GameClient.SendRangePacket(Character,
                netty.commands.old_client.AttackLaserRunCommand.write(Character.Id, enemy.Id, laserColor, false,
                    true), true);
            GameClient.SendRangePacket(Character,
                netty.commands.new_client.AttackLaserRunCommand.write(Character.Id, enemy.Id, laserColor, false,
                    true), true);
            Damage(enemy, absDamage, damage, 1);

            enemy.Controller.Attacked = true;
            enemy.Controller.LastTimeAttacked = DateTime.Now;
            LastLaserLoop = DateTime.Now;
        }

        private DateTime LastMissleLoop = new DateTime();
        public void LaunchMissle(string missleId)
        {
            var enemy = Character.Selected;
            if (enemy == null)
                return;

            if (!Character.InRange(enemy, AttackRange))
            {
                var pCharacter = Character as Player;
                if (pCharacter != null && LastMissleLoop.AddSeconds(1) < DateTime.Now)
                {
                    var gameSession = World.StorageManager.GetGameSession(pCharacter.Id);
                    Packet.Builder.LegacyModule(gameSession, "0|A|STM|outofrange");
                }
                return;
            }

            int damage = 0;
            int rocketId = 0;

            switch (missleId)
            {
                case "ammunition_rocket_r-310":
                    rocketId = 1;
                    damage = RandomizeDamage(Character.RocketDamage);
                    break;
                case "ammunition_rocket_plt-2026":
                    rocketId = 2;
                    damage = RandomizeDamage(Character.RocketDamage * 2);
                    break;
                case "ammunition_rocket_plt-2021":
                    rocketId = 3;
                    damage = RandomizeDamage(Character.RocketDamage * 3);
                    break;
                case "ammunition_rocket_plt-3030":
                    rocketId = 4;
                    damage = RandomizeDamage(Character.RocketDamage * 4);
                    break;
                case "ammunition_specialammo_pld-8":
                    rocketId = 5;
                    break;
                case "ammunition_specialammo_dcr-250":
                    rocketId = 6;
                    break;
            }

            if (Character.Cooldowns.Exists(cooldown => cooldown is RocketCooldown)) return;

            var newCooldown = new RocketCooldown();
            var player = Character as Player;
            if (player != null) newCooldown.Send(World.StorageManager.GetGameSession(player.Id));
            Character.Cooldowns.Add(newCooldown);

            GameClient.SendRangePacket(Character, netty.commands.old_client.LegacyModule.write("0|v|" + Character.Id + "|" + enemy.Id + "|H|" + rocketId + "|0|0"), true);
            GameClient.SendRangePacket(Character, netty.commands.new_client.LegacyModule.write("0|v|" + Character.Id + "|" + enemy.Id + "|H|" + rocketId + "|0|0"), true);
            Damage(enemy, 0, damage, 1);

            enemy.Controller.Attacked = true;
            enemy.Controller.LastTimeAttacked = DateTime.Now;
            LastMissleLoop = DateTime.Now;
        }

        private int RandomizeDamage(int baseDmg, double missProbability = 1.00)
        {
            var randNums = Random.Next(0, 6);

            if (missProbability < 1.00)
                randNums = Random.Next(0, 7);
            if (missProbability > 1.00 && missProbability < 2.00)
                randNums = Random.Next(0, 4);
            if (missProbability >= 2.00)
                randNums = Random.Next(2, 4);

            switch (randNums)
            {
                case 0:
                    return (int) (baseDmg*1.10);
                case 1:
                    return (int) (baseDmg*0.98);
                case 2:
                    return (int) (baseDmg*1.02);
                case 3:
                    return 0;
                case 4:
                    return (int) (baseDmg*0.92);
                case 5:
                    return (int) (baseDmg*0.99);
                default:
                    return baseDmg;
            }
        }

        public void Damage(Character target, int absDamage, int damage, short damageEffect)
        {
            if (target.Controller.Dead || Character.Controller.Dead) return;
            var attackerSession = (Character is Player) ? World.StorageManager.GetGameSession(Character.Id) : null;
            var targetSession = (target is Player) ? World.StorageManager.GetGameSession(target.Id) : null;

            target.LastCombatTime = DateTime.Now; //To avoid repairing and logging off | My' own logging is set to off in the correspondent handlers

            if (!target.Controller.Invincible)
            {
                #region DamageCalculations

                if (target.CurrentShield > 0)
                {
                    if (absDamage > 0)
                    {
                        target.CurrentShield -= absDamage;
                        Character.CurrentShield += absDamage;
                    }

                    //For example => Target has 80% abs but you' have moth (+20% penetration) :> damage * 0.6

                    var totalAbs = Math.Abs(Character.ShieldPenetration - target.ShieldAbsorption);
                    var shieldDamage = damage * totalAbs;

                    var healthDamage = 0;
                    if (target.CurrentShield <= damage)
                    {
                        healthDamage = Math.Abs(target.CurrentShield - damage);
                        target.CurrentShield = 0;
                        target.CurrentHealth -= healthDamage;
                    }
                    else
                    {
                        target.CurrentShield -= (int)shieldDamage; //Correspondent shield damage
                        healthDamage = (int)(damage * (1 - totalAbs));
                    }

                    if (target.CurrentNanoHull > 0)
                    {
                        //If the player can receive some damage on the nanohull
                        if (target.CurrentNanoHull - healthDamage < 0)
                        {
                            var nanoDamage = healthDamage - target.CurrentNanoHull;
                            target.CurrentNanoHull = 0;
                            target.CurrentHealth -= nanoDamage;
                        }
                        else
                            target.CurrentNanoHull -= healthDamage;
                    }
                    else
                        target.CurrentHealth -= healthDamage; //80% shield abs => 20% life
                }
                else //NO SHIELD
                {
                    absDamage = 0;

                    if (target.CurrentNanoHull > 0)
                    {
                        //If the player can receive some damage on the nanohull
                        if (target.CurrentNanoHull - damage < 0)
                        {
                            var nanoDamage = damage - target.CurrentNanoHull;
                            target.CurrentNanoHull = 0;
                            target.CurrentHealth -= nanoDamage;
                        }
                        else
                            target.CurrentNanoHull -= damage; //Full dmg to nanohull
                    }
                    else
                        target.CurrentHealth -= damage; //Full dmg to health
                }

                #endregion DamageCalculations
            }
            else
            {
                damage = 0;
                absDamage = -1;
            }

            if (attackerSession != null)
                Packet.Builder.AttackHitCommand(attackerSession, Character, target, damage + absDamage, damageEffect);
            if (targetSession != null)
                Packet.Builder.AttackHitCommand(targetSession, Character, target, damage + absDamage, damageEffect);

            if (target.CurrentHealth <= 0 && !target.Controller.Dead)
            {
                Destroy(target);
            }

            target.Update();
            Character.Update();
        }

        /// <summary>
        /// Causes a damage in area.
        /// </summary>
        public void DamageArea(int amount, int distance = 0, DamageType damageType = DamageType.DEFINED)
        {
            if (distance == 0) distance = AttackRange;

            foreach (var entry in Character.Spacemap.Entities.ToList())
            {
                if (Character.Position.DistanceTo(entry.Value.Position) > distance) return;
                if (Character.Id == entry.Value.Id) continue;

                var damage = 0;
                switch (damageType)
                {
                    case DamageType.DEFINED:
                        damage = amount;
                        break;
                    case DamageType.PERCENTAGE:
                        damage = entry.Value.CurrentHealth*amount/100;
                        break;
                }
                //TODO use Damage() method instead

                entry.Value.CurrentHealth -= damage;
                entry.Value.LastCombatTime = DateTime.Now;
                if (entry.Value is Player) Packet.Builder.AttackHitCommand(World.StorageManager.GetGameSession(entry.Value.Id), Character, entry.Value, damage, 14);
                if (Character is Player) Packet.Builder.AttackHitCommand(World.StorageManager.GetGameSession(Character.Id), Character, entry.Value, damage, 14);
            }
        }

        public void Heal(int amount, int healerId = 0, HealType healType = HealType.HEALTH)
        {
            if (amount < 0)
                return;

            var newAmount = amount;
            var oldHp = Character.CurrentHealth;
            var oldShd = Character.CurrentShield;

            switch (healType)
            {
                case HealType.HEALTH:
                    if (Character.CurrentHealth + amount > Character.MaxHealth)
                    {
                        newAmount = (Character.CurrentHealth + amount) - Character.MaxHealth;
                    }
                    Character.CurrentHealth += newAmount;
                    break;
                case HealType.SHIELD:
                    if (Character.CurrentShield + amount > Character.MaxShield)
                    {
                        newAmount = (Character.CurrentShield + amount) - Character.MaxShield;
                    }
                    Character.CurrentShield += newAmount;
                    break;
            }

            if (Character is Player && healType == HealType.HEALTH)
                Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Character.Id), "0|A|HL|" + healerId + "|" + Character.Id + "|HPT|" + oldHp + "|" +
                                               newAmount);
            else if (Character is Player && healType == HealType.SHIELD)
                Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Character.Id), "0|A|HL|" + healerId + "|" + Character.Id + "|SHD|" + oldShd + "|" +
                                               newAmount);

            Character.Update();
        }

        #endregion

        #region Effects

        public void Slowdown(Character targetCharacter)
        {
            //TODO
            GameClient.SendToSpacemap(targetCharacter.Spacemap, netty.commands.new_client.LegacyModule.write("0|n|fx|start|GRAPHIC_FX_SABOTEUR_DEBUFF|" + targetCharacter.Id));
            GameClient.SendToSpacemap(targetCharacter.Spacemap, netty.commands.old_client.LegacyModule.write("0|n|fx|start|GRAPHIC_FX_SABOTEUR_DEBUFF|" + targetCharacter.Id));
        }

        public void SetInvincible(int time, bool showEffect = false)
        {
            if (Character.Cooldowns.Exists(x => x is InvincibilityCooldown)) return;

            var cooldown = new InvincibilityCooldown(showEffect, DateTime.Now.AddMilliseconds(time));
            Character.Cooldowns.Add(cooldown);
            cooldown.OnStart(Character);
        }

        public void NotTargetable(int time)
        {
            if (Character.Cooldowns.Exists(x => x is NonTargetableCooldown)) return;

            var cooldown = new NonTargetableCooldown(DateTime.Now.AddMilliseconds(time));
            Character.Cooldowns.Add(cooldown);
            cooldown.OnStart(Character);
        }
        #endregion

        #region Destruction

        public void Destroy(Character target)
        {
            if (target.CurrentHealth <= 0 && !target.Controller.Dead)
            {
                target.Controller.Kill();
                if (target is Player)
                {
                    var pTarget = (Player) target;
                    if (pTarget.UsingNewClient)
                    {
                        var options = new List<netty.commands.new_client.KillScreenOptionModule>
                        {
                            new KillscreenOption(KillscreenOption.NEAREST_BASE).Object
                        };

                        World.StorageManager.GetGameSession(target.Id)
                            .Client.Send(
                                KillScreenPostCommand.write(Character.Name, "", Character.Hangar.Ship.LootId, new DestructionTypeModule(DestructionTypeModule.USER), options).Bytes);
                    }
                }

                Attacking = false;
                Character.Selected = null;
            }
        }

        public void Kill()
        {
            GameClient.SendRangePacket(Character, ShipDestroyedCommand.write(Character.Id, 0), true);
            GameClient.SendRangePacket(Character, netty.commands.old_client.ShipDestroyedCommand.write(Character.Id, 0), true);

            Attacking = false;
            Character.Selected = null;
            Dead = true;

            //Remove from the spacemap
            if (Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.Entities.Remove(Character.Id);
            StopAll();
        }

        public void Remove(Character targetCharacter)
        {
            if (targetCharacter == null)
                return;

            if (targetCharacter.Spacemap.Entities.ContainsKey(targetCharacter.Id))
                targetCharacter.Spacemap.Entities.Remove(targetCharacter.Id);

            if (targetCharacter is Player)
            {
                var player = (Player)targetCharacter;
                player.InstantSave();
            }

            foreach (var user in targetCharacter.Spacemap.Entities.Values.ToList())
            {
                RemoveCharacter(targetCharacter, user);
            }
            targetCharacter.Controller.StopController = true;
        }

        public void Deselect(Character targetCharacter)
        {
            if (targetCharacter == null)
                return;

            foreach (var entity in targetCharacter.Spacemap.Entities.ToList())
            {
                if (entity.Value.Selected != null && entity.Value.Selected == targetCharacter)
                {
                    if (entity.Value.Controller != null)
                    {
                        if (entity.Value.Controller.Attacking)
                        {
                            entity.Value.Controller.Attacking = false;
                        }
                    }

                    if (entity.Value is Player)
                    {
                        Packet.Builder.ShipSelectionCommand(World.StorageManager.GetGameSession(entity.Value.Id), null);
                        //World.StorageManager.GetGameSession(entity.Value.Id).Client.Send(Builder.ShipDeselectionCommand());
                    }

                    entity.Value.Selected = null;
                }
            }
        }

        public void Respawn()
        {
            Dead = false;
            StopController = false;
            Attacking = false;

            if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.Entities.Add(Character.Id, Character);

            if (Character is Npc)
            {
                var npc = (Npc)Character;
                npc.CurrentHealth = npc.MaxHealth;
                npc.CurrentShield = npc.MaxShield;
                npc.Position = Vector.Random(1000, 28000, 1000, 12000);

                npc.Controller.Restart();
            }
            else if (Character is Player)
            {
                var player = (Player)Character;
                player.CurrentHealth = 1000;

                if (player.Controller == null)
                    player.Controller = new PlayerController(Character);

                player.Controller.Start();

                var closestStation = player.GetClosestStation();
                player.Position = player.Destination = closestStation.Item1;
                player.Spacemap = closestStation.Item2;

                player.Refresh();
                player.Update();
            }

            Character.RangeEntities.Clear();
        }

        #endregion

        public void StopAll()
        {
            StopController = true;
            Attacking = false;
            Invincible = false;
        }

    }
}
