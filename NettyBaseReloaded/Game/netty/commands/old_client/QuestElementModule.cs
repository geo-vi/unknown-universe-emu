using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuestElementModule
    {
        public const short ID = 29674;

        public QuestCaseModule questCase;
        public QuestConditionModule condition;

        public QuestElementModule(QuestCaseModule questCase, QuestConditionModule condition)
        {
            this.questCase = questCase;
            this.condition = condition;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(questCase.write());
            cmd.AddBytes(condition.write());
            return cmd.Message.ToArray();
        }
    }
}
