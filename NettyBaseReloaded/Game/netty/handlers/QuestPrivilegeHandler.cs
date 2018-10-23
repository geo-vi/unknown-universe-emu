using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class QuestPrivilegeHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient) return;

            var cmd = new QuestPrivilegeRequest();
            cmd.readCommand(bytes);
            var qId = cmd.questId;
            Packet.Builder.QuestPrivilegeCommand(gameSession, qId);
        }
    }
}
