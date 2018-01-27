using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.quests;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Quest : PlayerBaseClass
    {
        public int Id { get; set; }

        public QuestTypes QuestType { get; set; }

        public QuestRoot Root { get; set; }

        public QuestIcons Icon { get; set; }

        public Reward Reward { get; set; }

        public Quest(Player player, int id) : base(player)
        {
            var questBase = LoadQuest(id);
            QuestType = questBase.QuestType;
            Icon = questBase.Icon;
            Root = questBase.Root;
            Reward = questBase.Reward;
            Root.LoadPlayerData(player);
        }

        public Quest() : base(null) { }

        public static Quest LoadQuest(int id)
        {
            return null;
        }
    }
}
