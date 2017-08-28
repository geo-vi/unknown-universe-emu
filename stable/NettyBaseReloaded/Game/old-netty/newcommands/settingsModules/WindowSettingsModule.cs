namespace NettyBaseReloaded.Game.netty.newcommands.settingsModules
{
	class WindowSettingsModule : IServerCommand
	{
        public static int ID = 14110;

        private int varM3J;
        private string vark3j;
        private bool hideAllWindows;

        public WindowSettingsModule(int varM3J, string vark3J, bool hideAllWindows)
        {
            this.varM3J = varM3J;
            this.vark3j = vark3J;
            this.hideAllWindows = hideAllWindows;
        }

        public override void write()
        {
            writeShort(ID);
            writeInt(varM3J >> 11 | varM3J << 21);
            writeBoolean(hideAllWindows);
            writeUTF(vark3j);
        }
    }
}
