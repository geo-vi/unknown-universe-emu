using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;

namespace NettyBaseReloaded.Game.objects.world.map.gg
{
    class LowGate : GalaxyGate
    {
        public override Dictionary<int, Wave> Waves => new Dictionary<int, Wave>
        {
            {
                0,
                new Wave(0, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[94], 1, ""), 14),
                    new Vector(1000, 6400))
            },
            {
                1,
                new Wave(1, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[94], 1, ""), 12),
                    new Vector(1000, 6400))
            },
            {
                2,
                new Wave(2, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[94], 1, ""), 8),
                    new Vector(1000, 6400))
            },
            {
                3,
                new Wave(3, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[93], 1, ""), 12),
                    new Vector(1000, 7800))
            },
            {
                4,
                new Wave(4, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[93], 1, ""), 8),
                    new Vector(1000, 7800))
            },
            {
                5,
                new Wave(5, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[92], 1, ""), 4),
                    new Vector(1000, 8600))
            },
            {
                6,
                new Wave(6, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[91], 1, ""), 14),
                    new Vector(1000, 10800))
            },
            {
                7,
                new Wave(7, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[97], 1, ""), 10),
                    new Vector(1000, 10800))
            },
            {
                8,
                new Wave(8, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[95], 1, ""), 8),
                    new Vector(1000, 10800))
            },
            {
                9,
                new Wave(9, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[96], 1, ""), 8),
                    new Vector(1000, 10800))
            },
            {
                10,
                new Wave(10, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[96], 1, ""), 8),
                    new Vector(1000, 10800))
            },
            {
                11,
                new Wave(11, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[90], 1, ""), 1),
                    new Vector(1000, 10800))
            }
        };


        public LowGate(int wave, Spacemap baseMap) : base(baseMap, wave)
        {
            AlmostNoNpcsLeft += LowGate_AlmostNoNpcsLeft;
        }

        private int DestroyedRelays = 0;
        private void DestroyedRelay(object sender, Asset asset)
        {
            Start();
            DestroyedRelays++;
            if (DestroyedRelays == 4)
                WavesLeftTillEnd = 10;
        }

        private void LowGate_AlmostNoNpcsLeft(object sender, EventArgs e)
        {
            SendWave();
        }

        public override void Initiate()
        {
            foreach (var relayObject in VirtualMap.Objects.Where(x => x.Value is RelayStation))
            {
                var relay = relayObject.Value as RelayStation;
                relay.Core.Destroyed += DestroyedRelay;
            }
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
        }

        public override void Reward()
        {
            //TODO: Add rewards
            MoveOut();
        }
    }
}
