namespace NettyBaseReloaded.Game.netty.newcommands.settingsModules
{
	class AudioSettingsModule : IServerCommand
	{
        public static int ID = 32696;

        private bool varN2e;
        private int sound;
        private int music;
        private int vars2M;
        private bool playCombatMusic;

        public AudioSettingsModule(bool varN2E, int sound, int music, int vars2M, bool playCombatMusic)
        {
            this.varN2e = varN2E;
            this.sound = sound;
            this.music = music;
            this.vars2M = vars2M;
            this.playCombatMusic = playCombatMusic;
        }

        public override void write()
        {
            writeShort(ID);
            writeBoolean(playCombatMusic);
            writeInt(sound << 1 | sound >> 31);
            writeInt(vars2M >> 10 | vars2M << 22);
            writeBoolean(varN2e);
            writeInt(music << 13 | music >> 19);
        }
    }
}
