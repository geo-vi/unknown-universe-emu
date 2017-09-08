using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class Levels
    {
        public Dictionary<int, Level> PlayerLevels = new Dictionary<int, Level>();
        public Dictionary<int, Level> DroneLevels = new Dictionary<int, Level>();
        public Dictionary<int, Level> PetLevels = new Dictionary<int, Level>();
    }

    class Level
    {
        public int Id { get; set; }
        public long Experience { get; set; }

        public Level(int id, long exp)
        {
            Id = id;
            Experience = exp;
        }
    }
}
