using DotNetty.Buffers;
using Server.Game.netty.commands.new_client.requests;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class ItemSelectionHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient)
            {
                var cmd = new ItemSelectionRequest();
                cmd.readCommand(buffer);
                gameSession.Player.Controller.Miscs.UseItem(cmd.itemId);
            }
            else
            {
                var selectCmd = new SelectRocketRequest();
                selectCmd.readCommand(buffer);
                var type = selectCmd.type;
                gameSession.Player.Controller.Miscs.UseItem(AmmoConverter.AmmoTypeToString(type));
            }
        }
    }
}