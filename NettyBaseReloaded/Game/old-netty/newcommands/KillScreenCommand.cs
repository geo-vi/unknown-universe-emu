using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.newcommands.killscreenModules;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class KillScreenCommand : SimpleCommand
	{
		public static int ID = 5565;

		public KillScreenCommand(string varf32, string varT4x, string varI3l, KillscreenDestroyerModule varK3b, List<KillscreenOptionsModule> options)
		{
			writeShort(ID);
			writeInt(options.Count);
		    foreach (var c in options)
		    {
		        c.write();
                writeBytes(c.command.ToArray());
		    }
			writeUTF(varT4x);
			writeShort(-22236);
			writeShort(5954);
			writeUTF(varI3l);
            varK3b.write();
            writeBytes(varK3b.command.ToArray());
			writeUTF(varf32);
		}
	}
}
