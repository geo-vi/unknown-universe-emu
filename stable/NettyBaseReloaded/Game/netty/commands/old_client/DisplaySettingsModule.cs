using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class DisplaySettingsModule
    {
        public const short ID = 1583;

        public bool notSet = false;
      
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

        public DisplaySettingsModule(bool param1 = false, bool param2 = false, bool param3 = false, bool param4 = false, bool param5 = false, bool param6 = false, bool param7 = false, bool param8 = false, bool param9 = false, bool param10 = false, bool param11 = false, bool param12 = false, bool param13 = false, bool param14 = false, bool param15 = false, bool param16 = false)
        {
            this.notSet = param1;
            this.displayPlayerName = param2;
            this.displayResources = param3;
            this.displayBoxes = param4;
            this.displayHitpointBubbles = param5;
            this.displayChat = param6;
            this.displayDrones = param7;
            this.displayCargoboxes = param8;
            this.displayPenaltyCargoboxes = param9;
            this.displayWindowBackground = param10;
            this.displayNotifications = param11;
            this.preloadUserShips = param12;
            this.alwaysDraggableWindows = param13;
            this.shipHovering = param14;
            this.showSecondQuickslotBar = param15;
            this.useAutoQuality = param16;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(this.notSet);
            cmd.Boolean(this.displayPlayerName);
            cmd.Boolean(this.displayResources);
            cmd.Boolean(this.displayBoxes);
            cmd.Boolean(this.displayHitpointBubbles);
            cmd.Boolean(this.displayChat);
            cmd.Boolean(this.displayDrones);
            cmd.Boolean(this.displayCargoboxes);
            cmd.Boolean(this.displayPenaltyCargoboxes);
            cmd.Boolean(this.displayWindowBackground);
            cmd.Boolean(this.displayNotifications);
            cmd.Boolean(this.preloadUserShips);
            cmd.Boolean(this.alwaysDraggableWindows);
            cmd.Boolean(this.shipHovering);
            cmd.Boolean(this.showSecondQuickslotBar);
            cmd.Boolean(this.useAutoQuality);
            return cmd.Message.ToArray();
        }
    }
}
