using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ReadyRequest
    {
        public const short EMPTY = 0;

        public const short SHIP_LOADED = 1;

        public const short UI_WINDOWS_LOADED = 2;

        public const short UI_QUICKSLOT_LOADED = 3;

        public const short CHAT_LOADED = 4;

        public const short LOG_LOADED = 5;

        public const short MAP_LOADED = 6;

        public const short ID = 28114;
    }
}
