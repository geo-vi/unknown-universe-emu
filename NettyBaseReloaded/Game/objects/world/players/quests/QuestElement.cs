using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.players.quests
{
    class QuestElement
    {
        public QuestRoot Case { get; set; }
        public QuestCondition Condition { get; set; }

        public static List<netty.commands.old_client.QuestElementModule> ParseElementsOld(List<QuestElement> elements)
        {
            var questElements = new List<netty.commands.old_client.QuestElementModule>();
            foreach (var questElement in elements)
            {
                questElements.Add(new netty.commands.old_client.QuestElementModule(new netty.commands.old_client.QuestCaseModule(questElement.Case.Id, questElement.Case.Active, questElement.Case.Mandatory, questElement.Case.Ordered, questElement.Case.MandatoryCount, ParseElementsOld(questElement.Case.Elements)), null));
            }
            return questElements;
        }

        //public static List<netty.commands.old_client.LootModule> ParseRewardsOld(Reward rewards)
        //{
        //    foreach (var reward in rewards.Rewards)
        //    {
                
        //    }
        //}

        //TODO: Add new
    }
}
