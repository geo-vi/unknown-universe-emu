using System.Collections.Generic;

namespace NettyBaseReloadedBrowser.Game.netty.commands
{
    class HotkeysCommand : SimpleCommand
    {
        public static int ID = 23932;

        public HotkeysCommand(List<HotkeysModuleCommand> keys, bool remove)
        {
            writeShort(ID);
            writeShort(7939);
            writeBoolean(remove);
            writeInt(keys.Count);
            foreach (var c in keys)
            {
                c.write();
                writeBytes(c.command.ToArray());
            }
        }
    }
}