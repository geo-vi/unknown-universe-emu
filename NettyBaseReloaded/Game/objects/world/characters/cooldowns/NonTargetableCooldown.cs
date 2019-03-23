using System;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class NonTargetableCooldown : Cooldown
    {
        internal NonTargetableCooldown(DateTime endTime) : base(DateTime.Now, endTime)
        {
        }

        public override void OnStart(Character character)
        {
            character.Targetable = false;
        }

        public override void OnFinish(Character character)
        {
            character.Targetable = true;
        }

        public override void Send(GameSession gameSession)
        {
        }
    }
}