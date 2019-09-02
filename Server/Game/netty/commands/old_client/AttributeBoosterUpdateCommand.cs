using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class AttributeBoosterUpdateCommand
    {
        public const short ID = 13651;

        public static Command write(List<BoosterUpdateModule> boostedAttributes)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(boostedAttributes.Count);
            foreach (var booster in boostedAttributes)
            {
                cmd.AddBytes(booster.write());
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
