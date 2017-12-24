using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class LogMessage
    {
        public enum LogType
        {
            DEBUG,
            SYSTEM,
            NORMAL
        }

        public string Key { get; }

        public DateTime TimeSent { get; }

        public LogType Type { get; }

        public LogMessage(string key, LogType type)
        {
            Key = key;
            Type = type;
            TimeSent = DateTime.Now;
        }
    }
}
