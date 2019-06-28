using DotNetty.Buffers;
using Server.Game;
using Server.Game.netty;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class QuestInfoHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;

            var questInfoRequest = new QuestInfoRequest();
            questInfoRequest.readCommand(buffer);

            Packet.Builder.QuestInfoCommand(gameSession, World.StorageManager.Quests[questInfoRequest.questId]);
        }
    }
}
