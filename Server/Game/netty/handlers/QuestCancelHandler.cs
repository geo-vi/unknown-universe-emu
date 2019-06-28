using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class QuestCancelHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;
            var questCancelRequest = new QuestCancelRequest();
            questCancelRequest.readCommand(buffer);

            var player = gameSession.Player;
            if (player.QuestData.IsQuestActive(questCancelRequest.questId))
            {
                var quest = World.StorageManager.Quests[questCancelRequest.questId];
                player.QuestData.CancelQuest(quest);
            }
            else
            {
                Packet.Builder.QuestCancelledCommand(gameSession, questCancelRequest.questId);
            }
        }
    }
}
