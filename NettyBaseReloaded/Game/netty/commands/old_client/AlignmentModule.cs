using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AlignmentModule
    {
        public const short NORTH = 0;
      
        public const short NORT_EAST = 1;
      
        public const short EAST = 2;
      
        public const short SOUTH_EAST = 3;
      
        public const short SOUTH = 4;
      
        public const short SOUTH_WEST = 5;
      
        public const short WEST = 6;
      
        public const short NORTH_WEST = 7;
      
        public const short CENTER = 8;
      
        public const short ID = 422;

        public short alignmentValue;

        public AlignmentModule(short alignmentValue)
        {
            this.alignmentValue = alignmentValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(alignmentValue);
            return cmd.Message.ToArray();
        }
    }
}
