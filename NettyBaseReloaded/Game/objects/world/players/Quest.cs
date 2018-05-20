using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.players.quests;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Quest : PlayerBaseClass
    {
        public int Id { get; set; }

        public virtual QuestTypes QuestType { get; }

        public virtual QuestRoot Root { get; }

        public virtual QuestIcons Icon { get; }

        public virtual Reward Reward { get; }

        public Quest(Player player, int id) : base(player)
        {
            Id = id;
            Root.LoadPlayerData(player);
        }

        public virtual void Tick() { }

        public void AddKill(IAttackable attackable)
        {

        }

        public void AddCollection(Collectable collectable)
        {

        }

        public void AddResourceCollection(Ore ore)
        {

        }

        /// <summary>
        /// If Position monitor is needed just override Tick and add PositionMonitor - (saving resources).
        /// </summary>
        public void PositionMonitor()
        {

        }
    }
}
