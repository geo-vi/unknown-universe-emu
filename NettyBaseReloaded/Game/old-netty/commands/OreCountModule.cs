using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class OreCountModule
    {
        public const short ID = 28044;

        public OreTypeModule oreType;
        public double count;

        public OreCountModule(OreTypeModule oreType, double count)
        {
            this.oreType = oreType;
            this.count = count;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(oreType.write());
            cmd.Double(count);
            return cmd.Message.ToArray();
        }
    }
}
