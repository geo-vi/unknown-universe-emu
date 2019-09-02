using System;
using Server.Game.objects.server;

namespace Server.Game.controllers.characters
{
    class CharacterDamageController : AbstractedSubController
    {
        public override void OnAdded()
        {
            Character.OnDamageReceived += OnDamageReceived;
        }

        protected virtual void OnDamageReceived(object sender, PendingDamage e)
        {
        }

        public override void OnRemoved()
        {
            Character.OnDamageReceived -= OnDamageReceived;
        }
    }
}