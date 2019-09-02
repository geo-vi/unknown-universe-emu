using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Damage : IAbstractCharacter
    {
        public enum Types
        {
            ROCKET = 0,
            LASER = 1,
            MINE = 2,
            RADIATION = 3,
            PLASMA = 4,
            ECI = 5,
            SL = 6,
            CID = 7,
            SINGULARITY = 8,
            KAMIKAZE = 9,
            REPAIR = 10,
            DECELERATION = 11,
            SHIELD_ABSORBER_ROCKET_CREDITS = 12,
            SHIELD_ABSORBER_ROCKET_URIDIUM = 13,
            STICKY_BOMB = 14
        }
        private class DamageEntry
        {
            public bool Absorb { get; set; }
            public int Damage { get; set; }
            public Types Type { get; set; }
            public IAttackable Target { get; set; }
        }

        private ConcurrentQueue<DamageEntry> Entries = new ConcurrentQueue<DamageEntry>();
        private void AddEntry(DamageEntry entry)
        {
            Entries.Enqueue(entry);
        }

        private bool RemoveEntry(out DamageEntry entry)
        {
            return Entries.TryDequeue(out entry);
        }

        public Damage(AbstractCharacterController controller) : base(controller)
        {
            
        }

        public override void Tick()
        {
            //DistributeDamage();
        }

        public override void Stop()
        {
        }

        public void Laser(IAttackable attacked, int damage, int absorb)
        {
            if (attacked == null || attacked.EntityState == EntityStates.DEAD) return;
            DistributeDamage(Types.LASER, damage, absorb, attacked);
            //AddEntry(new DamageEntry {Absorb = absorb, Damage = damage, Target = attacked, Type = Types.LASER});
        }

        public void Rocket(IAttackable attacked, int damage, int absorb, Types type = Types.ROCKET)
        {
            if (attacked == null || attacked.EntityState == EntityStates.DEAD) return;
            DistributeDamage(Types.ROCKET, damage, absorb, attacked);
            //AddEntry(new DamageEntry { Absorb = absorb, Damage = damage, Target = attacked, Type = type });
        }

        public void Radiation(int damage)
        {
            var player = Character as Player;
            if (player == null) return;
            if (player.CurrentNanoHull > 0)
            {
                //If the player can receive some damage on the nanohull
                if (player.CurrentNanoHull - damage < 0)
                {
                    var nanoDamage = damage - player.CurrentNanoHull;
                    player.CurrentNanoHull = 0;
                    player.CurrentHealth -= nanoDamage;
                }
                else
                    player.CurrentNanoHull -= damage;
            }
            else
                player.CurrentHealth -= damage;

            Packet.Builder.AttackHitCommand(player.GetGameSession(), 0, player, damage, (short)Types.RADIATION);

            player.Updaters.Update();
            if (player.CurrentHealth <= 0 && player.EntityState != EntityStates.DEAD)
            {
                Controller.Destruction.Destroy(player, DeathType.RADITATION);
            }
        }

        public void ECI(int distance = 1000)
        {
            var player = Character as Player;
            var targets = new Dictionary<int, Character>();

            foreach (var entry in player.Spacemap.Entities)
            {
                if(entry.Value != null)
                    if (entry.Value != player && entry.Value != player.Pet)
                        if (player.Position.DistanceTo(entry.Value.Position) < distance && entry.Value.FactionId != player.FactionId)
                            if (targets.Count < 7)
                                targets.Add(entry.Value.Id, entry.Value);
            }

            foreach (var target in targets)
            {
                if (target.Value == null) return;

                var damage = RandomizeDamage(10000);

                string eciPacket = "0|TX|ECI||" + player.Id;
                eciPacket += "|" + target.Value.Id;

                target.Value.CurrentHealth -= damage;
                target.Value.LastCombatTime = DateTime.Now;
                GameClient.SendToPlayerView(player, netty.commands.old_client.LegacyModule.write(eciPacket), true);
                if (target.Value is Player) Packet.Builder.AttackHitCommand(player.GetGameSession(), player.Id, target.Value, damage, (short)Types.ECI);
                Packet.Builder.AttackHitCommand(player.GetGameSession(), player.Id, target.Value, damage, (short)Types.ECI);
            }

            foreach (var target in targets)
            {
                if (target.Value.CurrentHealth <= 0 && target.Value.EntityState != EntityStates.DEAD)
                    Controller.Destruction.Destroy(target.Value, DeathType.PLAYER);
            }

            targets.Clear();
        }

        private void DistributeDamage(Types damageType, int totalDamage, int totalAbsDamage, IAttackable target)
        {
            //if (Entries.Count == 0) return;

            //int totalDamage = 0;
            //int totalAbsDamage = 0;
            //IAttackable target = null;
            //var damageType = Types.LASER;

            //for (int i = 0; i < Entries.Count; i++)
            //{
            //    DamageEntry entry;
            //    if (Entries.TryPeek(out entry))
            //    {
            //        if (target != null && entry.Target != target)
            //            break;
            //    }

            //    if (RemoveEntry(out entry))
            //    {
            //        target = entry.Target;
            //        if (entry.Absorb) totalAbsDamage += entry.Damage;
            //        else totalDamage += entry.Damage;
            //        damageType = entry.Type;
            //    }
            //    else return;
            //}

            lock (DamageLock)
            {

                if (target == null) return;

                if (Character.Invisible)
                {
                    Character.Controller.Effects.Uncloak();
                }

                target.LastCombatTime =
                    DateTime.Now; //To avoid repairing and logging off | My' own logging is set to off in the correspondent handlers

                if (!target.Invincible && target.EntityState != EntityStates.DEAD)
                {
                    Entity(target, totalDamage, damageType, Character.Id, Character.ShieldPenetration, totalAbsDamage);
                }

                if (Character is Player player && target is Character cTarget)
                {
                    player.State.InDemiZone = false;

                    if (cTarget.Controller.Attack.Attackers.ContainsKey(player.Id))
                    {
                        cTarget.Controller.Attack.Attackers[player.Id].Damage(totalDamage + totalAbsDamage);
                    }
                }

                Character.Updaters.Update();
            }
        }

        private static object DamageLock = new object();
        public static void Entity(IAttackable target, int totalDamage, Types damageType, int attackerId = 0, double shieldPenetration = 1, int totalAbsDamage = 0, bool direct = false)
        {
            lock (DamageLock)
            {
                if (target == null) return;
                if (totalAbsDamage == 0 && totalDamage == 0)
                {
                    foreach (var session in AssembleSelectedSessions(target))
                        Packet.Builder.AttackHitCommand(session, attackerId, target, 0, (short) damageType);
                    return;
                }

                Character attacker = null;
                if (attackerId != 0 && target.Spacemap.Entities.ContainsKey(attackerId))
                    attacker = target.Spacemap.Entities[attackerId];

                #region DamageCalculations

                if (target.CurrentShield > 0)
                {
                    //if (totalAbsDamage > 0)
                    //    totalDamage += totalAbsDamage;
                    //For example => Target has 80% abs but you' have moth (+20% penetration) :> damage * 0.6

                    var totalAbs = Math.Abs(shieldPenetration - target.ShieldAbsorption);

                    if (totalAbs > 0)
                    {
                        var _absDmg = (int) (totalAbs * totalAbsDamage);
                        target.CurrentShield -= _absDmg;
                        if (target.CurrentShield < 0) target.CurrentShield = 0;
                        if (attacker != null)
                            attacker.CurrentShield += _absDmg;
                    }

                    var shieldDamage = totalDamage * totalAbs;
                    if (target is Player targetedPlayer && targetedPlayer.Storage.SentinelFortressActive)
                    {
                        shieldDamage *= 0.7;
                    }

                    var healthDamage = 0;
                    if (target.CurrentShield <= totalDamage)
                    {
                        healthDamage = Math.Abs(target.CurrentShield - totalDamage);
                        target.CurrentShield = 0;
                        target.CurrentHealth -= healthDamage;
                    }
                    else
                    {
                        target.CurrentShield -= (int) shieldDamage; //Correspondent shield damage
                        healthDamage = (int) (totalDamage * (1 - totalAbs));
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
                    totalAbsDamage = 0;

                    if (target.CurrentNanoHull > 0)
                    {
                        //If the player can receive some damage on the nanohull
                        if (target.CurrentNanoHull - totalDamage < 0)
                        {
                            var nanoDamage = totalDamage - target.CurrentNanoHull;
                            target.CurrentNanoHull = 0;
                            target.CurrentHealth -= nanoDamage;
                        }
                        else
                            target.CurrentNanoHull -= totalDamage; //Full dmg to nanohull
                    }
                    else
                    {
                        target.CurrentHealth -= totalDamage; //Full dmg to health
                    }
                }

                #endregion

                target.Hit(totalDamage, attackerId);

                foreach (var session in AssembleSelectedSessions(target))
                {
                    if (session != null)
                        Packet.Builder.AttackHitCommand(session, attackerId, target, totalDamage + totalAbsDamage,
                            (short) damageType);
                }

                if (target is AttackableAssetCore)
                    foreach (var session in target.Spacemap.Entities.Values.Where(x => x is Player))
                    {
                        var player = session as Player;
                        var gameSession = player?.GetGameSession();
                        if (gameSession != null)
                            Packet.Builder.AttackHitAssetCommand(gameSession, target.Id, target.CurrentHealth,
                                target.MaxHealth);
                    }

                if (target.CurrentHealth <= 0 && target.EntityState == EntityStates.ALIVE)
                {
                    target.Destroy(attacker);
                }

                (target as Character)?.Updaters.Update();
            }
        }

        private static GameSession[] AssembleSelectedSessions(IAttackable target)
        {
            var hits = target.Spacemap.Entities.Where(x => x.Value is Player && x.Value.Selected == target);
            var sessions = new List<GameSession>();
            if (target is Player pTarget)
            {
                var tSession = pTarget.GetGameSession();
                if (tSession != null) sessions.Add(tSession);
            }
            foreach (var hit in hits)
            {
                if (hit.Value is Player player)
                {
                    var session = player.GetGameSession();
                    if (session == null) continue;
                    sessions.Add(session);
                }
            }
            return sessions.ToArray();
        }

        /// <summary>
        /// Causes a damage in area.
        /// </summary>
        public void Area(int amount, Types attackType, int distance = 0, bool playerOnly = false,
            DamageType damageType = DamageType.DEFINED)
        {
            if (distance == 0) distance = Character.AttackRange;

            var pet = Character as Pet;

            foreach (var entry in Character.Spacemap.Entities)
            {
                if (playerOnly && !(entry.Value is Player)) return;

                if (Character.Position.DistanceTo(entry.Value.Position) > distance) continue;
                if (Character.Id == entry.Value.Id) continue;

                if (pet != null && pet.GetOwner() == entry.Value) continue;


                var damage = 0;
                switch (damageType)
                {
                    case DamageType.DEFINED:
                        damage = amount;
                        break;
                    case DamageType.PERCENTAGE:
                        damage = (int) (entry.Value.CurrentHealth * amount / 100.0);
                        break;
                }
                //TODO use Damage() method instead

                entry.Value.CurrentHealth -= damage;
                entry.Value.LastCombatTime = DateTime.Now;

                foreach (var session in AssembleSelectedSessions(entry.Value))
                {
                    Packet.Builder.AttackHitCommand(session, Character.Id, entry.Value, damage, (short) attackType);
                }

                if (entry.Value.CurrentHealth <= 0 && entry.Value.EntityState == EntityStates.ALIVE)
                {
                    if (pet != null)
                    {
                        entry.Value.Destroy(pet.GetOwner());
                    }
                    else entry.Value.Destroy(Character);
                }

                entry.Value.Updaters.Update();
            }
        }

        public static void Area(Spacemap map, Vector center, int radius, int amount, Types attackType, DamageType damageType = DamageType.DEFINED, bool playersOnly = true)
        {
            foreach (var entry in map.Entities.Where(x => x.Value.Position.DistanceTo(center) <= radius))
            {
                if (entry.Value is Player && playersOnly || !playersOnly)
                {

                    if (entry.Value.Invincible) continue;
                    var damage = 0;
                    switch (damageType)
                    {
                        case DamageType.DEFINED:
                            damage = amount;
                            break;
                        case DamageType.PERCENTAGE:
                            damage = (int) (entry.Value.CurrentHealth * amount / 100.0);
                            break;
                    }
                    //TODO use Damage() method instead

                    entry.Value.CurrentHealth -= damage;
                    entry.Value.LastCombatTime = DateTime.Now;

                    foreach (var session in AssembleSelectedSessions(entry.Value))
                    {
                        Packet.Builder.AttackHitCommand(session, 0, entry.Value, damage, (short) attackType);
                    }

                    if (entry.Value.CurrentHealth <= 0 && entry.Value.EntityState == EntityStates.ALIVE)
                    {
                        entry.Value.Destroy();
                    }

                    entry.Value.Updaters.Update();
                }
            }
        }

        private int RandomizeDamage(int damage)
        {
            if (damage <= 0)
                return 0;

            int max = damage + 1000;
            int min = damage - 1000;

            if (min < 0)
                min = 0;

            var random = RandomInstance.getInstance(this);
            int calculatedDamage = random.Next(max - min) + min;
            return calculatedDamage;
        }
    }
}
