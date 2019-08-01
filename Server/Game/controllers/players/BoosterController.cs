using System.Collections.Concurrent;
using Server.Game.objects.entities.players;

namespace Server.Game.controllers.players
{
    class BoosterController : PlayerSubController
    {
        public readonly ConcurrentDictionary<int, Booster> ActiveBoosters = new ConcurrentDictionary<int, Booster>();

        public void RefreshBoost()
        {
            CalculateBoost();
        }

        private void CalculateBoost()
        {
            
        }
        
        private void ShareSelfBoost()
        {
            
        }

        public void DestroyBooster(int boosterId)
        {
            
        }
    }
}