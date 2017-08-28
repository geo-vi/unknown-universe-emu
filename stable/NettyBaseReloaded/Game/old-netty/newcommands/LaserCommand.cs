namespace NettyBaseReloaded.Game.netty.newcommands
{
	class LaserCommand : SimpleCommand
	{
		public static int ID = 4160;
		public LaserCommand(int vard3f, int varc23, int varki, bool vary2F, bool varP2W)
		{
			writeShort(ID);
			writeInt(varc23 << 3 | varc23 >> 29);
			writeShort(22550);
			writeInt(varki >> 8 | varki << 24);
			writeBoolean(varP2W);
			writeInt(vard3f << 7 | vard3f >> 25);
			writeBoolean(vary2F);
		}
	}
}
