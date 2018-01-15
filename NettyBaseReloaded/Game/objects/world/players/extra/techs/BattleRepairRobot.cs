using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.players.extra.techs
{
    class BattleRepairRobot : Tech
    {
        public BattleRepairRobot(Player player) : base(player)
        {
        }

        public override void Tick()
        {
            if (Active)
            {
                if (TimeFinish.AddSeconds(1) > DateTime.Now) CheckForEnd();
                ExecuteHeal();
            }
        }

        public override void execute()
        {
            if (Player.Cooldowns.Exists(x => x is BattleRepairRobotCooldown)) return;

            Active = true;
            GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write("0|TX|A|S|BRB|" + Player.Id), true);
            GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write("0|TX|A|S|BRB|" + Player.Id), true);
            ExecuteHeal();
            TimeFinish = DateTime.Now.AddSeconds(9);
        }

        private DateTime LastHeal = new DateTime();
        private void ExecuteHeal()
        {
            if (LastHeal.AddSeconds(1) > DateTime.Now)
            {
                return;
            }
            Player.Controller.Heal.Execute(10000);
            LastHeal = DateTime.Now;
        }

        private void CheckForEnd()
        {
            if (TimeFinish.AddSeconds(10) < DateTime.Now)
            {
                Active = false;
                GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write("0|TX|D|S|BRB|" + Player.Id), true);
                GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write("0|TX|D|S|BRB|" + Player.Id), true);

                var cld = new BattleRepairRobotCooldown();
                cld.Send(Player.GetGameSession());
                Player.Cooldowns.Add(cld);
            }
        }
    }
}
