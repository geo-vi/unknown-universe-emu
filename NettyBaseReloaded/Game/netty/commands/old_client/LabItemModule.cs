using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class LabItemModule
    {
        public const short ID = 31721;

        public const short LASER = 0;
      
        public const short ROCKETS = 1;
      
        public const short DRIVING = 2;
      
        public const short SHIELD = 3;

        public short itemValue;

        public LabItemModule(short itemValue)
        {
            this.itemValue = itemValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(itemValue);
            return cmd.Message.ToArray();
        }

        public void read(ByteParser parser)
        {
            parser.readShort();
            itemValue = parser.readShort();
        }
    }
}
