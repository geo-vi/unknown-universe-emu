namespace NettyBaseReloaded.Game.netty.newcommands.killscreenModules
{
	class KillscreenOptionsModule : IServerCommand
	{
		public static int ID = 16458;

	    public int var11h;
        public KillscreenButtonModule varxo;
	    public command64J toolTipKey;
	    public command64J varg2i;
        public command64J vart15;
        public commandX3Z price;
	    public command64J varq36;
        public bool vari3H;

	    public KillscreenOptionsModule(int var11H, KillscreenButtonModule varxo, command64J toolTipKey, command64J varg2I, command64J vart15, commandX3Z price, command64J varq36, bool vari3H)
	    {
	        var11h = var11H;
	        this.varxo = varxo;
	        this.toolTipKey = toolTipKey;
	        varg2i = varg2I;
	        this.vart15 = vart15;
	        this.price = price;
	        this.varq36 = varq36;
	        this.vari3H = vari3H;
	    }

	    public override void write()
	    {
            writeShort(ID);
            writeInt(var11h >> 7 | var11h << 25);
            varxo.write();
            writeBytes(varxo.command.ToArray());
            toolTipKey.write();
            writeBytes(toolTipKey.command.ToArray());
            varg2i.write();
            writeBytes(varg2i.command.ToArray());
            vart15.write();
            writeBytes(vart15.command.ToArray());
            price.write();
            writeBytes(price.command.ToArray());
            varq36.write();
            writeBytes(varq36.command.ToArray());
            writeBoolean(vari3H);
        }
	}
}
