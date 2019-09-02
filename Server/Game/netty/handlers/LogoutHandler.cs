using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
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
