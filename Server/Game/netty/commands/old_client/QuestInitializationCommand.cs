using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class QuestInitializationCommand
    {
        public const short ID = 16927;

        public static Command write(QuestDefinitionModule quest)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(quest.write());
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
