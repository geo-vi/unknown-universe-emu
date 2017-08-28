namespace NettyBaseReloaded.Game.netty.newcommands.settingsModules
{
	class QuestSettingsModule : IServerCommand
	{
		public static int ID = 15633;

	    private bool varY1h;
	    private bool varBM;
	    private bool varCJ;
	    private bool varb33;
	    private bool varDb;
	    private bool varF45;

	    public QuestSettingsModule(bool varY1H, bool varBm, bool varCj, bool varb33, bool varDb, bool varF45)
	    {
	        varY1h = varY1H;
	        varBM = varBm;
	        varCJ = varCj;
	        this.varb33 = varb33;
	        this.varDb = varDb;
	        this.varF45 = varF45;
	    }

	    public override void write()
	    {
            writeShort(ID);
            writeShort(32721);
            writeBoolean(varDb);
            writeShort(19584);
            writeBoolean(varF45);
            writeBoolean(varY1h);
            writeBoolean(varb33);
            writeBoolean(varBM);
            writeBoolean(varCJ);
        }
	}
}
