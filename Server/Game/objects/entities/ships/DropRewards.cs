using System;
using Newtonsoft.Json;
using Server.Game.objects.entities.players;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.ships
{
    /// <summary>
    /// Implementing the OreCollection class because its the 
    /// </summary>
    class DropRewards
    {
        public Reward Reward { get; set; }

        public OreCollection OreCollection { get; set; }

        public DropRewards()
        {
        }

        public DropRewards Multiply(int i)
        {
            throw new System.NotImplementedException();
        }
    }
}
