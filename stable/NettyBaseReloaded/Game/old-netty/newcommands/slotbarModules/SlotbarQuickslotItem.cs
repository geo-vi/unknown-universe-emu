namespace NettyBaseReloaded.Game.netty.newcommands.slotbarModules
{
	class SlotbarQuickslotItem : IServerCommand
	{
		public static int ID = 9992;

	    public int slotId;
	    public string lootId;

	    public SlotbarQuickslotItem(int slotId, string lootId)
	    {
	        this.slotId = slotId;
	        this.lootId = lootId;
	    }

	    public override void write()
	    {
            writeShort(ID);
            writeInt(slotId >> 3 | slotId << 29);
            writeUTF(lootId);
        }
	}
}
