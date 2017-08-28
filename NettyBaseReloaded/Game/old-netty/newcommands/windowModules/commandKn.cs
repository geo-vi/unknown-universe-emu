using System.Collections.Generic;

namespace NettyBaseReloaded.Game.netty.newcommands.windowModules
{
	class commandKn : IServerCommand
	{
		public static int ID = 3444;

        public static short GENERIC_FEATURE_BAR = 2;
      
        public static short NOT_ASSIGNED = 0;
      
        public static short GAME_FEATURE_BAR = 1;

        public List<WindowButtonModule> varh1j;
      
        public string varA39 = "";
      
        public short varJ2d = 0;
      
        public string varh2J = "";

		public commandKn(short varJ2d, List<WindowButtonModule> varh1j, string varh2J, string varA39)
		{
		    this.varJ2d = varJ2d;
		    this.varh1j = varh1j;
		    this.varh2J = varh2J;
		    this.varA39 = varA39;
		}

	    public override void write()
	    {
            writeShort(ID);
            writeInt(varh1j.Count);
	        foreach (var c in varh1j)
	        {
	            c.write();
                writeBytes(c.command.ToArray());
            }
            writeUTF(varA39);
            writeShort(varJ2d);
            writeUTF(varh2J);
            writeShort(-15817);
            writeShort(13250);
        }
	}
}