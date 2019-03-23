using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.pets;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class PetKamikazeCooldown : Cooldown
    {
        public PetKamikazeCooldown() : base(DateTime.Now, DateTime.Now.AddMinutes(1))
        {
        }

        public override void OnFinish(Character character)
        {
            if (character is Player player)
            {
                var gameSession = player.GetGameSession();
                //Packet.Builder.PetBuffCommand(gameSession, false, BuffPattern.KAMIKAZE_BUFF, new List<int>());
            }
        }

        public override void Send(GameSession gameSession)
        {
            //Packet.Builder.PetBuffCommand(gameSession, true, BuffPattern.KAMIKAZE_BUFF, new List<int>{TimeLeft.Seconds});
        }
    }
}
