namespace NettyBaseReloadedBrowser.Game.netty.commands
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
