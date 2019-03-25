using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.pets;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class PetComboRepairCooldown : Cooldown
    {
        public PetComboRepairCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(30))
        {
        }

        public override void OnStart(Character character)
        {
            base.OnStart(character);
        }

        public override void OnFinish(Character character)
        {
            var player = character as Player;
            if (player != null)
            {
                //Packet.Builder.PetBuffCommand(player.GetGameSession(), false, BuffPattern.SHIP_REPAIR_BUFF, new List<int>());
            }
        }


        public override void Send(GameSession gameSession)
        {
            //Packet.Builder.PetBuffCommand(gameSession, true, BuffPattern.SHIP_REPAIR_BUFF, new List<int>{ TimeLeft.Seconds });
        }
    }
}
