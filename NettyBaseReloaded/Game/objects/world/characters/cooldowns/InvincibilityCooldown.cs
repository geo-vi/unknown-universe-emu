using System;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class InvincibilityCooldown : Cooldown
    {
        private bool ShowEffect { get; }
        internal InvincibilityCooldown(bool showEffect, DateTime endTime) : base(DateTime.Now, endTime)
        {
            ShowEffect = showEffect;
        }

        public override void OnStart(Character character)
        {
            base.OnStart(character);
            character.Invincible = true;
            if (ShowEffect)
            {
                var effect = new VisualEffect(character, ShipVisuals.INVINCIBILITY, EndTime);
                effect.Start();
            }
        }

        public override void OnFinish(Character character)
        {
            character.Invincible = false;
            if (character is Player player)
            {
                if (player.State.LoginProtection)
                {
                    player.State.LoginProtection = false;
                }
            }
        }

        public override void Send(GameSession gameSession)
        {
        }
    }
}