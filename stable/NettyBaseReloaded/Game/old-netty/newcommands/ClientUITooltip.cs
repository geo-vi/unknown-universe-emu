using System.Collections.Generic;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class ClientUITooltip : IServerCommand
	{
		public static int ID = 11872;

	    private List<ClientUITooltipTextFormat> textFormat;

		public ClientUITooltip(List<ClientUITooltipTextFormat> textFormat)
		{
		    this.textFormat = textFormat;
		    
		}

	    public override void write()
	    {
            writeShort(ID);
            writeInt(textFormat.Count);
	        foreach (var c in textFormat)
	        {
	            c.write();
                writeBytes(c.command.ToArray());
            }
       }
	}
}
