using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBuilder.quests
{
    class QuestRoot
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public bool Ordered { get; set; }

        public bool Mandatory { get; set; }
        
        public int MandatoryCount { get; set; }

        public List<QuestElement> Elements { get; set; }

        public QuestElement FindElement(int conditionId)
        {
            var found = Elements.Find(x => x.Condition.Id == conditionId);
            if (found == null)
            {
                found = Elements.Find(x => x.Case.FindElement(conditionId) != null);
            }
            return found;
        }
    }
}
