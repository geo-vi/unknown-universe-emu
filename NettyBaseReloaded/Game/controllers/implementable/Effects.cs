using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Effects : IAbstractCharacter
    {
        public bool SlowedDown = false;
        
        public Effects(AbstractCharacterController controller) : base(controller)
        {
        }

        public override void Tick()
        {
        }

        public override void Stop()
        {
        }

        public void SetInvincible(int time, bool showEffect = false)
        {
            if (Character.Cooldowns.CooldownDictionary.Any(c => c.Value is InvincibilityCooldown)) return;

            var cooldown = new InvincibilityCooldown(showEffect, DateTime.Now.AddMilliseconds(time));
            cooldown.OnStart(Character);
            Character.Cooldowns.Add(cooldown);
        }

        public void NotTargetable(int time)
        {
            if (Character.Cooldowns.CooldownDictionary.Any(c => c.Value is NonTargetableCooldown)) return;

            var cooldown = new NonTargetableCooldown(DateTime.Now.AddMilliseconds(time));
            cooldown.OnStart(Character);
            Character.Cooldowns.Add(cooldown);
        }

        public void UpdateCharacterVisibility()
        {
            GameClient.SendPacketSelected(Controller.Character,
                netty.commands.old_client.LegacyModule.write("0|n|INV|" + Controller.Character.Id + "|" +
                                                             Convert.ToInt32(Controller.Character.Invisible)));
            GameClient.SendPacketSelected(Controller.Character,
                netty.commands.new_client.LegacyModule.write("0|n|INV|" + Controller.Character.Id + "|" +
                                                             Convert.ToInt32(Controller.Character.Invisible)));
            GameClient.SendRangePacket(Controller.Character,
                netty.commands.old_client.LegacyModule.write("0|n|INV|" + Controller.Character.Id + "|" +
                                                             Convert.ToInt32(Controller.Character.Invisible)), true);
            GameClient.SendRangePacket(Controller.Character,
                netty.commands.new_client.LegacyModule.write("0|n|INV|" + Controller.Character.Id + "|" +
                                                             Convert.ToInt32(Controller.Character.Invisible)), true);
        }

        public void Uncloak()
        {
            Character.Invisible = false;
            UpdateCharacterVisibility();
            if (Character is Player player)
            {
                if (player.Pet != null && player.Pet.Controller.Active)
                {
                    player.Pet.Invisible = false;
                    player.Pet.Controller.Effects.UpdateCharacterVisibility();
                }
            }
        }
    }
}
