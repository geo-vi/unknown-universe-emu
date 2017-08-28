using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    public class UserKeyBindingsModule
    {
        public const short ID = 1168;

        public short charCode = 0;
        public short actionType = 0;
        public int parameter = 0;
        public List<int> keyCodes;

        public UserKeyBindingsModule(short actionType, List<int> keyCodes, int parameter, short charCode)
        {
            this.charCode = charCode;
            this.actionType = actionType;
            this.parameter = parameter;
            this.keyCodes = keyCodes;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(this.charCode);
            cmd.Short(-23840);
            cmd.Short(this.actionType);
            cmd.Integer(this.parameter << 5 | this.parameter >> 27);
            cmd.Short(10665);
            cmd.Integer(this.keyCodes.Count);
            foreach (var c in keyCodes)
            {
                cmd.Integer(c >> 5 | c << 27);
            }
            return cmd.Message.ToArray();
        }
    }
}