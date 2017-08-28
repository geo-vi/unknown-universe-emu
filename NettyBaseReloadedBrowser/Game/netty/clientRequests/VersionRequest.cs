using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedBrowser.Utils;

namespace NettyBaseReloadedBrowser.Game.netty.clientRequests
{
    class VersionRequest
    {
        public const short ID = 666;
        public static byte[] write(int userId, string session)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.UTF(session);
            return cmd.ToByteArray();
        }
    }
}
