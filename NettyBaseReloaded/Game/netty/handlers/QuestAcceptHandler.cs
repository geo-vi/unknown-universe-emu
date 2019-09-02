using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
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
