namespace NettyBaseReloaded.Game.netty.newcommands
{
	class LegacyModule : SimpleCommand
	{
		public static int ID = 32601;
		public LegacyModule(string message)
		{
			writeShort(ID);
			writeUTF(message);
		}
	}
}
