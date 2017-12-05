using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world.characters;
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
        public static DebugLog Log = new DebugLog("npc");
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

        public sealed override int Damage { get; set; }

        public sealed override int CurrentShield { get; set; }

        public sealed override int MaxShield { get; set; }

        public sealed override double ShieldAbsorption { get; set; }

        public sealed override double ShieldPenetration { get; set; }

        public Npc MotherShip { get; set; }

        public int RespawnTime { get; set; }

        public Npc(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int currentHealth, int currentNanoHull, Reward rewards,
            int maxShield, int damage, int respawnTime = 0, Npc motherShip = null)
            : base(id, name, hangar, factionId, position, spacemap, rewards)
        {
            CurrentHealth = currentHealth;
            CurrentNanoHull = currentNanoHull;
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
