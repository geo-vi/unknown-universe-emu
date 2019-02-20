using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class PlasmaEffect : Cooldown
    {
        public PlasmaEffect() : base(DateTime.Now, DateTime.Now.AddSeconds(3))
        {
        }

        public override void OnStart(Character character)
        {
            base.OnStart(character);

            var player = character as Player;
            if (player != null)
            {
                player.Controller.Attack.Disabled = true;
            }
        }

        public override void OnFinish(Character character)
        {
            var player = character as Player;
            if (player != null)
            {
                player.Controller.Attack.Disabled = false;
            }
        }

        public override void Send(GameSession gameSession)
        {
        }
    }
}
