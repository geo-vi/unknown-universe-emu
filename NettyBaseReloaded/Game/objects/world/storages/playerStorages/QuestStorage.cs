using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.storages.playerStorages
{
    class QuestStorage
    {
        public int Id { get; set; }

        public int KilledNpcs { get; set; }

        public int KilledPlayers { get; set; }

        public int CollectedOres { get; set; }

        public int CollectedBoxes { get; set; }

        public bool TraveledToLocation { get; set; }

        public bool TraveledToQuestGiver { get; set; }
    }
}
