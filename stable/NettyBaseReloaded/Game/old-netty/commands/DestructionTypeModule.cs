using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class DestructionTypeModule
    {
        public const short ID = 19986;

        public const short PLAYER = 0;

        public const short NPC = 1;

        public const short RADITATION = 2;

        public const short MINE = 3;

        public const short MISC = 4;

        public const short BATTLESTATION = 5;

        public short type = 0;

        public DestructionTypeModule(short type)
        {
            this.type = type;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(type);
            return cmd.Message.ToArray();
        }
    }
}
