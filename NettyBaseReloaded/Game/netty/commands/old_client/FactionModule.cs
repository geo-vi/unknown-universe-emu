using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class FactionModule
    {
        public const short NONE = 0;
      
        public const short MMO = 1;
      
        public const short EIC = 2;
      
        public const short VRU = 3;
      
        public const short ID = 15721;

        public short faction;

        public FactionModule(short faction)
        {
            this.faction = faction;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(faction);
            return cmd.Message.ToArray();
        }
    }
}
