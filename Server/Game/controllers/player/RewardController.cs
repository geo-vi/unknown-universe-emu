using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players;

namespace Server.Game.controllers.player
{
    class RewardController : PlayerSubController
    {
        private ConcurrentQueue<Reward> PendingRewards = new ConcurrentQueue<Reward>();

        private Reward CombinedReward;
        
        public RewardController(Player player) : base(player)
        {
        }
        
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
