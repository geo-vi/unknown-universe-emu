using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players.killscreen;

namespace NettyBaseReloaded.Game.controllers.login
{
    class KilledLogin : ILogin
    {
        public KilledLogin(GameSession gameSession) : base(gameSession)
        {
            gameSession.Player.EntityState = objects.world.EntityStates.DEAD;
        }

        public override void Execute()
        {
            SendSettings();
            SendLegacy();
            Packet.Builder.KillScreenCommand(GameSession, Killscreen.Load(GameSession.Player));
        }
    }
}
