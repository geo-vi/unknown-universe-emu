using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
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
