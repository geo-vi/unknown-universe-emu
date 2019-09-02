using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.npcs
{
    class Cubikon : Npc
    {
        /// <summary>
        /// All the Protegits which are getting spawned
        /// </summary>
        public ConcurrentDictionary<int, Npc> Children = new ConcurrentDictionary<int, Npc>();

        /// <summary>
        /// Respawn helper (will respawn every 90 secs)
        /// </summary>
        public DateTime LastDeathTime;

        public Cubikon(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int maxHealth, int currentNanoHull,
            int maxShield, int currentShield, int currentHealth, int damage, Reward reward, int respawnTime = 0, bool respawning = true, Npc motherShip = null) : base(id, name, hangar, factionId, position, spacemap, maxHealth, currentNanoHull, maxShield, currentShield, currentHealth, damage, reward, respawnTime, respawning, motherShip)
        {
        }

        public override void Destroy(Character destroyer)
        {
            base.Destroy(destroyer);
            foreach (var child in Children)
            {
                child.Value.MotherShip = null;
            }
        }
    }
}
