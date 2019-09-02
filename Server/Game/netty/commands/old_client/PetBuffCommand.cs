using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class PetBuffCommand
    {
        public const short ADD = 0;

        public const short REMOVE = 1;

        public const short ID = 29824;

        public static Command write(short effectAction, short effectId, List<int> addingParameters)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(effectAction);
            cmd.Short(effectId);
            cmd.Integer(addingParameters.Count);
            foreach (var addedParam in addingParameters)
                cmd.Integer(addedParam);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
