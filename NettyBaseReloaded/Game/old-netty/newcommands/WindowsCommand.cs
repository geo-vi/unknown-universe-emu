using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.newcommands.windowModules;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class WindowsCommand : IServerCommand
	{
		public static int ID = 12242;

	    private readonly List<commandKn> _slotbars; 

		public WindowsCommand(List<commandKn> slotbars)
		{
		    _slotbars = slotbars;
		}

	    public override void write()
	    {
            writeShort(ID);
            writeShort(14628);
            writeInt(_slotbars.Count);
            foreach (commandKn c in _slotbars)
            {
                c.write();
                writeBytes(c.command.ToArray());
            }
            writeShort(32289);
        }
	}
}
