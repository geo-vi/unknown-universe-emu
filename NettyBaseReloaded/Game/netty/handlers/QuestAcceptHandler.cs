using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players.quests.player_quests;
using NettyBaseReloaded.Game.objects.world.players.quests.quest_stats;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class QuestAcceptHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient) return;

            var request = new QuestAcceptRequest();
            request.readCommand(bytes);

            if (gameSession.Player.AcceptedQuests.Exists(x => x.Id == request.questId) ||
                gameSession.Player.CompletedQuests.Exists(x => x.Id == request.questId)) return;

            switch (request.questId)
            {
                case 0:
                    gameSession.Player.AcceptedQuests.Add(new KillstreakQuest(gameSession.Player, new KillstreakQuestStat()));
                    break;
                case 2:
                    gameSession.Player.AcceptedQuests.Add(new StarterBaseQuest(gameSession.Player, new StarterBaseQuestStats()));
                    break;
            }

            Packet.Builder.QuestInitializationCommand(gameSession);
            Packet.Builder.QuestListCommand(gameSession);
            World.DatabaseManager.SaveQuestData(gameSession.Player, gameSession.Player.AcceptedQuests.Find(x => x.Id == request.questId));
        }
    }
}
