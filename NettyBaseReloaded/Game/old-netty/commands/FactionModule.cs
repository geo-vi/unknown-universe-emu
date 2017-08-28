using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class FactionModule
    {
        public static short NONE = 0;
        public static short MMO = 1;
        public static short EIC = 2;
        public static short VRU = 3;

        public const short ID = 15721;

        public short faction = 0;

        public FactionModule(short faction)
        {
            this.faction = faction;
        }

        public byte[] write()
        {
            ByteArray enc = new ByteArray(ID);
            enc.Short(faction);
            return enc.Message.ToArray();
        }

    }
}
