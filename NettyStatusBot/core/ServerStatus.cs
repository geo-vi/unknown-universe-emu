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
        public TimeSpan Runtime;
        public int PlayersOnline;
        public bool Online;
        public bool Maintenance;
    }
}
