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
    class ClanMemberInvitationHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;

            var request = new ClanMemberInvitationRequest();
            request.readCommand(buffer);

            var inviterSession = World.StorageManager.GetGameSession(request.inviterId);
            var candidateSession = World.StorageManager.GetGameSession(request.candidateId);

            if (candidateSession != null && inviterSession != null)
            {
                var groupSystem = new GroupSystemHandler();
                groupSystem.AssembleInvite(inviterSession, candidateSession);
            }
        }
    }
}
