using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using Server.Game.objects.server;

namespace Server.Game.managers
{
    class WhitelistManager
    {
        private static WhitelistManager _instance;
        
        public static WhitelistManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WhitelistManager();
                }
                return _instance;
            }
        }

        private readonly ConcurrentBag<IPAddress> _whitelistedInstances = new ConcurrentBag<IPAddress>();
        
        private readonly ConcurrentBag<BlacklistedInstance> _blacklistedInstances = new ConcurrentBag<BlacklistedInstance>();
        
        public void AddToWhitelist(IPAddress ip)
        {
            _whitelistedInstances.Add(ip);
        }

        public void AddToBlacklist(IPAddress ip, string message)
        {
            var instance = new BlacklistedInstance 
                {Address = ip, Reason = message};
            _blacklistedInstances.Add(instance);
        }

        public bool IsInWhitelist(IPAddress ip)
        {
            return _whitelistedInstances.Contains(ip);
        }
    }
}