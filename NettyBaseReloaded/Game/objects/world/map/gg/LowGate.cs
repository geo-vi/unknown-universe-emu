using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players.equipment;

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

        public int RelaysDown = 0;
        public LowGate(int wave, Spacemap baseMap) : base(baseMap, wave)
        {
            AlmostNoNpcsLeft += LowGate_AlmostNoNpcsLeft;
        }

        public override void Tick()
        {
            base.Tick();
            if (RelaysDown == 4 && (!Active || VirtualMap.Entities.Count(x => x.Value is Npc) == 0) && Waves.ContainsKey(Wave))
            {
                Start();
            }
        }

        private void DestroyedRelay(object sender, Asset asset)
        {
            Start();
            RelaysDown++;
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
            base.Initiate();
        }

        public override void Start()
        {
            WavesLeftTillEnd = 3;
            Active = true;
            SendWave();
        }

        public override void SendWave()
        {
            try
            {
                if (!Waves.ContainsKey(Wave) || WavesLeftTillEnd <= 0 && RelaysDown != 4)
                {
                    return;
                }
                Task.Factory.StartNew(() => Waves[Wave].Create(VirtualMap, VWID));
                Wave++;
                WavesLeftTillEnd--;
                foreach (var joined in JoinedPlayers.Values)
                {
                    if (joined.GetGameSession() == null) continue;
                    Packet.Builder.LegacyModule(joined.GetGameSession(), "0|A|STD|Wave " + Wave);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                new ExceptionLog("low", "", e);
            }
        }

        public override void End()
        {
            Active = false;
            WaitingPhaseEnd = DateTime.MaxValue;
        }

        public override void Reward()
        {
            AlmostNoNpcsLeft -= LowGate_AlmostNoNpcsLeft;
            Finished = true;
            var random = RandomInstance.getInstance(Owner);
            var hit = random.Next(0, 100);
            foreach (var joined in JoinedPlayers.Values)
            {
                var currencyReward = new Reward(new Dictionary<RewardType, int> { { RewardType.CREDITS, 2500000 }, { RewardType.URIDIUM, 25000 } });
                var ammoReward = new Reward(RewardType.AMMO, new Item(-1, "ammunition_laser_ucb-100", 25000), 25000);
                var specialReward = new characters.Reward(RewardType.ITEM, new Item(-1, "equipment_weapon_laser_lf-4", 1), 1);
                if (joined?.GetGameSession() == null) continue;
                var joinedSession = joined.GetGameSession();
                if (hit == 15)
                {
                    Packet.Builder.LegacyModule(joinedSession, "0|A|STD|You've won a LF4!");
                    specialReward.ParseRewards(joined);
                }
                //TODO
                MoveOut(joined);
                ammoReward.ParseRewards(joined);
                currencyReward.ParseRewards(joined);
            }
            JoinedPlayers.Clear();
        }
    }
}
