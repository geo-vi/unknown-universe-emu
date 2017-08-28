namespace NettyBaseReloaded.Game.netty.newcommands.settingsModules
{
	class GameplaySettingsModule : IServerCommand
	{
        public static int ID = 28409;
        public bool O2W;
        public bool autoBoost;
        public bool autoRefinement;
        public bool D4d;
        public bool showBattlerayNotifications;
        public bool N2G;
        public bool N2e;
        public bool F3w;
        public bool HD;
        public bool D4P;

        public GameplaySettingsModule(bool o2W, bool autoBoost, bool autoRefinement, bool d4D, bool showBattlerayNotifications, bool n2G, bool n2E, bool f3W, bool hd, bool d4P)
        {
            O2W = o2W;
            this.autoBoost = autoBoost;
            this.autoRefinement = autoRefinement;
            D4d = d4D;
            this.showBattlerayNotifications = showBattlerayNotifications;
            N2G = n2G;
            N2e = n2E;
            F3w = f3W;
            HD = hd;
            D4P = d4P;
        }

        public override void write()
        {
            writeShort(ID);
            writeBoolean(this.O2W);
            writeBoolean(this.autoBoost);
            writeBoolean(this.autoRefinement);
            writeBoolean(this.D4d);
            writeShort(-9045);
            writeBoolean(this.showBattlerayNotifications);
            writeBoolean(this.N2G);
            writeBoolean(this.N2e);
            writeBoolean(this.F3w);
            writeBoolean(this.HD);
            writeBoolean(this.D4P);
        }
    }
}
