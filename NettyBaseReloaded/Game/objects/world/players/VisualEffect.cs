using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class VisualEffect : PlayerBaseClass
    {
        private Player Player;

        public ShipVisuals Visual { get; set; }

        public int Attribute { get; set; } // Used for WIZARD_ATTACK, BattleStations, Generic Glow
        public DateTime EndTime { get; set; }

        public bool Active => true;

        public VisualEffect(Player player, ShipVisuals visualEffect, DateTime effectEndTime) : base(player)
        {
            Player = player;
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

            var gs = Player.GetGameSession();
            Packet.Builder.VisualModifierCommand(gs, this);
            //call packet.builder
        }

        public void Cancel()
        {
            //TODO
            //Player.Effects.Remove(this);
        }

        public static List<VisualModifierCommand> ToOldModifierCommand(Player player)
        {
            var visuals = new List<VisualModifierCommand>();
            foreach (var visual in player.Visuals)
            {
                visuals.Add(new VisualModifierCommand(player.Id, (short)visual.Visual, visual.Attribute, visual.Active));
            }

            return visuals;
        }

        public static List<netty.commands.new_client.VisualModifierCommand> ToNewModifierCommand(Player player)
        {
            var visuals = new List<netty.commands.new_client.VisualModifierCommand>();
            foreach (var visual in player.Visuals)
            {
                visuals.Add(new netty.commands.new_client.VisualModifierCommand(player.Id, (short)visual.Visual, visual.Attribute, visual.Active, "", 1));
            }

            return visuals;
        }
    }
}
