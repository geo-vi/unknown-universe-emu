using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class LogMessage
    {
        public string Key { get; }

        public DateTime TimeSent { get; }

        public LogMessage(string key)
        {
            Key = key;
            TimeSent = DateTime.Now;
        }
    }
}
