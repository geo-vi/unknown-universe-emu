using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class DroneFormationCooldown : Cooldown
    {
        public DroneFormationCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(3))
        {
        }

        public override void OnFinish(Character character)
        {
        }

        public override void Send(GameSession gameSession)
        {
            Packet.Builder.LegacyModule(gameSession, "0|A|CLD|DRF|" + TimeLeft.Seconds, true);
            //foreach (var item in player.SlotbarItems) // TODO NEW CLIENT
            //{
            //    var key = item.Key;
            //    var value = item.Value;

            //    if (value is FormationItem)
            //    {
            //        player.Controller.SetCooldown(value, TimerState.COOLDOWN, 3000);
            //        value.Selected = false;
            //        gameSession.Client.Send(value.ChangeStatus());
            //    }
            //}
        }
    }
}
