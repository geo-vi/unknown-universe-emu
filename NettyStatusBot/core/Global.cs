using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Helper.objects;

namespace NettyStatusBot.core
{
    static class Global
    {
        public static int ProcessId { get; set; }

        public static Process EmulatorProcess => Process.GetProcessById(ProcessId);

        public static List<ConnectedPlayer> ConnectedPlayers = new List<ConnectedPlayer>();
    }
}
