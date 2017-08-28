using System.Collections.Generic;

namespace NettyBaseReloaded.Game.netty.newcommands
{
    class HotkeysModuleCommand : IServerCommand
    {
        public static int ID = 1168;

        public int charCode = 0;
        public short actionType = 0;
        public int parameter = 0;
        public List<int> keyCodes;

        public HotkeysModuleCommand(int charCode, short actionType, int parameter, List<int> keyCodes)
        {
            this.charCode = charCode;
            this.actionType = actionType;
            this.parameter = parameter;
            this.keyCodes = keyCodes;
        }

        public override void write()
        {
            writeShort(ID);
            writeShort(this.charCode);
            writeShort(-23840);
            writeShort(this.actionType);
            writeInt(this.parameter << 5 | this.parameter >> 27);
            writeShort(10665);
            writeInt(this.keyCodes.Count);
            foreach (var c in keyCodes)
            {
                writeInt(c >> 5 | c << 27);
            }
        }
    }
}
