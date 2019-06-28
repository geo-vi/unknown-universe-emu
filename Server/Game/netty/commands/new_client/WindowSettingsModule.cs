using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class WindowSettingsModule
    {
        public const short ID = 14110;

        private int varM3J;
        private string vark3j;
        private bool hideAllWindows;

        public WindowSettingsModule(int varM3J, string vark3J, bool hideAllWindows)
        {
            this.varM3J = varM3J;
            this.vark3j = vark3J;
            this.hideAllWindows = hideAllWindows;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(varM3J >> 11 | varM3J << 21);
            cmd.Boolean(hideAllWindows);
            cmd.UTF(vark3j);
            return cmd.Message.ToArray();
        }
    }
}
