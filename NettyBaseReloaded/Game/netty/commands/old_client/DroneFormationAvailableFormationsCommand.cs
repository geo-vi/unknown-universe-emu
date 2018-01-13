using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class DroneFormationAvailableFormationsCommand
    {
        public const short ID = 4479;

        public static Command write(List<int> availableFormations)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(availableFormations.Count);
            foreach (var formation in availableFormations)
            {
                cmd.Integer(formation);
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
