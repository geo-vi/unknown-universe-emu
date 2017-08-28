using System.Collections.Generic;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class ShipInitializationCommand : SimpleCommand
	{
		public static int ID = 11451;

		public ShipInitializationCommand(int varGX, string varj4s, string varmj, int speed, int shield, int varY2s, int varW3j, int varO4v, int varc2e, int varn2b, int varU4r, int varZ1X, int x, int y, int mapId, int factionId, int varp3v, int varK4p, bool premium, double varQ1u, double varMe, int level, double credits, double uridium, float jackpot, int varg1T, string varFO, int varp3D, bool varW40, bool cloaked, bool var83D, List<VisualModifierCommand> modifiers)
		{
            writeShort(ID);
            writeInt(varn2b >> 12 | varn2b << 20);
            writeInt(speed >> 11 | speed << 21);
            writeUTF(varj4s);
            writeBoolean(var83D);
            writeInt(varW3j << 1 | varW3j >> 31);
            writeFloat(jackpot);
            writeBoolean(premium);
            writeInt(varp3v >> 11 | varp3v << 21);
            writeInt(y << 6 | y >> 26);
            writeUTF(varmj);
            writeInt(shield >> 9 | shield << 23);
            writeInt(varp3D << 5 | varp3D >> 27);
            writeShort(-27558);
            writeUTF(varFO);
            writeInt(mapId >> 14 | mapId << 18);
            writeInt(varY2s << 9 | varY2s >> 23);
            writeInt(level >> 7 | level << 25);
            writeInt(x << 5 | x >> 27);
            writeDouble(varQ1u);
            writeInt(factionId >> 5 | factionId << 27);
            writeInt(varc2e >> 3 | varc2e << 29);
            writeDouble(varMe);
            writeDouble(uridium);
            writeInt(varZ1X >> 5 | varZ1X << 27);
            writeInt(varg1T << 13 | varg1T >> 19);
            writeInt(varGX << 11 | varGX >> 21);
            writeInt(varO4v << 14 | varO4v >> 18);
            writeInt(modifiers.Count);
		    foreach (VisualModifierCommand c in modifiers)
		    {
		        c.write();
                writeBytes(c.command.ToArray());
		    }
            writeBoolean(cloaked);
            writeInt(varK4p >> 9 | varK4p << 23);
            writeInt(varU4r >> 3 | varU4r << 29);
            writeDouble(credits);
            writeBoolean(varW40);
        }
	}
}