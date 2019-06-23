using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects;
using Console = System.Console;

namespace NettyBaseReloaded.Game.objects.world.npcs
{
    class Spaceball : Npc
    {
        /// <summary>
        /// Overriding max and current health so no reductions will be made.
        /// </summary>
        public override int CurrentHealth => 999999999;

        public override int MaxHealth => 999999999;

        public override int RenderRange => -1;

        /// <summary>
        /// Each company hitting harder than the other one
        /// </summary>
        public int MMOHitDamage = 0;

        public int EICHitDamage = 0;

        public int VRUHitDamage = 0;

        public Faction LeadingFaction;
        public int MovingSpeed; // will be done from controller

        public override int Speed
        {
            get
            {
                var speed = Hangar.Ship.Speed;
                return speed * MovingSpeed; 
            }
        }

        public int MMOScore = 0;

        public int EICScore = 0;

        public int VRUScore = 0;

        public Spaceball(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int maxHealth, int currentNanoHull,
            int maxShield, int currentShield, int currentHealth, int damage, Reward reward, int respawnTime = 0, bool respawning = true, Npc motherShip = null) : base(id, name, hangar, factionId, position, spacemap, maxHealth, currentNanoHull, maxShield, currentShield, currentHealth, damage, reward, respawnTime, respawning, motherShip)
        {
        }

        public override void Tick()
        {
            Calculate();
        }

        public void Calculate()
        {
            var mmoDamage = 0;
            Character tempCharacter = null;
            foreach (var nigga in mmoNiggas.Values)
            {
                if (IsNigga(nigga))
                {
                    mmoDamage += nigga.Damage;
                }
                else mmoNiggas.TryRemove(nigga.Id, out tempCharacter);
            }

            var eicDamage = 0;
            foreach (var nigga in eicNiggas.Values)
            {
                if (IsNigga(nigga))
                {
                    eicDamage += nigga.Damage;
                }
                else eicNiggas.TryRemove(nigga.Id, out tempCharacter);
            }

            var vruDamage = 0;
            foreach (var nigga in vruNiggas.Values)
            {
                if (IsNigga(nigga))
                {
                    vruDamage += nigga.Damage;
                }
                else vruNiggas.TryRemove(nigga.Id, out tempCharacter);
            }

            MMOHitDamage = mmoDamage;
            EICHitDamage = eicDamage;
            VRUHitDamage = vruDamage;
        }

        public bool IsNigga(Character nigga) => nigga.Controller.Active && nigga.Controller.Attack.Attacking &&
                                                nigga.Selected == this && LastCombatTime.AddSeconds(2) > DateTime.Now;

        public void WipeDamage()
        {
            MMOHitDamage = 0;
            EICHitDamage = 0;
            VRUHitDamage = 0;
        }

        private ConcurrentDictionary<int, Character> mmoNiggas = new ConcurrentDictionary<int, Character>();
        private ConcurrentDictionary<int, Character> eicNiggas = new ConcurrentDictionary<int, Character>();
        private ConcurrentDictionary<int, Character> vruNiggas = new ConcurrentDictionary<int, Character>();

        public override void Hit(int totalDamage, int attackerId)
        {
            if (Spacemap.Entities.ContainsKey(attackerId))
            {
                Character character;
                var attacker = Spacemap.Entities[attackerId];
                switch (attacker.FactionId)
                {
                    case Faction.MMO:
                        character = GetCharacter(attackerId);
                        if (!mmoNiggas.ContainsKey(attackerId)) mmoNiggas.TryAdd(attackerId, character);
                        break;
                    case Faction.EIC:
                        character = GetCharacter(attackerId);
                        if (!eicNiggas.ContainsKey(attackerId)) eicNiggas.TryAdd(attackerId, character);
                        break;
                    case Faction.VRU:
                        character = GetCharacter(attackerId);
                        if (!vruNiggas.ContainsKey(attackerId)) vruNiggas.TryAdd(attackerId, character);
                        break;
                }
            }
        }

        public Character GetCharacter(int id)
        {
            return Spacemap.Entities[id];
        }

        public void Score(Faction faction, Jumpgate portal)
        {
            switch (faction)
            {
                case Faction.MMO:
                    MMOScore++;
                    foreach (var session in World.StorageManager.GameSessions.Values)
                    {
                        if (session == null || !session.Player.Controller.Active) continue;
                        Packet.Builder.SpaceBallUpdateScoreCommand(session, faction, MMOScore, portal.Id);
                    }
                    break;
                case Faction.EIC:
                    EICScore++;
                    foreach (var session in World.StorageManager.GameSessions.Values)
                    {
                        if (session == null || !session.Player.Controller.Active) continue;
                        Packet.Builder.SpaceBallUpdateScoreCommand(session, faction, EICScore, portal.Id);
                    }
                    break;
                case Faction.VRU:
                    VRUScore++;
                    foreach (var session in World.StorageManager.GameSessions.Values)
                    {
                        if (session == null || !session.Player.Controller.Active) continue;
                        Packet.Builder.SpaceBallUpdateScoreCommand(session, faction, VRUScore, portal.Id);
                    }
                    break;
            }

            for (var i = 0; i < 50; i++)
            {
                Spacemap.CreateLootBox(Vector.GetPosOnCircle(portal.Position, 50 * i), new Reward(RewardType.URIDIUM, 750), Types.BIG_PUMPKIN, 25000);
            }
        }
    }
}
