using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AbilityLaunchHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient) return;
            var request = new AbilityLaunchRequest();
            request.readCommand(bytes);

            var selectedAbilityId = request.selectedAbilityId;

            var ability = gameSession.Player.Abilities.FirstOrDefault(x => x.AbilityId == selectedAbilityId);
            ability?.execute();

        }
    }
}
