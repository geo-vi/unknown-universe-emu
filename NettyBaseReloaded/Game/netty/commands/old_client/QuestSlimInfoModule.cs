using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuestSlimInfoModule
    {
        public const short ID = 20919;

        public int questId;

        public int rootCaseId;

        public int minLevel;

        public List<QuestTypeModule> types;

        public QuestIconModule icon;

        public QuestAcceptabilityStatus acceptabilityStatus;

        public List<QuestRequirementModule> missingAcceptRequirements;
    }
}
