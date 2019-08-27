using DotNetty.Buffers;
using Server.Game.controllers.players;
using Server.Game.managers;
using Server.Game.netty.commands.new_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.enums;

namespace Server.Game.netty.handlers
{
    class AttackLaserHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession == null) return;
            var targetId = 0;

            if (gameSession.Player.UsingNewClient)
            {
                var cmd = new AttackLaserRequest();
                cmd.readCommand(buffer);
                targetId = cmd.selectedId;
            }
            else
            {
                var cmd = new commands.old_client.requests.AttackLaserRequest();
                cmd.readCommand(buffer);
                targetId = cmd.targetId;
            }

            // Get if targetId is valid
            var player = gameSession.Player;

            if (player.Selected == null) return;

            if (!player.Spacemap.Entities.ContainsKey(targetId) && !player.Spacemap.Objects.ContainsKey(targetId) ||
                player.Selected.Id != targetId)
            {
                //Debug.WriteLine("Selected ID: " + player.Selected.Id + " Target ID: " + targetId);
                return;
            }

            player.Controller.GetInstance<PlayerCombatController>().OnLaserAttackStart(player.Selected);
        }
    }
}