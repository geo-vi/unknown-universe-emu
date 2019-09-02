using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class QuestAcceptHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;

            var request = new QuestAcceptRequest();
            request.readCommand(buffer);

            var quest = World.StorageManager.Quests[request.questId];
            if (gameSession.Player.QuestData.GetActiveQuests().Count > 4 || gameSession.Player.QuestData.CompletedQuests.ContainsKey(request.questId) || gameSession.Player.QuestData.IsQuestActive(request.questId))
            {
                return;
            }

            gameSession.Player.QuestData.AcceptQuest(quest);
        }
    }
}
