using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class AudioSettingsModule
    {
        public const short ID = 32696;

        private bool varN2e;
        private int sound;
        private int music;
        private int vars2M;
        private bool playCombatMusic;

        public AudioSettingsModule(bool varN2E, int sound, int music, short vars2M, bool playCombatMusic)
        {
            this.varN2e = varN2E;
            this.sound = sound;
            this.music = music;
            this.vars2M = vars2M;
            this.playCombatMusic = playCombatMusic;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(playCombatMusic);
            cmd.Integer(sound << 1 | sound >> 31);
            cmd.Integer(vars2M >> 10 | vars2M << 22);
            cmd.Boolean(varN2e);
            cmd.Integer(music << 13 | music >> 19);
            return cmd.Message.ToArray();
        }
    }
}
