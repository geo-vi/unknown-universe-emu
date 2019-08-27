using Server.Game.controllers.characters;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.controllers.players
{
    class PlayerCombatController : CharacterCombatController
    {        
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }

        /// <summary>
        /// Creating a different laser combat designed for player where it calculates the lasers equipped for amount
        /// </summary>
        /// <param name="target"></param>
        protected override void OnLaserCombat(AbstractAttackable target)
        {
            var currentLaserSelected = Player.Settings.GetSettings<SlotbarSettings>().SelectedLaserAmmo;
            
            CombatManager.Instance.CreateCombat(Player, target, AttackTypes.LASER, currentLaserSelected);
        }
    }
}