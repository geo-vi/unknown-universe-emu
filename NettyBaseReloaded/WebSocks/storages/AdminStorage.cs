using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.WebSocks.objects;

namespace NettyBaseReloaded.WebSocks.storages
{
    class AdminStorage
    {
        public static ConcurrentDictionary<int, Admin> AuthorizedSessions = new ConcurrentDictionary<int, Admin>();

        public bool GetAdminAccount(int userId, out Admin admin)
        {
            var success = AuthorizedSessions.TryRemove(userId, out admin);
            return success;
        }
    }
}
