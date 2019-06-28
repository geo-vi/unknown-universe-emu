using DotNetty.Buffers;
using Server.Game.netty.commands.old_client;

namespace Server.Game.netty.handlers
{
    class UserKeyBindingsUpdateHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;

            var bindings = new UserKeyBindingsUpdate();
            bindings.readCommand(buffer);
            gameSession.Player.Settings.OldClientKeyBindingsCommand = bindings;
            gameSession.Player.Settings.SaveSettings();
        }
    }
}
