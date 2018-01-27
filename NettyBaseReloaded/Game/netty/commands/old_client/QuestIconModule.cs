using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuestIconModule
    {
        public const short KILL = 0;

        public const short COLLECT = 1;

        public const short DISCOVER = 2;

        public const short PVP = 3;

        public const short TIME = 4;

        public const short SUMMERGAMES3 = 5;

        public const short WINTERGAMES09 = 6;

        public const short HALLOWEEN2012 = 7;

        public const short ID = 2882;

        public short icon;
        public QuestIconModule(short icon)
        {
            this.icon = icon;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(icon);
            return cmd.Message.ToArray();
        }
    }
}
