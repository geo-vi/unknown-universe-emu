namespace NettyBaseReloaded.Game.netty.newcommands
{
	class ShipSelectionCommand : SimpleCommand
	{
		public static int ID = 10954;

		public ShipSelectionCommand(int varGX, int varc2p, int shield, int varY2s, int varp2c, int var545, int varU4r, int varZ1X, bool vard37)
		{
			writeShort(ID);
			writeInt(varZ1X >> 13 | varZ1X << 19);
			writeInt(varGX >> 11 | varGX << 21);
			writeInt(varc2p << 7 | varc2p >> 25);
			writeInt(varU4r << 3 | varU4r >> 29);
			writeInt(var545 >> 12 | var545 << 20);
			writeInt(varp2c >> 16 | varp2c << 16);
			writeInt(shield << 7 | shield >> 25);
			writeShort(-12272);
			writeInt(varY2s >> 12 | varY2s << 20);
			writeBoolean(vard37);
		}
	}
}
