using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class UserKeyBindingsUpdate
    {
        public const short ID = 23508;

        public static byte[] write(List<UserKeyBindingsModule> changedKeyBindings, bool remove)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(changedKeyBindings.Count);
            foreach (var loc in changedKeyBindings)
            {
                cmd.AddBytes(loc.write());
            }
            cmd.Boolean(remove);
            return cmd.ToByteArray();
        }
    }
}
