using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class QuestInfoHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient) return;

            var questInfoRequest = new QuestInfoRequest();
            questInfoRequest.readCommand(bytes);

            if (gameSession.Player.AcceptedQuests.Exists(x => x.Id == questInfoRequest.questId))
                Packet.Builder.QuestInfoCommand(gameSession, gameSession.Player.AcceptedQuests.Find(x => x.Id == questInfoRequest.questId));
            else if (gameSession.Player.CompletedQuests.Exists(x => x.Id == questInfoRequest.questId))
                Packet.Builder.QuestInfoCommand(gameSession, gameSession.Player.CompletedQuests.Find(x => x.Id == questInfoRequest.questId));
            else Packet.Builder.QuestInfoCommand(gameSession, Quest.Quests.Find(x => x.Id == questInfoRequest.questId));
        }
    }
}
