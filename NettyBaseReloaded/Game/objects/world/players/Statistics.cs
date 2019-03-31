using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.players.statistics;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Statistics : PlayerBaseClass
    {
        /// <summary>
        /// Killed entities
        /// </summary>
        public Dictionary<int, int> SHIPS_KILLED { get; set; }

        /// <summary>
        /// Deaths
        /// </summary>
        public List<DeathStat> DEATHS { get; set; }

        /// <summary>
        /// Spent Ammunitions
        /// </summary>
        public Dictionary<string, int> SPENT_AMMO { get; set; }

        /// <summary>
        /// Damage dealt to all sort of entities
        /// </summary>
        public int TOTAL_DAMAGE_DEALT { get; set; }

        public int COLLECTABLES_COLLECTED { get; set; }

        public int COLLECTED_PIXELBOXES = 0;

        /// <summary>
        /// Average movement per minute
        /// </summary>
        public double AVG_MOVEREQUESTS_PER_MIN { get; set; }

        /// <summary>
        /// Averege session lenght
        /// </summary>
        public int AVG_GAMESESSION_LENGHT { get; set; }

        private int AVG_DISTANCE_FROM_NPC { get; set; }

        public DateTime LAST_TIME_CONNECTED { get; set; }

        private TimeSpan ConnectedTime => DateTime.Now - Player.GetGameSession().SessionStartTime;

        public Statistics(Player player) : base(player)
        {
            AVG_DISTANCE_FROM_NPC = 500;
            SHIPS_KILLED = new Dictionary<int, int>();
            DEATHS = new List<DeathStat>();
            SPENT_AMMO = new Dictionary<string, int>();
        }

        public void CollectBox(bool isPixelBox = false)
        {
            if (isPixelBox) COLLECTED_PIXELBOXES++;
            else COLLECTABLES_COLLECTED++;
        }

        public void AddKill(Character target, int damageDealt = 0, DateTime attackStartTime = new DateTime())
        {
            var ship = target.Hangar.Ship;
            if (target is Npc npc)
            {
                KillNpc(npc, damageDealt, attackStartTime);
            }

            if (SHIPS_KILLED.ContainsKey(ship.Id))
            {
                SHIPS_KILLED[ship.Id]++;
            }
            else
            {
                SHIPS_KILLED.Add(ship.Id, 1);
            }
        }

        private void KillNpc(Npc npcKilled, int damageDealt = 0, DateTime attackStartTime = new DateTime())
        {
            var distanceFromNpc = npcKilled.Position.DistanceTo(Player.Position);
            AVG_DISTANCE_FROM_NPC = (int)((AVG_DISTANCE_FROM_NPC + distanceFromNpc) / 2);
            World.DatabaseManager.AddPlayerLog(Player, PlayerLogTypes.NORMAL,
                "Killed NPC " + npcKilled.Name + " and you have dealt " + damageDealt +
                " damage to it with attack starting at " + attackStartTime + " and finishing it off at " + Math.Round(distanceFromNpc) + " meters away!");
        }
    }
}
