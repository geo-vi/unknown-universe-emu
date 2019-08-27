using System;
using Server.Game.controllers.players;
using Server.Game.managers;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.enums;

namespace Server.Game.netty.handlers
{
    class AttackRocketLegacyHandler : ILegacyHandler
    {
        public void Execute(GameSession gameSession, string[] param)
        {
            if (gameSession == null) return;
            var targetId = Int32.Parse(param[1]);


            // Get if targetId is valid
            var player = gameSession.Player;

            if (player.Selected == null) return;

            if (!player.Spacemap.Entities.ContainsKey(targetId) || player.Selected.Id != targetId)
            {
                //Debug.WriteLine("Selected ID: " + player.Selected.Id + " Target ID: " + targetId);
                return;
            }

            player.Controller.GetInstance<PlayerCombatController>().OnRocketAttack(player.Selected);
        }
    }
}
