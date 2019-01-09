using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class DecelerationEffect : Cooldown
    {
        public DecelerationEffect() : base(DateTime.Now, DateTime.Now.AddSeconds(5))
        {
        }

        public override void OnStart(Character character)
        {
            base.OnStart(character);

            var player = character as Player;
            if (player != null)
            {
                player.Controller.Effects.SlowedDown = true;
                GameClient.SendRangePacket(character, netty.commands.old_client.LegacyModule.write("0|n|fx|start|SABOTEUR_DEBUFF|" + character.Id), true);
                GameClient.SendRangePacket(character, netty.commands.new_client.LegacyModule.write("0|n|fx|start|SABOTEUR_DEBUFF|" + character.Id), true);
                player.UpdateSpeed();
                MovementController.Move(player, player.Destination);
            }
        }

        public override void OnFinish(Character character)
        {
            var player = character as Player;
            if (player != null)
            {
                player.Controller.Effects.SlowedDown = false;
                GameClient.SendRangePacket(character, netty.commands.old_client.LegacyModule.write("0|n|fx|end|SABOTEUR_DEBUFF|" + character.Id), true);
                GameClient.SendRangePacket(character, netty.commands.new_client.LegacyModule.write("0|n|fx|end|SABOTEUR_DEBUFF|" + character.Id), true);
                player.UpdateSpeed();
                MovementController.Move(player, player.Destination);
            }
        }

        public override void Send(GameSession gameSession)
        {
        }
    }
}
