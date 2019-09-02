using System.Collections.Generic;
using Server.Game.objects.entities.players;

namespace Server.Game.objects.server
{
    class Level
    {
        public int Id { get; private set; }

        public double Experience { get; private set; }

        public List<Reward> Reward { get; private set; }
        
        public Level(int id, double experience, List<Reward> reward = null)
        {
            Id = id;
            Experience = experience;
            if (reward != null)
            {
                Reward = reward;
            }
            else Reward = new List<Reward>();
        }
    }
}