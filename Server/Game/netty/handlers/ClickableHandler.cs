using DotNetty.Buffers;
using Server.Game.netty.commands.new_client.requests;

namespace Server.Game.netty.handlers
{
    class ClickableHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new ClickableRequest();
            cmd.readCommand(buffer);
            var obj = gameSession.Player.Range.Objects[cmd.clickableId];
            (obj as IClickable)?.click(gameSession.Player);
        }
    }
}
