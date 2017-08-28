using System.Collections.Generic;
using NettyBaseReloaded.Chat.objects.chat;

namespace NettyBaseReloaded.Chat.objects.storages
{
    class RoomsStorage
    {
        public Dictionary<int, Room> Loaded = new Dictionary<int, Room>();
    }
}