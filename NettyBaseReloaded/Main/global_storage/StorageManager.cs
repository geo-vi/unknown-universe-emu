using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.global_storage
{
    class StorageManager
    {
        public Dictionary<DateTime, Exception> Errors = new Dictionary<DateTime, Exception>();

        public Dictionary<int, Clan> Clans = new Dictionary<int, Clan>();

        public Clan GetClan(string tag)
        {
            return Clans.Values.FirstOrDefault(x => x.Tag == tag);
        }
    }
}