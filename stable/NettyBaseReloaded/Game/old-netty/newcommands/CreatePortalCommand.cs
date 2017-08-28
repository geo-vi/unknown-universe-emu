using System.Collections.Generic;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class CreatePortalCommand : SimpleCommand
	{
		public static int ID = 30093;

		public CreatePortalCommand(int varfi, int factionId, int varA2z, int x, int y, bool varX1v, bool vara3G, List<int> vara3D)
		{
			writeShort(ID);
			writeInt(factionId << 6 | factionId >> 26);
			writeInt(vara3D.Count);
		    foreach (var c in vara3D)
		    {
                writeInt(c >> 8 | c << 24);
            }
			writeInt(x >> 1 | x << 31);
			writeInt(varA2z << 1 | varA2z >> 31);
			writeBoolean(varX1v);
			writeBoolean(vara3G);
			writeInt(y >> 3 | y << 29);
			writeInt(varfi << 5 | varfi >> 27);
		}
	}
}
