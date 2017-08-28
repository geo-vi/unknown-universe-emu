using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.netty
{
    class Command
    {
        public byte[] Bytes;
        public bool NewClient;

        public Command(byte[] bytes, bool newClient)
        {
            Bytes = bytes;
            NewClient = newClient;
        }
    }
}
