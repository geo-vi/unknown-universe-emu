using DotNetty.Buffers;
using Server.Game.netty.commands.new_client.requests;

namespace Server.Game.netty.handlers
{
    class AttackLaserHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
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

            if (!player.Spacemap.Entities.ContainsKey(targetId) && !player.Spacemap.Objects.ContainsKey(targetId)|| player.Selected.Id != targetId)
            {
                //Debug.WriteLine("Selected ID: " + player.Selected.Id + " Target ID: " + targetId);
                return;
            }

            player.Controller.Attack.Attacking = true;
            player.Controller.Attack.LaserAttack();
        }
    }
}