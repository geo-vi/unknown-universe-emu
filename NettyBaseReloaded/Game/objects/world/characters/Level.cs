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

        public Level DeterminatePlayerLvl(double get)
        {
            var levelsForCurrentExp = PlayerLevels.FirstOrDefault(x => x.Value.Experience >= get).Value;
            return levelsForCurrentExp;
        }

        public Level DetermineDroneLvl(double get)
        {
            var levelsForCurrentExp = DroneLevels.FirstOrDefault(x => x.Value.Experience >= get).Value;
            return levelsForCurrentExp;
        }

        public Level DeterminePetLvl(double get)
        {
            var levelsForCurrentExp = PetLevels.FirstOrDefault(x => x.Value.Experience >= get).Value;
            return levelsForCurrentExp;
        }
    }

    class Level
    {
        public int Id { get; set; }
        public double Experience { get; set; }

        public Level(int id, double exp)
        {
            Id = id;
            Experience = exp;
        }
    }
}
