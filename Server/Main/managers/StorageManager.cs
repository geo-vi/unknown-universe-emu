using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Main.objects;

namespace Server.Main.managers
{
    class StorageManager
    {
        public readonly string[] ENTRY_KEYS_RCON = { "VEZYREV3UIOT08E0" , "331MVPRDEVDDFJ2P", "P07QM3FSP2I3PYJR", "1XQ2IGUURJCCYB73", "1WRELPA5D6U9PVNB"};
        
        public ConcurrentDictionary<string, RconUser> RconUsers = new ConcurrentDictionary<string, RconUser>();
        
        public RconUser ConsoleUser = new RconUser("console", "127.0.0.1");
        
        public ConcurrentDictionary<int, Clan> Clans = new ConcurrentDictionary<int, Clan>();

        public void GetClanById(int clanId, bool nullReturn, out Clan clan)
        {
            clan = Clans[0];
        }
    }
}
