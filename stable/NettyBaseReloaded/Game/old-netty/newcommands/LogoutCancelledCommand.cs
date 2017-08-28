namespace NettyBaseReloaded.Game.netty.newcommands
{
	class LogoutCancelledCommand : SimpleCommand
	{
		public static int ID = 10020;
		public LogoutCancelledCommand()
		{
			writeShort(ID);
		}
	}
}
