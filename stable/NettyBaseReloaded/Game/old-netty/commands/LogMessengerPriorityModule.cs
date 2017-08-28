using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class LogMessengerPriorityModule
    {
        public const short ID = 61;

        public const short STANDARD = 0;

        public const short HIGH_PRIORITY = 1;

        public short priorityModeValue;

        public LogMessengerPriorityModule(short priorityModeValue)
        {
            this.priorityModeValue = priorityModeValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(priorityModeValue);
            return cmd.Message.ToArray();
        }
    }
}
