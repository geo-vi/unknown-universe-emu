using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class LogoutHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var cmd = new LogoutRequest();
            cmd.readCommand(bytes);

            // very important thing - always send packets from Packet.Builder!!

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
