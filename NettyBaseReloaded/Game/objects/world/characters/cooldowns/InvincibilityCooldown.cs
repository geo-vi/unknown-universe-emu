﻿using System;

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
            character.Controller.Invincible = true;
            //if (ShowEffect)
            //character.Controller.UpdateVisuals(); TODO
        }

        public override void OnFinish(Character character)
        {
            character.Controller.Invincible = false;
            //if (ShowEffect)
                //1character.Controller.UpdateVisuals(); TODO
        }

        public override void Send(GameSession gameSession)
        {
        }
    }
}