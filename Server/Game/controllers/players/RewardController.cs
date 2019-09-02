using System.Collections.Concurrent;
using Server.Game.objects.entities.players;

namespace Server.Game.controllers.players
{
    class RewardController : PlayerSubController
    {
        private ConcurrentQueue<Reward> _pendingRewards = new ConcurrentQueue<Reward>();

        private Reward CombinedReward { get; set; }
        
        /// <summary>
        /// Adds new reward to the queue
        /// </summary>
        /// <param name="reward"></param>
        public void AddReward(Reward reward)
        {
            
        }
        
        /// <summary>
        /// Will combine all the rewards and make one big one
        /// </summary>
        public void StartRewarding()
        {
            
        }

        /// <summary>
        /// Removes a reward from the queue
        /// </summary>
        public void RemovePendingReward()
        {
            
        }
    }
}
