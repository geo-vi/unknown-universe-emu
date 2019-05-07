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
