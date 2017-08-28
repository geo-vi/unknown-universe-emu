using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class UserKeyBindingsUpdate
    {
        public const short ID = 23508;

        public static byte[] write(List<object> changedKeyBindings, bool remove)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(changedKeyBindings.Count);
            foreach (UserKeyBindingsModule loc in changedKeyBindings)
            {
                cmd.AddBytes(loc.write());
            }
            cmd.Boolean(remove);
            return cmd.ToByteArray();
        }
    }
}