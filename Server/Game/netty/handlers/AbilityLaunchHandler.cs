using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    class AbilityLaunchHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
//            if (gameSession.Player.UsingNewClient) return;
//            var request = new AbilityLaunchRequest();
//            request.readCommand(buffer);
//
//            var selectedAbilityId = request.selectedAbilityId;
//
//            var ability = (Abilities) selectedAbilityId;
//            if (gameSession.Player.Abilities.ContainsKey(ability))
//                gameSession.Player.Abilities[ability].execute();

        }
    }
}
