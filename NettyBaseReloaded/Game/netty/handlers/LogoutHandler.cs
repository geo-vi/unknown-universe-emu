using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class LogoutHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new LogoutRequest();
            cmd.readCommand(buffer);

            switch (cmd.request)
            {
                case LogoutRequest.REQUEST_LOGOUT:
                    gameSession.Player.Controller.Miscs.Logout(true);
                    break;
                case LogoutRequest.ABORT_LOGOUT:
                    gameSession.Player.Controller.Miscs.AbortLogout();
                    break;
            }
        }
    }
}
