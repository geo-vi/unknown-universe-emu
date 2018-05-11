using System;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class LaserCooldown : Cooldown
    {
        internal LaserCooldown() : base(DateTime.Now, DateTime.Now.AddMilliseconds(800)) { }

        public override void OnStart(Character character)
        {
            base.OnStart(character);
        }

        public override void OnFinish(Character character)
        {
        }

        public override void Send(GameSession gameSession)
        {
        }
    }
}