using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.newcommands.windowModules;

namespace NettyBaseReloaded.Game.netty.newcommands.killscreenModules
{
	class command64J : IServerCommand
	{
		public static int ID = 19662;

	    public string baseKey;
	    public commandWw varT1J;
	    public List<commandF5> vard3d;

	    public command64J(string baseKey, commandWw varT1J, List<commandF5> vard3D)
	    {
	        this.baseKey = baseKey;
	        this.varT1J = varT1J;
	        vard3d = vard3D;
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
            writeShort(-21482);
            writeShort(20005);
            varT1J.write();
            writeBytes(varT1J.command.ToArray());
            writeUTF(baseKey);
        }
	}
}
