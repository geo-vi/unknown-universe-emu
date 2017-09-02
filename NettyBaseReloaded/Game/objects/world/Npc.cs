using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Game.objects.world
{
    class BaseNpc
    {
        public int NpcId { get; set; }
        public int Count { get; set; }

        public BaseNpc(int NpcId, int Count)
        {
            this.NpcId = NpcId;
            this.Count = Count;
        }
    }
    class Npc : Character
    {
        /**********
         * BASICS *
         **********/
        public new NpcController Controller { get; set; }

        /*********
         * STATS *
         *********/
        public override int MaxHealth
        {
            get
            {
                return Hangar.Ship.Health;
            }
        }

        public Npc MotherShip { get; set; }

        public int RespawnTime { get; set; }

        public Npc(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int currentHealth, int currentNanoHull, Reward rewards, DropableRewards dropableRewards,
            int maxShield, int damage, int respawnTime = 0, Npc motherShip = null)
            : base(id, name, hangar, factionId, position, spacemap, currentHealth, currentNanoHull, rewards, dropableRewards)
        {
            Damage = damage;
            CurrentShield = maxShield;
            MaxShield = maxShield;
            ShieldAbsorption = 0.8;
            ShieldPenetration = 0;
            MotherShip = motherShip;
            RespawnTime = respawnTime;
        }

        public new void Tick()
        {
            Controller.Tick();
        }
    }
}
