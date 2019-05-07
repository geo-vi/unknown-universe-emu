using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    class UIOpenRequest
    {
        // this action is sent when user opens user stats window
        public const string ACTION_USER = "user";

        // this action is sent when user opens ship stats window
        public const string ACTION_SHIP = "ship";

        // this action is sent when user opens ship warp window ?????
        public const string ACTION_SHIP_WARP = "ship_warp";

        // this action is sent when user opens chat window
        public const string ACTION_CHAT = "chat";

        // this action is sent when user opens group window
        public const string ACTION_GROUP = "group";

        // this action is sent when user opens minimap window
        public const string ACTION_MINIMAP = "minimap";

        // this action is sent when user opens spacemap window
        public const string ACTION_SPACEMAP = "spacemap";

        // this action is sent when user opens missions window
        public const string ACTION_QUESTS = "quests";

        // this action is sent when user opens refinement window
        public const string ACTION_REFINEMENT = "refinement";

        // this action is sent when user opens log window
        public const string ACTION_LOG = "log";

        // this action is sent when user opens pet window
        public const string ACTION_PET = "pet";

        // this action is sent when user opens contacts window
        public const string ACTION_CONTACTS = "contacts";

        // this action is sent when user opens logout window
        public const string ACTION_LOGOUT = "logout";

        public const short ID = 13008;

        public string itemId;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            itemId = parser.readUTF();
        }
    }
}
