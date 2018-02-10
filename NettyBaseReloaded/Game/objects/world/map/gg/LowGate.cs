using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.gg
{
    class LowGate : GalaxyGate
    {
        public override Dictionary<int, Wave> Waves => new Dictionary<int, Wave>
        {
            {0, new Wave(0, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[94], 1, ""), 12), new Vector(1000, 6400))},
            {1, new Wave(1, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[94], 1, ""), 10), new Vector(1000, 7800))},
            {2, new Wave(2, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[94], 1, ""), 8), new Vector(1000, 8600))},
            {3, new Wave(3, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[94], 1, ""), 4), new Vector(1000, 10800))}
        };

        public LowGate(int wave, Spacemap baseMap) : base(baseMap, wave)
        {
            AlmostNoNpcsLeft += LowGate_AlmostNoNpcsLeft;
        }

        private void LowGate_AlmostNoNpcsLeft(object sender, EventArgs e)
        {
            SendWave();
        }

        public override void Start()
        {
            WavesLeftTillEnd = 3;
            Active = true;
            SendWave();
        }

        public override void SendWave()
        {
            if (!Waves.ContainsKey(Wave) || WavesLeftTillEnd <= 0)
            {
                return;
            }
            Waves[Wave].Create(VirtualMap);
            Wave++;
            WavesLeftTillEnd--;
        }

        public override void End()
        {
            Active = false;
            Console.WriteLine("END");
            WaitingPhaseEnd = DateTime.MaxValue;
            Console.WriteLine("Initiated wait phase");
            WavesLeftTillEnd = 3;
        }

        public override void Reward()
        {
            //TODO: Add rewards
            MoveOut();
        }
    }
}
