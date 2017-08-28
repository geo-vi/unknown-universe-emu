namespace NettyBaseReloaded.Game.netty.newcommands.windowModules
{
    class WindowButtonModule : IServerCommand
    {

        public static int ID = 28947;

        public ClientUITooltip tooltip;

        public bool visible = false;
      
        public string itemId = "";

        public WindowButtonModule(string itemId, bool visible, ClientUITooltip tooltip)
        {
            this.visible = visible;
            this.itemId = itemId;
            this.tooltip = tooltip;
        }

        public override void write()
        {
            writeShort(ID);
            writeShort(13273);
            tooltip.write();
            writeBytes(tooltip.command.ToArray());
            writeBoolean(this.visible);
            writeUTF(this.itemId);
        }
    }
}
