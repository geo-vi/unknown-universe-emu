using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class AbilityStatusFullCommand
    {
        public const short ID = 24975;

        public static Command write(List<AbilityStatusSingleCommand> abilities)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(abilities.Count);
            foreach (var ability in abilities)
                cmd.AddBytes(ability.write());
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
