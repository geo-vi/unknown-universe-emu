using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    public class UserKeyBindingsUpdate
    {
        public const short ID = 23932;

        public static byte[] write(List<object> keys, bool remove)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(7939);
            cmd.Boolean(remove);
            cmd.Integer(keys.Count);
            foreach (UserKeyBindingsModule c in keys)
            {
                cmd.AddBytes(c.write());
            }
            return cmd.ToByteArray();
        }
    }
}