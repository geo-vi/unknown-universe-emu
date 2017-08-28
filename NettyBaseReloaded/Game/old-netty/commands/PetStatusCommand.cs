using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class PetStatusCommand
    {
        public const short ID = 27211;

        public static byte[] write(int petId, int petLevel, double petExperiencePoints, double petExperiencePointsUntilNextLevel, int petHitPoints,
            int petHitPointsMax, int petShieldEnergyNow, int petShieldEnergyMax, int petCurrentFuel, int petMaxFuel, int petSpeed, string petName)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(petId);
            cmd.Integer(petLevel);
            cmd.Double(petExperiencePoints);
            cmd.Double(petExperiencePointsUntilNextLevel);
            cmd.Integer(petHitPoints);
            cmd.Integer(petHitPointsMax);
            cmd.Integer(petShieldEnergyNow);
            cmd.Integer(petShieldEnergyMax);
            cmd.Integer(petCurrentFuel);
            cmd.Integer(petMaxFuel);
            cmd.Integer(petSpeed);
            cmd.UTF(petName);
            return cmd.ToByteArray();
        }
    }
}
