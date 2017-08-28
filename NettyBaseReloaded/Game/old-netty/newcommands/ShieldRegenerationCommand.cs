namespace NettyBaseReloaded.Game.netty.newcommands
{
	class ShieldUpdateCommandCommand : SimpleCommand
	{
		public static int ID = 5107;

		public ShieldUpdateCommandCommand(int currentShield, int maxShield)
		{
			writeShort(ID);
            writeInt(maxShield << 5 | maxShield >> 27);
            writeInt(currentShield >> 11 | currentShield << 21);
        }
	}
}
