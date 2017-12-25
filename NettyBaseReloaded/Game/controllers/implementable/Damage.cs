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
            public Character Target { get; set; }
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

        public void Laser(Character character, int damage, bool absorb)
        {
            if (character == null || character.Controller.Dead || damage == 0) return;
            AddEntry(new DamageEntry {Absorb = absorb, Damage = damage, Target = character, Type = Types.LASER});
        }

        public void Rocket(Character character, int damage, bool absorb, Types type = Types.ROCKET)
        {
            if (character == null || character.Controller.Dead || damage == 0) return;
            AddEntry(new DamageEntry { Absorb = absorb, Damage = damage, Target = character, Type = type });
        }

        public void Radiation(Character character, int damage)
        {
            //TODO
        }

        private void DistributeDamage()
        {
            if (Entries.Count == 0) return;

            int totalDamage = 0;
            int totalAbsDamage = 0;
            Character target = null;
            Types damageType = Types.LASER;
            bool absorb = false;

            foreach (var entry in Entries)
            {
                if (target != null && entry.Value.Target != target)
                    continue;

                target = entry.Value.Target;
                if (entry.Value.Absorb) totalDamage += totalAbsDamage;
                else totalDamage += entry.Value.Damage;
                damageType = entry.Value.Type;
                RemoveEntry(entry.Key, entry.Value);
            }

            var attackerSession = (Character is Player) ? World.StorageManager.GetGameSession(Character.Id) : null;
            var pTarget = target as Player;
            var targetSession = pTarget?.GetGameSession();
            var pairedSessions = new List<GameSession>();

            if (attackerSession != null) pairedSessions.Add(attackerSession);
            if (targetSession != null) pairedSessions.Add(targetSession);

            if (Character.Controller.Invisible)
            {
                Character.Controller.Invisible = false;
                Character.Controller.Effects.UpdatePlayerVisibility();
            }

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

                    if (totalAbs > 0)
                    {
                        var _absDmg = (int)(totalAbs * totalAbs);
                        target.CurrentShield -= _absDmg;
                        Character.CurrentShield += _absDmg;
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

                #endregion DamageCalculations
            }
            else
            {
                totalDamage = 0;
                totalAbsDamage = 0;
            }

            foreach (var session in pairedSessions)
                Packet.Builder.AttackHitCommand(session, Character, target, totalDamage + totalAbsDamage, (short)damageType);

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
                if (entry.Value is Player) Packet.Builder.AttackHitCommand(World.StorageManager.GetGameSession(entry.Value.Id), Character, entry.Value, damage, 14);
                if (Character is Player) Packet.Builder.AttackHitCommand(World.StorageManager.GetGameSession(Character.Id), Character, entry.Value, damage, 14);
            }
        }
    }
}
