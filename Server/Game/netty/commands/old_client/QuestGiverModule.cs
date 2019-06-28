using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class QuestGiverModule
    {
        public const short ID = 32052;

        public int questGiverId;

        public int mapObjectId;

        public QuestGiverModule(int questGiverId, int mapObjectId)
        {
            this.questGiverId = questGiverId;
            this.mapObjectId = mapObjectId;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(questGiverId);
            cmd.Integer(mapObjectId);
            return cmd.Message.ToArray();
        }
    }
}
