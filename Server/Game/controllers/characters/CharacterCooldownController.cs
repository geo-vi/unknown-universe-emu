using Server.Game.objects.server;

namespace Server.Game.controllers.characters
{
    class CharacterCooldownController : AbstractedSubController
    {
        public virtual void OnCooldownStart(Cooldown cooldown)
        {
        }

        public virtual void OnCooldownFinish(Cooldown cooldown)
        {
        }
    }
}