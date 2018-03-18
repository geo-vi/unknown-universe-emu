using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class PartnerTypeModule
    {
        public const short ANTEC = 0;
      
        public const short DARKORBIT = 1;
      
        public const short RAZER = 2;
      
        public const short ID = 10084;

        public short typeValue;

        public PartnerTypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(typeValue);
            return cmd.Message.ToArray();
        }
    }
}
