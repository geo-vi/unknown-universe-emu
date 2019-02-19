using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.players.extra.abilities
{
    class CitadelTravelMode : Ability
    {
        public CitadelTravelMode(Player player) : base(player, Abilities.SHIP_ABILITY_CITADEL_TRAVEL_MODE)
        {
        }

        public override void Tick()
        {
            Update();
        }

        public override void ThreadUpdate()
        {
            if (TimeFinish < DateTime.Now) Stop();
        }

        public override void execute()
        {
            if (!Enabled) return;
            Active = true;
            Start();
            TimeFinish = DateTime.Now.AddSeconds(5);
            Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|fx|start|SPEED_BUFF|" + Player.Id);
            Player.BoostSpeed(1.3);
        }

        public void Stop()
        {
            Active = false;
            Player.BoostedAcceleration = 0;
            Player.UpdateSpeed();
            End(); // todo:
            //Cooldown = new VengeanceLightningCooldown(this);
        }
    }
}
