using Server.Utils;
using System;

namespace Server.Game.netty.commands.old_client
{
    class AudioSettingsModule
    {
        public const short ID = 26805;

        public Boolean notSet = false;

        public Boolean sound = false;

        public Boolean music = false;

        public AudioSettingsModule(bool param1 = true, bool param2 = false, bool param3 = false)
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
