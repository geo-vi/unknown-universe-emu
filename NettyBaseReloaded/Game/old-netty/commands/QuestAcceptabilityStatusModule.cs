using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestAcceptabilityStatusModule
    {
        public const short NOT_ACCEPTABLE = 0;

        public const short NOT_STARTED = 1;

        public const short RUNNING = 2;

        public const short COMPLETED = 3;

        public const short ID = 6093;

        public short type;

        public QuestAcceptabilityStatusModule(short type)
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
