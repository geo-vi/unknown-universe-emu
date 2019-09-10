using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;

namespace Server.Game.controllers.characters
{
    class ShipEffectController : AbstractedSubController
    {
        public void InstantShield()
        {
            //PrebuiltRangeCommands.Instance.InstantShieldCommand();
        }

        public void Emp()
        {
            //PrebuiltRangeCommands.Instance.EmpCommand();
        }

        public void SmartBomb()
        {
            //PrebuiltRangeCommands.Instance.SmartbombCommand();
        }
    }
}