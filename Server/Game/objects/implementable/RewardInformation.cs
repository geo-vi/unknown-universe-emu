using Server.Game.objects.entities.players;
using Server.Game.objects.enums;

namespace Server.Game.objects.implementable
{
    class RewardInformation
    {
        public RewardTypes Type { get; set; }

        public int Amount { get; set; }

        public RewardInformation(RewardTypes type, int amount)
        {
            
        }
    }
}