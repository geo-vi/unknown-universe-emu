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

        public bool IsRegenerating;

        public Npc(int id, string name, Hangar hangar, Faction factionId, Vector position, Spacemap spacemap, int maxHealth, int currentNanoHull,
            int maxShield, int currentShield, int currentHealth, int damage, Reward reward, int respawnTime = 0, bool respawning = true, Npc motherShip = null)
            : base(id, name, hangar, factionId)
        {
            CurrentShield = currentShield;
            MaxShield = maxShield;
            ShieldAbsorption = 0.5;
            MaxHealth = maxHealth;
            CurrentNanoHull = currentNanoHull;
            CurrentHealth = currentHealth;
            Damage = damage;
            MotherShip = motherShip;
            RespawnTime = respawnTime;
            Respawning = respawning;
            Reward = reward;
            IsRegenerating = true;
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

        public int GetMotherShipId()
        {
            if (MotherShip != null)
            {
                return MotherShip.Id;
            }

            return 0;
        }
    }
}
