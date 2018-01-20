using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class VisualEffect : PlayerBaseClass
    {
        public ShipVisuals Visual { get; set; }

        public int Attribute { get; set; } // Used for WIZARD_ATTACK, BattleStations, Generic Glow
        public DateTime EndTime { get; set; }

        public bool Active { get; set; }

        public VisualEffect(Player player, ShipVisuals visualEffect, DateTime effectEndTime) : base(player)
        {
            Visual = visualEffect;
            EndTime = effectEndTime;
        }

        public void Tick()
        {
            if (DateTime.Now > EndTime) Cancel();
        }

        public void Start()
        {
            if (Player.Visuals.Exists(x => x.Visual == Visual))
            {
                var indx = Player.Visuals.FindIndex(x => x.Visual == Visual);
                Player.Visuals[indx] = this;
            }
            else Player.Visuals.Add(this);

            if (Active)
            {
                //TODO
                return;
            } 

            //call packet.builder
        }

        public void Cancel()
        {
            //TODO
            //Player.Effects.Remove(this);
        }
    }
}
