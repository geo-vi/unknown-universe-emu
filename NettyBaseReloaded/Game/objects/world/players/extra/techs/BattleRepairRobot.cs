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
        BattleRepairRobotCooldown cld = new BattleRepairRobotCooldown();

        public BattleRepairRobot(Player player) : base(player)
        {
        }

        public override void Tick()
        {
            if (Active)
            {
                if (TimeFinish < DateTime.Now)
                    Disable();
            }
        }

        public override void execute()
        {
            if (Enabled && !Player.Cooldowns.CooldownDictionary.Any(c => c.Value is BattleRepairRobotCooldown))
            {
                Active = true;
                Player.Storage.BattleRepairRobotActivated = true;
                Packet.Builder.TechStatusCommand(Player.GetGameSession());
                GameClient.SendToPlayerView(Player,
                    netty.commands.old_client.LegacyModule.write("0|TX|A|S|BRB|" + Player.Id), true);
                GameClient.SendToPlayerView(Player,
                    netty.commands.new_client.LegacyModule.write("0|TX|A|S|BRB|" + Player.Id), true);
                TimeFinish = DateTime.Now.AddSeconds(10);
                Player.Cooldowns.Add(cld);
                Start();
            }
        }

        public override void ThreadUpdate() => ExecuteHeal();

        private void ExecuteHeal()
        {
            Player.Controller.Heal.Execute(10000);
        }

        private void Disable()
        {
            Active = false;
            Player.Storage.BattleRepairRobotActivated = false;
            Packet.Builder.TechStatusCommand(Player.GetGameSession());
            GameClient.SendToPlayerView(Player, netty.commands.old_client.LegacyModule.write("0|TX|D|S|BRB|" + Player.Id), true);
            GameClient.SendToPlayerView(Player, netty.commands.new_client.LegacyModule.write("0|TX|D|S|BRB|" + Player.Id), true);
            cld.Send(Player.GetGameSession());
        }
    }
}
