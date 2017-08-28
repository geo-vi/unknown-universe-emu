using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AudioSettingsModule
    {
        public const short ID = 26805;

        public Boolean notSet = false;
      
        public Boolean sound = false;
      
        public Boolean music = false;

        public AudioSettingsModule(bool param1, bool param2, bool param3)
        {
            this.notSet = param1;
            this.sound = param2;
            this.music = param3;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(notSet);
            cmd.Boolean(sound);
            cmd.Boolean(music);
            return cmd.Message.ToArray();
        }
    }
}
