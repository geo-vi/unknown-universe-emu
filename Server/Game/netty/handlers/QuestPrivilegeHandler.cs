using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class QuestPrivilegeHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;

            var cmd = new QuestPrivilegeRequest();
            cmd.readCommand(buffer);
            var qId = cmd.questId;
            gameSession.Player.QuestData.SelectedQuestId = qId;
            Packet.Builder.QuestPrivilegeCommand(gameSession, qId);
        }
    }
}
