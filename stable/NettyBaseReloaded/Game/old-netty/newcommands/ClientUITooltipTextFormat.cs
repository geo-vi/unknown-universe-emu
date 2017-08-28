using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.newcommands.windowModules;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class ClientUITooltipTextFormat : IServerCommand
	{
		public static int ID = 1623;

        public static short STANDARD = 0;
        public static short RED = 1;

        public List<commandF5> vard3d;
      
        public string baseKey = "";
      
        public commandWw varT1J;
      
        public short textColor = 0;

		public ClientUITooltipTextFormat(short textColor, string baseKey, commandWw varT1J, List<commandF5> vard3d)
	    {
	        this.textColor = textColor;
		    this.baseKey = baseKey;
		    this.varT1J = varT1J;
		    this.vard3d = vard3d;
	    }

	    public override void write()
	    {
            writeShort(ID);
            writeInt(vard3d.Count);
	        foreach (var c in vard3d)
	        {
	            c.write();
                writeBytes(c.command.ToArray());
            }
            writeUTF(baseKey);
            writeShort(-20953);
            varT1J.write();
            writeBytes(varT1J.command.ToArray());
            writeShort(textColor);
        }
	}
}
