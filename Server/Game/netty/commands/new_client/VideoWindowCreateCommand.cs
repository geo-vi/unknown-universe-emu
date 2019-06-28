using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.new_client
{
    class VideoWindowCreateCommand
    {
        public const short HELPMOVIE = 0;

        public const short COMMANDER = 1;

        public const short ID = 26804;

        public static byte[] write(int windowID, string windowAlign, bool showButtons, List<string> languageKeys,
            int videoID, short videoType)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(windowAlign);
            cmd.Integer(languageKeys.Count);
            foreach (var _loc2_ in languageKeys)
            {
                cmd.UTF(_loc2_);
            }
            cmd.Short(26779);
            cmd.Boolean(showButtons);
            cmd.Short(videoType);
            cmd.Integer(windowID >> 8 | windowID << 24);
            cmd.Integer(videoID >> 10 | videoID << 22);
            return cmd.ToByteArray();
        }
    }
}