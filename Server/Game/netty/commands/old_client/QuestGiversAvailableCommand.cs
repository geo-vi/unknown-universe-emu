using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class QuestGiversAvailableCommand
    {
        public const short ID = 29581;

        public static Command write(List<QuestGiverModule> questGivers)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(questGivers.Count);
            foreach (var questGiver in questGivers)
                cmd.AddBytes(questGiver.write());
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
