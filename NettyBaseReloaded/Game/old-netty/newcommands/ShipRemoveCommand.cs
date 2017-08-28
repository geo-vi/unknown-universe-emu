namespace NettyBaseReloaded.Game.netty.newcommands
{
	class ShipRemoveCommand : SimpleCommand
	{
		public static int ID = 14332;
		public ShipRemoveCommand(int uid)
		{
			writeShort(ID);
			writeShort(21523);
			writeInt(uid >> 9 | uid << 23);
		}
	}
}
