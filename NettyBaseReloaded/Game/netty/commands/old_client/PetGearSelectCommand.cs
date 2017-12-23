using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class PetGearSelectCommand
    {
        public const short ID = 19459;

        public static Command write(PetGearTypeModule gearType, List<int> optionalParams)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(gearType.write());
            cmd.Integer(optionalParams.Count);
            foreach (var param in optionalParams)
            {
                cmd.Integer(param);
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}