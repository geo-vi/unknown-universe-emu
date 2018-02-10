using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map
{
    class Wave
    {
        public class Npc
        {
            /// <summary>
            /// Base ship
            /// </summary>
            public Ship Ship { get; set; }
            /// <summary>
            /// Health multiplyer
            /// </summary>
            public double HealthMultiplyer { get; set; }
            /// <summary>
            /// Adding some sort of character at the end of the npc's name
            /// </summary>
            public string NamePrefix { get; set; }
            /// <summary>
            ///  This will make the NPC unkilleable until all others are dead
            /// </summary>
            public bool Invincible { get; set; }

            public Npc(Ship ship, double healthMultiplyer, string namePrefix, bool invincible = false)
            {
                Ship = ship;
                HealthMultiplyer = healthMultiplyer;
                NamePrefix = namePrefix;
                Invincible = invincible;
            }
        }

        public int Id { get; set; }

        public List<Npc> Npcs { get; set; }

        public Vector Position { get; set; }

        public Wave(int id, List<Npc> npcs, Vector position)
        {
            Id = id;
            Npcs = npcs;
        }

        public static List<Npc> CreateWave(Npc npc, int count)
        {
            List<Npc> waveNpcs = new List<Npc>();
            for (int i = 0; i < count; i++)
            {
                waveNpcs.Add(npc);
            }
            return waveNpcs;
        }

        public void Create(Spacemap spacemap)
        {
            foreach (var npc in Npcs)
            {
                spacemap.CreateNpc(npc.Ship, AILevels.AGGRESSIVE, false, 0, Position);
            }
        }

        internal void Create(Spacemap virtualMap, Vector vector, int r)
        {
            //TODO: Create with radius from vector
        }
    }
}
