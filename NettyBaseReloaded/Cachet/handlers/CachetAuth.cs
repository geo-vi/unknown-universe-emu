using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;

namespace NettyBaseReloaded.Cachet.handlers
{
    class CachetAuth
    {
        private static Cachet Cachet;
        public static void InitiateAuth()
        {
            Cachet = new Cachet("http://status.univ3rse.com", "JTVOo4MZ5T11tAeAsBwG");
            Task.Factory.StartNew(StartListeningForUpdates);
        }

        private static async void StartListeningForUpdates()
        {
            //if (RetriveIP().ToString().StartsWith("192.168.0")) return;
            while (true)
            {
                Cachet.AddPoint(1, World.StorageManager.GameSessions.Count);
                await Task.Delay(30000);
            }
        }

        private static IPAddress RetriveIP()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    return addr;
                }
            }
            return new IPAddress(0);
        }
    }
}
