using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.server;

namespace Server.Game.controllers.characters
{
    class CharacterDamageController : AbstractedSubController
    {
        public override void OnAdded()
        {
            Character.OnDamageReceived += DamageReceived;
        }

        public override void OnOverwritten()
        {
            Character.OnDamageReceived -= DamageReceived;
        }
        
        public override void OnRemoved()
        {
            Character.OnDamageReceived -= DamageReceived;
        }
        
        protected virtual void DamageReceived(object sender, PendingDamage e)
        {
            var selectors = Character.Controller.GetInstance<CharacterSelectionController>().FindAllSelectors();
            
            foreach (var selector in selectors)
            {
                if (selector is Player playerSelector)
                {
                    PrebuiltCombatCommands.Instance.DamageCommand(playerSelector, e);
                }
            }
        }
    }
}