using Server.Game.objects.entities;
using Server.Game.objects.entities.players;

namespace Server.Game.controllers.player
{
    class AbilityController : PlayerSubController
    {
        public Ability SelectedAbility;

        public AbilityController(Player player) : base(player)
        {
        } 
        
        public void Execute()
        {
            
        }

        public void Disable()
        {
            
        }
    }
}