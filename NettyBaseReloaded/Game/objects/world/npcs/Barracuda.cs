using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.npcs
{
    class Barracuda : Npc
    {
        public Barracuda(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int currentHealth, int currentNanoHull, int maxShield, int damage, int respawnTime = 0, bool respawning = true, Npc motherShip = null) : base(id, name, hangar, factionId, position, spacemap, currentHealth, currentNanoHull, maxShield, damage, respawnTime, respawning, motherShip)
        {
        }

        public override void Destroy()
        {
            Controller.Damage.Area(20, controllers.implementable.Damage.Types.KAMIKAZE, 500, true, DamageType.PERCENTAGE);
            base.Destroy();
        }
    }
}
