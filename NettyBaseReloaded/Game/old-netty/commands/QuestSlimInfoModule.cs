using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestSlimInfoModule
    {
        public const short ID = 20919;

        public int questId;
        public int rootCaseId;
        public int minLevel;
        public List<QuestTypeModule> types;
        public QuestIconModule icon;
        public QuestAcceptabilityStatusModule acceptabilityStatus;
        public List<QuestRequirementModule> missingAcceptRequirements;

        public QuestSlimInfoModule(int questId, int rootCaseId, int minLevel, List<QuestTypeModule> types, QuestIconModule  icon, 
            QuestAcceptabilityStatusModule acceptabilityStatus, List<QuestRequirementModule> missingAcceptRequirements)
        {
            this.questId = questId;
            this.rootCaseId = rootCaseId;
            this.minLevel = minLevel;
            this.types = types;
            this.icon = icon;
            this.acceptabilityStatus = acceptabilityStatus;
            this.missingAcceptRequirements = missingAcceptRequirements;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(questId);
            cmd.Integer(rootCaseId);
            cmd.Integer(minLevel);
            cmd.Integer(types.Count);
            foreach (var loc0 in types)
            {
                cmd.AddBytes(loc0.write());
            }
            cmd.AddBytes(icon.write());
            cmd.AddBytes(acceptabilityStatus.write());
            cmd.Integer(missingAcceptRequirements.Count);
            foreach (var loc1 in missingAcceptRequirements)
            {
                cmd.AddBytes(loc1.write());
            }
            return cmd.Message.ToArray();
        }
    }
}
