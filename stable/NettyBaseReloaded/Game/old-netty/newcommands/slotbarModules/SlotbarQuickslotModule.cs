using System.Collections.Generic;

namespace NettyBaseReloaded.Game.netty.newcommands.slotbarModules
{
	class SlotbarQuickslotModule : IServerCommand
	{
		public static int ID = 2545;

	    public string slotBarId;
	    public List<SlotbarQuickslotItem> var2Z;
	    public string position;
	    public string varW2e;
	    public bool visible;

	    public SlotbarQuickslotModule(string slotBarId, List<SlotbarQuickslotItem> var2Z, string position, string varW2E, bool visible)
	    {
	        this.slotBarId = slotBarId;
	        this.var2Z = var2Z;
	        this.position = position;
	        varW2e = varW2E;
	        this.visible = visible;
	    }

	    public override void write()
	    {
            writeShort(ID);
            writeUTF(position);
            writeShort(-31484);
            writeInt(var2Z.Count);
            foreach (var c in var2Z)
            {
                c.write();
                writeBytes(c.command.ToArray());
            }
            writeUTF(slotBarId);
            writeBoolean(visible);
            writeShort(-8177);
            writeUTF(varW2e);
        }
	}
}
