using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class DroneFormationAvailableFormationsCommand
    {
        public const short ID = 4479;

        public static byte[] write(List<int> droneFormations)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(droneFormations.Count);
            foreach (var droneFormation in droneFormations)
                cmd.Integer(droneFormation);
            return cmd.ToByteArray();
        }
    }
}
