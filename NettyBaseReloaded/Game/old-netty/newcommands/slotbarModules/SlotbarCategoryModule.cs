using System.Collections.Generic;

namespace NettyBaseReloaded.Game.netty.newcommands.slotbarModules
{
	class SlotbarCategoryModule : IServerCommand
	{
		public static int ID = 21275;

        public string name;
        public List<SlotbarCategoryItemModule> items;

        public SlotbarCategoryModule(string name, List<SlotbarCategoryItemModule> items)
        {
            this.name = name;
            this.items = items;
        }

	    public override void write()
	    {
            writeShort(ID);
            writeInt(items.Count);
            foreach (var c in items)
            {
                c.write();
                writeBytes(c.command.ToArray());
            }
            writeShort(-25751);
            writeUTF(name);
        }
	}
}
