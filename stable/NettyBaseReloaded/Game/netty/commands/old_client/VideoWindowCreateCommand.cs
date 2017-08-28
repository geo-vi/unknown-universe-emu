using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class VideoWindowCreateCommand
    {
        public const short HELPMOVIE = 0;

        public const short COMMANDER = 1;

        public const short ID = 11048;

        public static byte[] write(int windowID, string windowAlign, bool showButtons, List<string> languageKeys, int videoID, short videoType)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(windowID);
            cmd.UTF(windowAlign);
            cmd.Boolean(showButtons);
            cmd.Integer(languageKeys.Count);
            foreach (var _loc2_ in languageKeys)
            {
                cmd.UTF(_loc2_);
            }
            cmd.Integer(videoID);
            cmd.Short(videoType);
            return cmd.ToByteArray();
        }
    }
}