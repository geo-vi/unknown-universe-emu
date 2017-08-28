using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class GalaxyGate
    {
        public int Id { get; set; }

        public List<Wave> Waves { get; set; }

        public Reward Reward { get; set; }

        public Jumpgate Jumpgate { get; set; }

        public int LivesLeft { get; set; }

        public int CurrentWave { get; set; }

        public GalaxyGate(int id, List<Wave> waves, Reward reward, Jumpgate jumpgate, int livesLeft = 0, int currentWave = 0)
        {
            Id = id;
            Waves = waves;
            Reward = reward;
            Jumpgate = jumpgate;
            LivesLeft = livesLeft;
            CurrentWave = currentWave;
        }
    }
}
