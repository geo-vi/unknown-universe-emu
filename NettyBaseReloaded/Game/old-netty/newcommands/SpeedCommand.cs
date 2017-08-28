namespace NettyBaseReloaded.Game.netty.newcommands
{
	class SpeedCommand : SimpleCommand
	{
		public static int ID = 1506;
		public SpeedCommand(int realSpeed, int speed)
		{
			writeShort(ID);
			writeInt(speed >> 15 | speed << 17);
			writeShort(-20132);
			writeShort(22708);
			writeInt(realSpeed << 6 | realSpeed >> 26);
		}
	}
}
