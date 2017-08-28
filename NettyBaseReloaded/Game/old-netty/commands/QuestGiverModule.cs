using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestGiverModule
    {
        public const short ID = 32052;

        public int questGiverId = 0;

        public int mapObjectId = 0;

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
