using Server.Game.controllers.server;
using Server.Game.objects.server;

namespace Server.Game.controllers.characters
{
    class CharacterCooldownController : AbstractedSubController
    {
        /// <summary>
        /// Adding event handles
        /// </summary>
        public override void OnAdded()
        {
            ServerController.Get<CooldownController>().OnCooldownAdded += OnCooldownAdded;
            ServerController.Get<CooldownController>().OnCooldownFinish += OnCooldownFinish;
        }

        public override void OnOverwritten()
        {
            ServerController.Get<CooldownController>().OnCooldownAdded -= OnCooldownAdded;
            ServerController.Get<CooldownController>().OnCooldownFinish -= OnCooldownFinish;
        }

        /// <summary>
        /// Removing event handles
        /// </summary>
        public override void OnRemoved()
        {
            ServerController.Get<CooldownController>().OnCooldownAdded -= OnCooldownAdded;
            ServerController.Get<CooldownController>().OnCooldownFinish -= OnCooldownFinish;
        }

        /// <summary>
        /// Checking the owner since it receives every cooldown added
        /// </summary>
        /// <param name="sender">CooldownController</param>
        /// <param name="cooldown">Cooldown</param>
        private void OnCooldownAdded(object sender, Cooldown cooldown)
        {
            if (cooldown.Owner == Character)
            {
                OnCooldownAdded(cooldown);
            }
        }

        /// <summary>
        /// Cooldown that has been added for Character
        /// </summary>
        /// <param name="cooldown"></param>
        protected virtual void OnCooldownAdded(Cooldown cooldown)
        {
        }

        /// <summary>
        /// Checking cooldown owner for finish
        /// </summary>
        /// <param name="sender">CooldownController</param>
        /// <param name="cooldown">Cooldown</param>
        private void OnCooldownFinish(object sender, Cooldown cooldown)
        {
            if (cooldown.Owner == Character)
            {
                OnCooldownFinish(cooldown);
            }
        }

        /// <summary>
        /// Cooldown that finished
        /// </summary>
        /// <param name="cooldown"></param>
        protected virtual void OnCooldownFinish(Cooldown cooldown)
        {
        }
    }
}