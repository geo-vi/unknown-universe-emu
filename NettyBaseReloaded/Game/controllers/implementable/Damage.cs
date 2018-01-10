using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;

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

        private ConcurrentDictionary<int, DamageEntry> Entries = new ConcurrentDictionary<int,DamageEntry>();
        private bool AddEntry(DamageEntry entry)
        {
            return Entries.TryAdd(Entries.Count, entry);
        }

        private bool RemoveEntry(int key, DamageEntry entry)
        {
            return Entries.TryRemove(key, out entry);
        }

        public Damage(AbstractCharacterController controller) : base(controller)
        {
            
        }

        private DateTime LastTick = new DateTime();
        public override void Tick()
        {
            if (LastTick.AddMilliseconds(200) < DateTime.Now)
            {
                DistributeDamage();
                LastTick = DateTime.Now;
            }
        }

        public override void Stop()
        {
        }

        public void Laser(IAttackable attacked, int damage, bool absorb)
        {
            if (attacked == null || attacked.EntityState == EntityStates.DEAD || damage == 0) return;
            AddEntry(new DamageEntry {Absorb = absorb, Damage = damage, Target = attacked, Type = Types.LASER});
        }

        public void Rocket(IAttackable attacked, int damage, bool absorb, Types type = Types.ROCKET)
        {
            if (attacked == null || attacked.EntityState == EntityStates.DEAD || damage == 0) return;
            AddEntry(new DamageEntry { Absorb = absorb, Damage = damage, Target = attacked, Type = type });
        }

        public void Radiation(Character character, int damage)
        {
            var player = character as Player;
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

            Character.Update();
            if (Character.CurrentHealth <= 0 && Character.EntityState != EntityStates.DEAD)
            {
                Controller.Destruction.Destroy();
            }
        }

        private void DistributeDamage()
        {
            if (Entries.Count == 0) return;

            int totalDamage = 0;
            int totalAbsDamage = 0;
            IAttackable target = null;
            Types damageType = Types.LASER;
            bool absorb = false;

            foreach (var entry in Entries)
            {
                if (target != null && entry.Value.Target != target)
                    continue;

                target = entry.Value.Target;
                if (entry.Value.Absorb) totalAbsDamage += entry.Value.Damage;
                else totalDamage += entry.Value.Damage;
                damageType = entry.Value.Type;
                RemoveEntry(entry.Key, entry.Value);
            }
            if (target == null) return;

            if (Character.Invisible)
            {
                Character.Invisible = false;
                Character.Controller.Effects.UpdatePlayerVisibility();
            }

            target.LastCombatTime = DateTime.Now; //To avoid repairing and logging off | My' own logging is set to off in the correspondent handlers

            if (!target.Invincible)
            {
                Entity(target, totalDamage, damageType, Character.Id, Character.ShieldPenetration, totalAbsDamage);
            }

            var player = Character as Player;
            if (player != null && target is Character)
            {
                var cTarget = (Character) target;

                player.State.InDemiZone = false;

                if (cTarget.Controller.Attack.Attackers.ContainsKey(player.Id))
                {
                    cTarget.Controller.Attack.Attackers[player.Id].Damage(totalDamage + totalAbsDamage);
                }
            }
            Character.Update();
        }

        public static void Entity(IAttackable target, int totalDamage, Types damageType, int attackerId = 0, double shieldPenetration = 1, int totalAbsDamage = 0, bool direct = false)
        {
            if (target == null) return;
            Character attacker = null;
            if (attackerId != 0 && target.Spacemap.Entities.ContainsKey(attackerId))
                attacker = target.Spacemap.Entities[attackerId];

            #region DamageCalculations

            if (target.CurrentShield > 0)
            {
                if (totalAbsDamage > 0)
                    totalDamage += totalAbsDamage;
                //For example => Target has 80% abs but you' have moth (+20% penetration) :> damage * 0.6

                var totalAbs = Math.Abs(shieldPenetration - target.ShieldAbsorption) + totalAbsDamage;

                if (totalAbs > 0)
                {
                    var _absDmg = (int)(totalAbs * totalAbs);
                    target.CurrentShield -= _absDmg;
                    if (attacker != null)
                        attacker.CurrentShield += _absDmg;
                }

                var shieldDamage = totalDamage * totalAbs;

                var healthDamage = 0;
                if (target.CurrentShield <= totalDamage)
                {
                    healthDamage = Math.Abs(target.CurrentShield - totalDamage);
                    target.CurrentShield = 0;
                    target.CurrentHealth -= healthDamage;
                }
                else
                {
                    target.CurrentShield -= (int)shieldDamage; //Correspondent shield damage
                    healthDamage = (int)(totalDamage * (1 - totalAbs));
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
                    target.CurrentHealth -= totalDamage; //Full dmg to health
            }

            #endregion

            foreach (var session in AssembleSelectedSessions(target))
                Packet.Builder.AttackHitCommand(session, attackerId, target, totalDamage, (short)damageType);

            if (target.CurrentHealth <= 0 && target.EntityState == EntityStates.ALIVE)
            {
                target.Destroy(attacker);
            }

            (target as Character)?.Update();
        }

        private static GameSession[] AssembleSelectedSessions(IAttackable target)
        {
            var hits = target.Spacemap.Entities.Where(x => x.Value is Player && x.Value.Selected == target).ToArray();
            var l = hits.Length;
            if (target is Player)
                l += 1;
            GameSession[] sessions = new GameSession[l];
            for (var i = 0; i < hits.Length; i++)
                sessions[i] = ((Player) hits[i].Value).GetGameSession();
            if (l > hits.Length) sessions[l - 1] = ((Player) target).GetGameSession();
            return sessions;
        }

        /// <summary>
        /// Causes a damage in area.
        /// </summary>
        public void Area(int amount, int distance = 0, DamageType damageType = DamageType.DEFINED)
        {
            if (distance == 0) distance = Controller.Attack.AttackRange;

            foreach (var entry in Character.Spacemap.Entities)
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
                if (entry.Value is Player) Packet.Builder.AttackHitCommand(World.StorageManager.GetGameSession(entry.Value.Id), Character.Id, entry.Value, damage, 14);
                if (Character is Player) Packet.Builder.AttackHitCommand(World.StorageManager.GetGameSession(Character.Id), Character.Id, entry.Value, damage, 14);
            }
        }
    }
}
