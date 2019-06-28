using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
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
