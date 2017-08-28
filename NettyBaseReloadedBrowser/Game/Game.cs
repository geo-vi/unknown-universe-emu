using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedBrowser.Game.objects;
using NettyBaseReloadedBrowser.Networking;
using NettyBaseReloadedBrowser.Networking.game;

namespace NettyBaseReloadedBrowser.Game
{
    class Game
    {
        public static GameClient GameClient { get; set; }
        public static GameServer GameServer { get; set; }
        public static Data UserData { get; set; }
    }
}
