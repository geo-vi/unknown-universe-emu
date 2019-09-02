using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class QuestSettingsModule
    {
        public const short ID = 15633;

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

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(32721);
            cmd.Boolean(varDb);
            cmd.Short(19584);
            cmd.Boolean(varF45);
            cmd.Boolean(varY1h);
            cmd.Boolean(varb33);
            cmd.Boolean(varBM);
            cmd.Boolean(varCJ);
            return cmd.Message.ToArray();
        }

    }
}
