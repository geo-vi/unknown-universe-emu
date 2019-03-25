using System;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class SMBCooldown : Cooldown
    {
        internal SMBCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(30)) { }

        public override void OnStart(Character character)
        {
            base.OnStart(character);
        }

        public override void OnFinish(Character character)
        {

        }

        public override void Send(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(SetCooldown("ammunition_mine_smb-01", TimerState.COOLDOWN, TimeLeft.TotalMilliseconds, TotalTime.TotalMilliseconds, true));
            }
            else
            {
                Packet.Builder.LegacyModule(gameSession, "0|A|CLD|SMB|" + TimeLeft.Seconds, true);
            }
        }
    }
}