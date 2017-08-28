using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ClanRelationModule
    {
        public const short NONE = 0;

        public const short ALLIED = 1;

        public const short NON_AGGRESSION_PACT = 2;

        public const short AT_WAR = 3;

        public const short ID = 12581;

        public short type = 0;

        public ClanRelationModule(short type)
        {
            this.type = type;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(type);
            return cmd.Message.ToArray();
        }
    }
}
