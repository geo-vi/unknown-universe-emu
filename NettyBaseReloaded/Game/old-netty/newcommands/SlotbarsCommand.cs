using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.newcommands.slotbarModules;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class SlotbarsCommand : SimpleCommand
	{
		public static int ID = 10437;

		public SlotbarsCommand(List<SlotbarCategoryModule> categories, string varnz, List<SlotbarQuickslotModule> slotBars)
		{
			writeShort(ID);
			writeInt(categories.Count);
		    foreach (var c in categories)
		    {
		        c.write();
                writeBytes(c.command.ToArray());
		    }
			writeShort(7085);
			writeUTF(varnz);
            writeInt(slotBars.Count);
            foreach (var c in slotBars)
            {
                c.write();
                writeBytes(c.command.ToArray());
            }
        }
	}
}
