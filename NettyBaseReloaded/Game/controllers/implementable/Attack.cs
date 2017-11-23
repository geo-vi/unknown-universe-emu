using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Attack : IAbstractCharacter
    {
        public bool Attacking { get; set; }

        public bool Attacked { get; set; }

        public bool Invincible { get; set; }

        public bool Targetable { get; set; }

        public int AttackRange = 700;

        public DateTime LastTimeAttacked = new DateTime();

        public Attack(AbstractCharacterController controller) : base(controller)
        {
            Targetable = true;            
        }

        public override void Tick()
        {
            if (Attacking)
                LaserAttack();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public Character GetAttacker()
        {
            foreach (var entity in Character.Range.Entities.Values.ToList())
            {
                if (entity.Controller.Dead) return null;
                if (entity.Selected == Character && entity.Controller.Attack.Attacking)
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
            if (Controller.Dead || Controller.StopController || enemy == null || !Attacking) return;
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
                pCharacter?.SendLogMessage("outofrange");
                return;
            }

            var damage = Character.Damage;
            var absDamage = 0; //This variable will be used for ammo that absobrs shield too

            var laserColor = 0;

            if (Character is Player)
            {
                var gameSession = World.StorageManager.GetGameSession(Character.Id);
                if (gameSession.Player.Equipment.LaserCount() == 0)
                {
                    Attacking = false; // Will stop attacking if there are no lasers equipped.
                    Packet.Builder.LegacyModule(gameSession, "0|A|STM|no_lasers_on_board");
                    return;
                }
                if (gameSession.Player.Settings.CurrentAmmo.Shoot() == 0)
                {
                    // NOTHING TO SHOOT
                    Packet.Builder.LegacyModule(gameSession, "0|A|STD|No more ammo (todo: find a proper STM message)");
                    Attacking = false;
                    return;
                }

                var pEnemy = enemy as Player;
                if (pEnemy != null)
                {
                    if (pEnemy.State.InDemiZone)
                        return;
                }

                bool isRsb = false;
                switch (gameSession.Player.Settings.CurrentAmmo.LootId)
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
            else if (Character is Pet)
            {
                
            }

            damage = RandomizeDamage(damage);
            GameClient.SendRangePacket(Character,
                netty.commands.old_client.AttackLaserRunCommand.write(Character.Id, enemy.Id, laserColor, false,
                    true), true);
            GameClient.SendRangePacket(Character,
                netty.commands.new_client.AttackLaserRunCommand.write(Character.Id, enemy.Id, laserColor, false,
                    true), true);

            Damage(enemy, absDamage, damage, 1);

            enemy.Controller.Attack.Attacked = true;
            enemy.Controller.Attack.LastTimeAttacked = DateTime.Now;
            LastLaserLoop = DateTime.Now;
        }

        private DateTime LastMissleLoop = new DateTime();
        public void LaunchMissle(string missleId)
        {
            var enemy = Character.Selected;
            if (enemy == null)
                return;

            var player = Character as Player;
            GameSession gameSession = null;
            if (player != null) gameSession = World.StorageManager.GetGameSession(player.Id);
            if (!Character.InRange(enemy, AttackRange))
            {
                if (player != null)
                {
                    if (LastMissleLoop.AddSeconds(1) < DateTime.Now)
                    {
                        Packet.Builder.LegacyModule(gameSession, "0|A|STM|outofrange");
                    }
                }
                return;
            }

            if (player?.Settings.CurrentRocket.Shoot() == 0)
            {
                // NOTHING TO SHOOT
                Packet.Builder.LegacyModule(gameSession,
                    "0|A|STD|No more ammo (todo: find a proper STM message)");
                Attacking = false;
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
            if (player != null) newCooldown.Send(World.StorageManager.GetGameSession(player.Id));
            Character.Cooldowns.Add(newCooldown);

            GameClient.SendRangePacket(Character, netty.commands.old_client.LegacyModule.write("0|v|" + Character.Id + "|" + enemy.Id + "|H|" + rocketId + "|0|0"), true);
            GameClient.SendRangePacket(Character, netty.commands.new_client.LegacyModule.write("0|v|" + Character.Id + "|" + enemy.Id + "|H|" + rocketId + "|0|0"), true);
            Damage(enemy, 0, damage, 1);

            enemy.Controller.Attack.Attacked = true;
            enemy.Controller.Attack.LastTimeAttacked = DateTime.Now;
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
                    return (int)(baseDmg * 1.10);
                case 1:
                    return (int)(baseDmg * 0.98);
                case 2:
                    return (int)(baseDmg * 1.02);
                case 3:
                    return 0;
                case 4:
                    return (int)(baseDmg * 0.92);
                case 5:
                    return (int)(baseDmg * 0.99);
                default:
                    return baseDmg;
            }
        }

        public void Damage(Character target, int absDamage, int damage, short damageEffect)
        {
            if (target.Controller.Dead || Character.Controller.Dead) return;
            var attackerSession = (Character is Player) ? World.StorageManager.GetGameSession(Character.Id) : null;
            var targetSession = (target is Player) ? World.StorageManager.GetGameSession(target.Id) : null;
            var pairedSessions = new List<GameSession>();
            if (attackerSession != null) pairedSessions.Add(attackerSession);
            if (targetSession != null) pairedSessions.Add(targetSession);

            foreach (var entry in target.Range.Entities.Where(x => x.Value.Selected == target && x.Value is Player))
            {
                pairedSessions.Add(World.StorageManager.GetGameSession(entry.Key));
            }

            target.LastCombatTime = DateTime.Now; //To avoid repairing and logging off | My' own logging is set to off in the correspondent handlers

            if (!target.Controller.Attack.Invincible)
            {
                #region DamageCalculations

                if (target.CurrentShield > 0)
                {
                    //For example => Target has 80% abs but you' have moth (+20% penetration) :> damage * 0.6

                    var totalAbs = Math.Abs(Character.ShieldPenetration - target.ShieldAbsorption);

                    if (absDamage > 0)
                    {
                        var _absDmg = (int)(absDamage * totalAbs);
                        target.CurrentShield -= _absDmg;
                        Character.CurrentShield += _absDmg;
                    }

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

            foreach (var session in pairedSessions)
                Packet.Builder.AttackHitCommand(session, Character, target, damage + absDamage, damageEffect);

            if (target.CurrentHealth <= 0 && !target.Controller.Dead)
            {
                Controller.Destruction.Destroy(target);
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
                        damage = entry.Value.CurrentHealth * amount / 100;
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
                    newAmount = Character.CurrentHealth + amount;
                    Character.CurrentHealth += newAmount;
                    break;
                case HealType.SHIELD:
                    newAmount = Character.CurrentHealth + amount;
                    Character.CurrentShield = newAmount;
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
    }
}
