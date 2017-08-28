using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestTypeModule
    {
        public const short UNDEFINED = 0;

        public const short STARTER = 1;

        public const short MISSION = 2;

        public const short DAILY = 3;

        public const short CHALLENGE = 4;

        public const short EVENT = 5;

        public const short ID = 2263;

        public short type;

        public QuestTypeModule(short type)
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
