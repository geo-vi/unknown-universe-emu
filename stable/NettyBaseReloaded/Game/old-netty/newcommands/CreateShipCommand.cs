using System.Collections.Generic;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class CreateShipCommand : SimpleCommand
	{
		public static int ID = 25118;

		public CreateShipCommand(int varGX, string varmj, int varK4p, string varFO, string varj4s, int x, int y, int factionId, int varp3v, int varg1T, bool varn3s, ClanRelationModule varT1o, int varp3D, bool varW40, bool npc, bool cloaked, int varxI, int vari2a, List<VisualModifierCommand> varcc, commandK13 varP1K)
		{
			writeShort(ID);
			writeInt(x >> 2 | x << 30);
			writeInt(varxI >> 9 | varxI << 23);
			writeUTF(varj4s);
			writeInt(varGX << 6 | varGX >> 26);
            varP1K.write();
		    writeBytes(varP1K.command.ToArray());
			writeUTF(varFO);
			writeInt(varp3v << 11 | varp3v >> 21);
			writeBoolean(varn3s);
			writeInt(varK4p >> 12 | varK4p << 20);
			writeBoolean(npc);
			writeBoolean(varW40);
			writeInt(varp3D << 7 | varp3D >> 25);
			writeInt(vari2a >> 2 | vari2a << 30);
			writeBoolean(cloaked);
            varT1o.write();
            writeBytes(varT1o.command.ToArray());
			writeInt(factionId << 13 | factionId >> 19);
			writeInt(varcc.Count);
		    foreach (var c in varcc)
		    {
		        c.write();
                writeBytes(c.command.ToArray());
		    }
			writeInt(varg1T >> 3 | varg1T << 29);
			writeInt(y >> 15 | y << 17);
			writeUTF(varmj);
		}
	}
}
