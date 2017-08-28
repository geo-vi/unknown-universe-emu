using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    class MoveRequest
    {
        public const short ID = 11943;

        public int NewY = 0;
        public int OldY = 0;
        public int NewX = 0;
        public int OldX = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);

            this.NewY = parser.readInt();
            this.NewY = (int)(((uint)this.NewY >> 6) | ((uint)this.NewY << 26));
            this.OldY = parser.readInt();
            this.OldY = (int)(((uint)this.OldY << 9) | ((uint)this.OldY >> 23));
            this.NewX = parser.readInt();
            this.NewX = (int)(((uint)this.NewX << 1) | ((uint)this.NewX >> 31));
            this.OldX = parser.readInt();
            this.OldX = (int)(((uint)this.OldX >> 10) | ((uint)this.OldX << 22));
        }
    }
}
