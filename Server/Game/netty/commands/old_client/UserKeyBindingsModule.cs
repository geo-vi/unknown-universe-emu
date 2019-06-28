using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class UserKeyBindingsModule
    {
        public const short JUMP = 0;

        public const short CHANGE_CONFIG = 1;

        public const short ACTIVATE_LASER = 2;

        public const short LAUNCH_ROCKET = 3;

        public const short PET_ACTIVATE = 4;

        public const short PET_GUARD_MODE = 5;

        public const short LOGOUT = 6;

        public const short QUICKSLOT = 7;

        public const short QUICKSLOT_PREMIUM = 8;

        public const short TOGGLE_WINDOWS = 9;

        public const short PERFORMANCE_MONITORING = 10;

        public const short ZOOM_IN = 11;

        public const short ZOOM_OUT = 12;

        public const short PET_REPAIR_SHIP = 13;

        public const short ID = 24;

        public short actionType;
        public List<int> keyCodes;
        public int parameter;
        public short charCode;

        public UserKeyBindingsModule(short actionType, List<int> keyCodes, int parameter, short charCode)
        {
            this.actionType = actionType;
            this.keyCodes = keyCodes;
            this.parameter = parameter;
            this.charCode = charCode;
        }

        public UserKeyBindingsModule()
        {

        }

        public void readCommand(ByteParser parser)
        {
            parser.readShort();
            actionType = parser.readShort();
            var codesLength = parser.readInt();
            keyCodes = new List<int>();
            for (int i = 0; i < codesLength; i++)
            {
                keyCodes.Add(parser.readInt());
            }

            parameter = parser.readInt();
            charCode = parser.readShort();
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(actionType);
            cmd.Integer(keyCodes.Count);
            foreach (var _loc2_ in keyCodes)
            {
                cmd.Integer(_loc2_);
            }
            cmd.Integer(parameter);
            cmd.Short(charCode);
            return cmd.Message.ToArray();
        }
    }
}