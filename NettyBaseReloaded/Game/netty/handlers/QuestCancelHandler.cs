using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class QuestCancelHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient) return;
            var questCancelRequest = new QuestCancelRequest();
            questCancelRequest.readCommand(bytes);
            var quest = gameSession.Player.AcceptedQuests.Find(x => x.Id == questCancelRequest.questId);
            if (quest != null)
            {
                gameSession.Player.AcceptedQuests.Remove(quest);
                gameSession.Client.Send(commands.old_client.QuestCancelledCommand.write(quest.Id).Bytes);
                Packet.Builder.QuestListCommand(gameSession);
                quest.Canceled = true;
                World.DatabaseManager.SaveQuestData(gameSession.Player, quest);
            }
        }
    }
}
