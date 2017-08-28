namespace NettyBaseReloaded.Game.netty.newcommands
{
	class ShipDestroyCommand : SimpleCommand
	{
		public static int ID = 649;

		public ShipDestroyCommand(int playerId, int value)
		{
			writeShort(ID);
            writeInt(playerId >> 5 | playerId << 27);
            writeShort(-5562);
            writeInt(value << 4 | value >> 28);
        }
	}
}
