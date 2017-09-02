using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class MessageWildcardReplacementModule
    {
        public const short ID = 32205;

        public string wildcard;
        public string replacement;

        public MessageWildcardReplacementModule(string wildcard, string replacement)
        {
            this.wildcard = wildcard;
            this.replacement = replacement;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(wildcard);
            cmd.UTF(replacement);
            return cmd.Message.ToArray();
        }
    }
}
