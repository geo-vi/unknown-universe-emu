using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players.extra.abilities;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AbilityLaunchHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;
            var request = new AbilityLaunchRequest();
            request.readCommand(buffer);

            var selectedAbilityId = request.selectedAbilityId;

            var ability = (Abilities) selectedAbilityId;
            if (gameSession.Player.Abilities.ContainsKey(ability))
                gameSession.Player.Abilities[ability].execute();

        }
    }
}
