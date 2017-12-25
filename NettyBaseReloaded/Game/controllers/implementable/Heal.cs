using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Heal : IAbstractCharacter
    {
        public Heal(AbstractCharacterController controller) : base(controller)
        {
            
        }

        private DateTime LastTick = new DateTime();
        public override void Tick()
        {
            if (LastTick.AddSeconds(1) < DateTime.Now)
            {
                Pulse();
                LastTick = DateTime.Now;
            }
        }

        public override void Stop()
        {
        }

        public void Pulse()
        {
            
        }

        public void Execute(int amount, int healerId = 0, HealType healType = HealType.HEALTH)
        {
            if (amount < 0)
                return;

            var newAmount = amount;
            switch (healType)
            {
                case HealType.HEALTH:
                    newAmount = Character.CurrentHealth + amount;
                    Character.CurrentHealth = newAmount;
                    break;
                case HealType.SHIELD:
                    newAmount = Character.CurrentHealth + amount;
                    Character.CurrentShield = newAmount;
                    break;
            }

            if (Character is Player && healType == HealType.HEALTH)
                Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Character.Id), "0|A|HL|" + healerId + "|" + Character.Id + "|HPT|" + Character.CurrentHealth + "|" +
                                                                                               amount);
            else if (Character is Player && healType == HealType.SHIELD)
                Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Character.Id), "0|A|HL|" + healerId + "|" + Character.Id + "|SHD|" + Character.CurrentShield + "|" +
                                                                                               amount);

            Character.Update();
        }
    }
}
