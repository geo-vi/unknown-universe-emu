using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.npcs;
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

        public sealed override int Damage { get; set; }

        public sealed override int CurrentShield { get; set; }

        public sealed override int MaxShield { get; set; }

        public sealed override double ShieldAbsorption { get; set; }

        public sealed override double ShieldPenetration { get; set; }

        public Npc MotherShip { get; set; }

        public int RespawnTime { get; set; }
        public bool Respawning { get; set; }

        public override int AttackRange => Hangar.Ship.GetAttackRange();

        public Npc(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int currentHealth, int currentNanoHull, Reward rewards,
            int maxShield, int damage, int respawnTime = 0, bool respawning = true, Npc motherShip = null)
            : base(id, name, hangar, factionId, position, spacemap, rewards)
        {
            CurrentHealth = currentHealth;
            CurrentNanoHull = currentNanoHull;
            Damage = damage;
            CurrentShield = maxShield;
            MaxShield = maxShield;
            ShieldAbsorption = 0.5;
            MotherShip = motherShip;
            RespawnTime = respawnTime;
            Respawning = respawning;
        }

        public new void Tick()
        {
            Controller.Tick();
        }

        public override void Destroy(Character destroyer)
        {
            base.Destroy(destroyer);
            if (MotherShip != null && MotherShip is Cubikon cubi)
            {
                Npc removedNpc;
                cubi.Children.TryRemove(Id, out removedNpc);
            }
        }
    }
}
