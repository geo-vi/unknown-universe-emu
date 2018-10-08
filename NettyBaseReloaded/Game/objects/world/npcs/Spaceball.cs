using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.npcs
{
    class Spaceball : Npc
    {
        /// <summary>
        /// Overriding max and current health so no reductions will be made.
        /// </summary>
        public override int CurrentHealth => 999999999;

        public override int MaxHealth => 999999999;

        /// <summary>
        /// Each company hitting harder than the other one
        /// </summary>
        public int MMOHitDamage = 0;
        public List<int> MMOAttackers = new List<int>();

        public int EICHitDamage = 0;
        public List<int> EICAttackers = new List<int>();

        public int VRUHitDamage = 0;
        public List<int> VRUAttackers = new List<int>();

        public Faction LeadingFaction;
        public int MovingSpeed; // will be done from controller

        public Spaceball(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int currentHealth, int currentNanoHull, Reward rewards, int maxShield, int damage, int respawnTime = 0, bool respawning = true, Npc motherShip = null) : base(id, name, hangar, factionId, position, spacemap, currentHealth, currentNanoHull, rewards, maxShield, damage, respawnTime, respawning, motherShip)
        {
        }



        public override void Hit(int totalDamage, int attackerId)
        {
            if (Spacemap.Entities.ContainsKey(attackerId))
            {
                var attacker = Spacemap.Entities[attackerId];
                switch (attacker.FactionId)
                {
                    case Faction.MMO:
                        MMOHitDamage += totalDamage;
                        MMOAttackers.Add(attackerId);
                        break;
                    case Faction.EIC:
                        break;
                    case Faction.VRU:
                        break;
                }
            }
        }
    }
}
