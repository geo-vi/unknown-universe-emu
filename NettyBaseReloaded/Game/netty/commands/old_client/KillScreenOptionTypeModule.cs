using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class KillScreenOptionTypeModule
    {
        public const short FREE_PHOENIX = 0;
      
        public const short BASIC_REPAIR = 1;
      
        public const short AT_JUMPGATE_REPAIR = 2;
      
        public const short AT_DEATHLOCATION_REPAIR = 3;
      
        public const short AT_SECTOR_CONTROL_SPAWNPOINT = 4;
      
        public const short EXIT_SECTOR_CONTROL = 5;
      
        public const short BASIC_FULL_REPAIR = 6;
      
        public const short ID = 25757;

        public short repairTypeValue;

        public KillScreenOptionTypeModule(short repairTypeValue)
        {
            this.repairTypeValue = repairTypeValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(repairTypeValue);
            return cmd.Message.ToArray();
        }
    }
}
