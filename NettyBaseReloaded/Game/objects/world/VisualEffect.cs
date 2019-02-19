using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;

namespace NettyBaseReloaded.Game.objects.world
{
    class VisualEffect
    {
        public IAttackable Entity;

        public ShipVisuals Visual { get; set; }

        public int Attribute { get; set; } // Used for WIZARD_ATTACK, BattleStations, Generic Glow
        public DateTime EndTime { get; set; }

        public bool Active = false;

        public VisualEffect(IAttackable attackable, ShipVisuals visualEffect, DateTime effectEndTime)
        {
            Entity = attackable;
            Visual = visualEffect;
            EndTime = effectEndTime;
        }

        public void Tick()
        {
            if (DateTime.Now > EndTime || !Active) Cancel();
        }

        public void Start()
        {
            Active = true;
            if (Entity.Visuals.ContainsKey(Visual))
            {
                Entity.Visuals[Visual] = this;
            }
            else Entity.Visuals.TryAdd(Visual, this);

            foreach (var rangeSession in GameSession.GetRangeSessions(Entity).Values)
            {
                if (rangeSession != null)
                    Packet.Builder.VisualModifierCommand(rangeSession, this);
            }
        }

        public void Cancel()
        {
            Active = false;
            VisualEffect output;
            Entity.Visuals.TryRemove(Visual, out output);
            foreach (var rangeSession in GameSession.GetRangeSessions(Entity).Values)
            {
                if (rangeSession != null)
                    Packet.Builder.VisualModifierCommand(rangeSession, this);
            }
            if (Entity is Character character)
            {
                character.RefreshPlayersView();
                if (Entity is Player player)
                    player.RefreshMyView();
            }
        }

        public static List<VisualModifierCommand> ToOldModifierCommand(IAttackable entity)
        {
            var visuals = new List<VisualModifierCommand>();
            foreach (var visual in entity.Visuals.Values)
            {
                visuals.Add(new VisualModifierCommand(entity.Id, (short)visual.Visual, visual.Attribute, visual.Active));
            }

            return visuals;
        }

        public static List<netty.commands.new_client.VisualModifierCommand> ToNewModifierCommand(Player player)
        {
            var visuals = new List<netty.commands.new_client.VisualModifierCommand>();
            foreach (var visual in player.Visuals.Values)
            {
                visuals.Add(new netty.commands.new_client.VisualModifierCommand(player.Id, (short)visual.Visual, visual.Attribute, visual.Active, "", 1));
            }

            return visuals;
        }
    }
}
