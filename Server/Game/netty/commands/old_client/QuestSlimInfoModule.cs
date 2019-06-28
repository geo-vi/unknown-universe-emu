using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
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

        public QuestSlimInfoModule(int questId, int rootCaseId, int minLevel, List<QuestTypeModule> types, QuestIconModule icon, QuestAcceptabilityStatus acceptabilityStatus, List<QuestRequirementModule> missingAcceptRequirements)
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
            foreach (var questType in types)
                cmd.AddBytes(questType.write());
            cmd.AddBytes(icon.write());
            cmd.AddBytes(acceptabilityStatus.write());
            cmd.Integer(missingAcceptRequirements.Count);
            foreach (var missingRequirement in missingAcceptRequirements)
                cmd.AddBytes(missingRequirement.write());
            return cmd.Message.ToArray();
        }
    }
}
