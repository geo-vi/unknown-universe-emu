using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyStatusBot.Properties;

namespace NettyStatusBot.core
{
    struct ServerStatus
    {
        public static TimeSpan Runtime;
        public static int PlayersOnline;
        public static bool Maintenance => BotConfiguration.DISPLAY_MAINTENANCE_STATUS;
    }
}
