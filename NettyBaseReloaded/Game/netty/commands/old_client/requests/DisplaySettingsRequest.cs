using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class DisplaySettingsRequest
    {
        public const short ID = 15703;

        public bool displayPlayerName = false;
        public bool displayResources = false;
        public bool displayBoxes = false;
        public bool displayHitpointBubbles = false;
        public bool displayChat = false;
        public bool displayDrones = false;
        public bool displayCargoboxes = false;
        public bool displayPenaltyCargoboxes = false;
        public bool displayWindowBackground = false;
        public bool displayNotifications = false;
        public bool preloadUserShips = false;
        public bool alwaysDraggableWindows = false;
        public bool shipHovering = false;
        public bool showSecondQuickslotBar = false;
        public bool useAutoQuality = false;

        public void readCommand(IByteBuffer bytes)
        {
            var cmd = new ByteParser(bytes);
            this.displayPlayerName = cmd.readBool();
            this.displayResources = cmd.readBool();
            this.displayBoxes = cmd.readBool();
            this.displayHitpointBubbles = cmd.readBool();
            this.displayChat = cmd.readBool();
            this.displayDrones = cmd.readBool();
            this.displayCargoboxes = cmd.readBool();
            this.displayPenaltyCargoboxes = cmd.readBool();
            this.displayWindowBackground = cmd.readBool();
            this.displayNotifications = cmd.readBool();
            this.preloadUserShips = cmd.readBool();
            this.alwaysDraggableWindows = cmd.readBool();
            this.shipHovering = cmd.readBool();
            this.showSecondQuickslotBar = cmd.readBool();
            this.useAutoQuality = cmd.readBool();
        }
    }
}
