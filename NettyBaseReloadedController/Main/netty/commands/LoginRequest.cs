using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Utils;

namespace NettyBaseReloadedController.Main.netty.commands
{
    class LoginRequest
    {
        public const short ID = 1;
        public static byte[] write(string username, string password)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(username);
            cmd.UTF(password);
            return cmd.ToByteArray();
        }
    }
}
