using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.characters;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.players.quests
{
    class QuestElement
    {
        public QuestRoot Case { get; set; }
        public QuestCondition Condition { get; set; }

        public static List<netty.commands.old_client.QuestElementModule> ParseElementsOld(Player player, List<QuestElement> elements)
        {
            try
            {
                var questElements = new List<netty.commands.old_client.QuestElementModule>();
                foreach (var questElement in elements)
                {
                    var state = player.QuestData.GetData(questElement.Condition.Id);
                    questElements.Add(new netty.commands.old_client.QuestElementModule(new netty.commands.old_client.QuestCaseModule(questElement.Case.Id, questElement.Case.Active, questElement.Case.Mandatory, questElement.Case.Ordered, questElement.Case.MandatoryCount, ParseElementsOld(player, questElement.Case.Elements)),
                        new QuestConditionModule(questElement.Condition.Id, questElement.Condition.Matches, (short)questElement.Condition.Type, (short)questElement.Condition.Type, questElement.Condition.TargetValue, questElement.Condition.Mandatory, new QuestConditionStateModule(state.CurrentValue, state.Active, state.Completed),
                            new List<QuestConditionModule>())));
                }
                return questElements;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        public netty.commands.old_client.QuestConditionModule ParseSubConditionsOld()
        {
            throw new NotImplementedException();
        }
        
        //TODO: Add new
    }
}
