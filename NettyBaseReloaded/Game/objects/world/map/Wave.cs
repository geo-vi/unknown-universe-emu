using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using Newtonsoft.Json;

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
            public double Multiplier { get; set; }
            /// <summary>
            /// Adding some sort of character at the end of the npc's name
            /// </summary>
            public string NamePrefix { get; set; }
            /// <summary>
            ///  This will make the NPC unkilleable until all others are dead
            /// </summary>
            public bool Invincible { get; set; }

            public Npc(Ship ship, double multiplier, string namePrefix, bool invincible = false)
            {
                Ship = ship;
                Multiplier = multiplier;
                NamePrefix = namePrefix;
                Invincible = invincible;
            }
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("npcs_left")]
        public List<Npc> Npcs { get; set; }

        [JsonProperty("pos")]
        public Vector Position { get; set; }

        public Wave(int id, List<Npc> npcs, Vector position)
        {
            Id = id;
            Npcs = npcs;
            Position = position;
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

        public void Create(Spacemap spacemap, int vwId)
        {
            foreach (var npc in Npcs)
            {
                spacemap.CreateNpc(npc.Ship.Strengthen(npc.Multiplier), AILevels.GALAXY_GATES, false, 0, Position, 0, npc.NamePrefix + " " + Id, npc.Ship.Reward.Multiply(npc.Multiplier));
            }
        }

        internal void Create(Spacemap spacemap, int vwId, Vector vector, int r)
        {
            var random = RandomInstance.getInstance(this);
            foreach (var npc in Npcs)
            {
                spacemap.CreateNpc(npc.Ship, AILevels.GALAXY_GATES, false, 0, Vector.GetPosOnCircle(vector, random.Next(0,360), r), 0, npc.NamePrefix + " " + (Id + 1), npc.Ship.Reward.Multiply(npc.Multiplier));
            }
        }
    }
}
