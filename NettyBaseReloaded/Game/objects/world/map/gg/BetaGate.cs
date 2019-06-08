using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment;

namespace NettyBaseReloaded.Game.objects.world.map.gg
{
    class BetaGate : BasicGalaxyGate
    {
        public override Dictionary<int, Wave> Waves => new Dictionary<int, Wave>
        {
            {
                0,
                new Wave(0, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[84], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                1,
                new Wave(1, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[84], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                2,
                new Wave(2, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[84],  2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                3,
                new Wave(3, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[84],  2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                4,
                new Wave(4, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[71],  2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                5,
                new Wave(5, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[71], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                6,
                new Wave(6, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[71], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                7,
                new Wave(7, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[71], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                8,
                new Wave(8, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[73], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                9,
                new Wave(9, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[73], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                10,
                new Wave(10, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[73], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                11,
                new Wave(11, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[73], 2, "b"), 10),
                    new Vector(1000, 6400))
            },
            {
                12,
                new Wave(12, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[75], 2, "b"), 20),
                    new Vector(1000, 7800))
            },
            {
                13,
                new Wave(13, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[75], 2, "b"), 20),
                    new Vector(1000, 7800))
            },
            {
                14,
                new Wave(14, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[75], 2, "b"), 20),
                    new Vector(1000, 7800))
            },
            {
                15,
                new Wave(15, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[75], 2, "b"), 20),
                    new Vector(1000, 7800))
            },
            {
                16,
                new Wave(16, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[72], 2, "b"), 5),
                    new Vector(1000, 7800))
            },
            {
                17,
                new Wave(16, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[72],  2, "b"), 5),
                    new Vector(1000, 7800))
            },
            {
                18,
                new Wave(16, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[72],  2, "b"), 5),
                    new Vector(1000, 7800))
            },
            {
                19,
                new Wave(16, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[72],  2, "b"), 5),
                    new Vector(1000, 7800))
            },
            {
                20,
                new Wave(20, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[78],  2, "b"), 20),
                    new Vector(1000, 8600))
            },
            {
                21,
                new Wave(21, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[78],  2, "b"), 20),
                    new Vector(1000, 8600))
            },
            {
                22,
                new Wave(22, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[78],  2, "b"), 20),
                    new Vector(1000, 8600))
            },
            {
                23,
                new Wave(23, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[78],  2, "b"), 20),
                    new Vector(1000, 8600))
            },
            {
                24,
                new Wave(24, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[74],  2, "b"), 5),
                    new Vector(1000, 10800))
            },
            {
                25,
                new Wave(26, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[74],  2, "b"), 5),
                    new Vector(1000, 10800))
            },
            {
                26,
                new Wave(26, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[74],  2, "b"), 5),
                    new Vector(1000, 10800))
            },
            {
                27,
                new Wave(27, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[74],  2, "b"), 5),
                    new Vector(1000, 10800))
            },
            {
                28,
                new Wave(28, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[76],  2, "b"), 20),
                    new Vector(1000, 10800))
            },
            {
                29,
                new Wave(29, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[76], 2, "b"), 20),
                    new Vector(1000, 10800))
            },
            {
                30,
                new Wave(30, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[76], 2, "b"), 20),
                    new Vector(1000, 10800))
            },
            {
                31,
                new Wave(31, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[76], 2, "b"), 20),
                    new Vector(1000, 10800))
            },
            {
                32,
                new Wave(32, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[79], 2, "b"), 4),
                    new Vector(1000, 10800))
            },
            {
                33,
                new Wave(33, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[79], 2, "b"), 4),
                    new Vector(1000, 10800))
            },
            {
                34,
                new Wave(34, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[79], 2, "b"), 4),
                    new Vector(1000, 10800))
            },
            {
                35,
                new Wave(35, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[79], 2, "b"), 4),
                    new Vector(1000, 10800))
            },
            {
                36,
                new Wave(36, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[81], 2, "b"), 5),
                    new Vector(1000, 10800))
            },
            {
                37,
                new Wave(37, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[81], 2,"b"), 10),
                    new Vector(1000, 10800))
            },
            {
                38,
                new Wave(38, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[81], 2,"b"), 5),
                    new Vector(1000, 10800))
            },
            {
                39,
                new Wave(39, map.Wave.CreateWave(new Wave.Npc(World.StorageManager.Ships[81], 2,"b"), 10),
                    new Vector(1000, 10800))
            }
        };

        public BetaGate(Spacemap coreMap, int wave) : base(2, coreMap, wave)
        {
        }

        public override void Reward()
        {
            base.Reward();
            var currencyReward = new Reward(new Dictionary<RewardType, int> { { RewardType.CREDITS, 40000000 }, { RewardType.URIDIUM, 40000 }, { RewardType.EXPERIENCE, 8000000 }, { RewardType.HONOR, 200000 } });
            var ammoReward = new Reward(RewardType.AMMO, Item.Find("ammunition_laser_ucb-100"), 42500);
            MoveOut(Owner);
            ammoReward.ParseRewards(Owner);
            currencyReward.ParseRewards(Owner);

            if (Owner.OwnedGates.ContainsKey(Id))
            {
                Owner.OwnedGates.TryRemove(Id, out _);
            }
            Owner.Gates.BetaComplete++;
            Owner.Gates.BetaReady = false;
            Owner.Gates.Save();
            JoinedPlayers.Clear();
            Owner.RefreshPlayersView();
        }
    }
}
