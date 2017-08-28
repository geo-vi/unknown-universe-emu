namespace NettyBaseReloaded.Game.netty.newcommands
{
	class PoizoneCommand : SimpleCommand
	{
		public static int ID = 311;

		public PoizoneCommand(int vare4D, bool varD3q, int vars1H, bool varA3j, int varBk, int varZ4W, bool varxl, bool varvG, string varfS)
		{
			writeShort(ID);
			writeInt(vare4D << 3 | vare4D >> 29);
			writeBoolean(varD3q);
			writeShort(-1851);
			writeInt(vars1H << 3 | vars1H >> 29);
			writeBoolean(varA3j);
			writeInt(varBk >> 1 | varBk << 31);
			writeInt(varZ4W << 14 | varZ4W >> 18);
			writeBoolean(varxl);
			writeBoolean(varvG);
			writeUTF(varfS);
		}
	}
}
