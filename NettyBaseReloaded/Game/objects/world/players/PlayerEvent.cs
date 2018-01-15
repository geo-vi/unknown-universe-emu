using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world.events;
using NettyBaseReloaded.Game.objects.world.players.events;

namespace NettyBaseReloaded.Game.objects.world.players
{
    abstract class PlayerEvent : PlayerBaseClass
    {
        /// <summary>
        /// Event ID
        /// </summary>
        public int Id { get; set; }

        public int Score { get; set; }

        private DateTime LastTimeUpdated = new DateTime();

        //Updates in MS
        protected int UpdatesInterval { get; set; }

        protected PlayerEvent(Player player, int id, int updInterval) : base(player)
        {
            Id = id;
            UpdatesInterval = updInterval;
        }

        public virtual void Tick()
        {
            if (LastTimeUpdated.AddMilliseconds(UpdatesInterval) < DateTime.Now) UpdatePlayer();
        }

        protected virtual void UpdatePlayer()
        {
            // made to be overrridden if needed
        }

        public virtual void DestroyAttackable(IAttackable attackable)
        {
            // BLANK -> made to be overriden if needed
        }

        public virtual void Destroyed()
        {
            // BLANK -> made to be overriden if needed
        }

        public abstract void Start();
        public abstract void End();
    }
}
