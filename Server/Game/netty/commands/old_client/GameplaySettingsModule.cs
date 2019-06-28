using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class GameplaySettingsModule
    {
        public const short ID = 2278;
        public bool notSet = false;

        public bool autoBoost = false;

        public bool autoRefinement = false;

        public bool quickslotStopAttack = false;

        public bool doubleclickAttack = false;

        public bool autoChangeAmmo = false;

        public bool autoStart = false;

        public bool autoBuyGreenBootyKeys = false;

        public GameplaySettingsModule(bool param1 = false, bool param2 = false, bool param3 = false, bool param4 = false, bool param5 = false, bool param6 = false, bool param7 = false, bool param8 = false)
        {
            this.notSet = param1;
            this.autoBoost = param2;
            this.autoRefinement = param3;
            this.quickslotStopAttack = param4;
            this.doubleclickAttack = param5;
            this.autoChangeAmmo = param6;
            this.autoStart = param7;
            this.autoBuyGreenBootyKeys = param8;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(this.notSet);
            cmd.Boolean(this.autoBoost);
            cmd.Boolean(this.autoRefinement);
            cmd.Boolean(this.quickslotStopAttack);
            cmd.Boolean(this.doubleclickAttack);
            cmd.Boolean(this.autoChangeAmmo);
            cmd.Boolean(this.autoStart);
            cmd.Boolean(this.autoBuyGreenBootyKeys);
            return cmd.Message.ToArray();
        }
    }
}
