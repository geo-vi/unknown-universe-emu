using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.controllers.login
{
    class Killscreen : ILogin
    {
        public Killscreen(GameSession gameSession) : base(gameSession)
        {
        }

        public override void Execute()
        {
            Packet.Builder.KillScreenCommand(GameSession, null);
        }
    }
}
