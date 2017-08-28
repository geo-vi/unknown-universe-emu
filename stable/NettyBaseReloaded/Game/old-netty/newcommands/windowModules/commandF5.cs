namespace NettyBaseReloaded.Game.netty.newcommands.windowModules
{
	class commandF5 : IServerCommand
	{
		public static int ID = 23340;

        public string replacement = "";
      
        public commandWw vare23;
      
        public string wildcard = "";


		public commandF5(string wildcard, string replacement, commandWw vare23)
	    {
	        this.replacement = replacement;
	        this.wildcard = wildcard;
		    this.vare23 = vare23;
	    }

	    public override void write()
	    {
            writeShort(ID);
            writeUTF(replacement);
            vare23.write();
            writeBytes(vare23.command.ToArray());
            writeUTF(wildcard);
        }
	}
}
