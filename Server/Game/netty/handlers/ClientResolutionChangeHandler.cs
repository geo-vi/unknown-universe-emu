using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class ClientResolutionChangeHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new ClientResolutionChangeRequest();
            cmd.readCommand(buffer);
            gameSession.Player.Settings.ASSET_VERSION = cmd.resolutionId;
            gameSession.Player.Settings.SaveSettings();
        }
    }
}
