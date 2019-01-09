using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.npcs
{
    class EventNpc : Npc
    {
        public EventNpc(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int currentHealth, int currentNanoHull, Reward rewards, int maxShield, int damage, int respawnTime = 0, bool respawning = true, Npc motherShip = null) : base(id, name, hangar, factionId, position, spacemap, currentHealth, currentNanoHull, rewards, maxShield, damage, respawnTime, false, motherShip)
        {
        }
        
        public void Announce()
        {
            var announcement = GetAnnouncement();
            if (announcement == "") return;
            foreach (var session in GameSession.GetRangeSessions(this))
            {
                Packet.Builder.LegacyModule(session.Value, "0|n|MSG|1|1|" + announcement);
            }
        }

        public void PrivateAnnouncement(GameSession gameSession)
        {
            var announcement = GetAnnouncement();
            if (announcement == "") return;

            Packet.Builder.LegacyModule(gameSession, "0|n|MSG|1|1|" + announcement);
        }

        private string GetAnnouncement()
        {
            switch (Hangar.Ship.Id)
            {
                case 104:
                    return "msg_alien1100101_spawned_proximity";
                default:
                    return "";
            }
        }
    }
}
