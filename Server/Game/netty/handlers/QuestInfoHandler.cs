using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;

namespace Server.Game.netty.handlers
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
