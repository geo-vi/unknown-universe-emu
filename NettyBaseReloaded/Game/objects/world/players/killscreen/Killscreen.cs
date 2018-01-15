using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.killscreen
{
    class Killscreen
    {
        /// <summary>
        /// If user connects for the 2nd time we should send him the option to repair at base only
        /// Otherwise if player is fresh killed (didn't disconnect since death) we just send him the killscreen with the killer's details.
        /// </summary>
        // TODO: Recode it

        public int Id { get; set; }

        public string KillerName { get; set; }
        public string KillerLink { get; set; }

        public DeathType DeathType { get; set; }

        public string Alias => "MISC"; // TEMPORARY until we find out what it is

        public DateTime TimeOfDeath { get; set; }

        public Killscreen()
        {
            
        }

        public static Killscreen Load(Player killedPlayer)
        {
            return World.DatabaseManager.GetLastKillscreen(killedPlayer.Id);
        }
    }
}
