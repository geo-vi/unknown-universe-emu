using System;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AttackRocketHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession == null) return;
            var targetId = 0;

            if (gameSession.Player.UsingNewClient)
            {
                var cmd = new commands.new_client.requests.AttackRocketRequest();
                cmd.readCommand(buffer);
                targetId = cmd.targetId;
            }

            // Get if targetId is valid
            var player = gameSession.Player;

            if (player.Selected == null) return;

            if (!player.Spacemap.Entities.ContainsKey(targetId) || player.Selected.Id != targetId)
            {
                //Debug.WriteLine("Selected ID: " + player.Selected.Id + " Target ID: " + targetId);
                return;
            }

            player.Controller.Attack.LaunchMissle(player.Settings.CurrentRocket.LootId);
        }
    }
}