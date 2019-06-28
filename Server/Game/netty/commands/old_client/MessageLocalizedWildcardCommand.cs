using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class MessageLocalizedWildcardCommand
    {
        public const short ID = 19731;

        public string baseKey;

        public List<MessageWildcardReplacementModule> wildCardReplacements;

        public MessageLocalizedWildcardCommand(string baseKey, List<MessageWildcardReplacementModule> wildCardReplacements)
        {
            this.baseKey = baseKey;
            this.wildCardReplacements = wildCardReplacements;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(baseKey);
            cmd.Integer(wildCardReplacements.Count);
            foreach (var replacement in wildCardReplacements)
            {
                cmd.AddBytes(replacement.write());
            }
            return cmd.Message.ToArray();
        }
    }
}
