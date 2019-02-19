using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class EventBox : BonusBox
    {
        public EventBox(int id, string hash, Types type, Vector pos, Spacemap map, Vector[] limits, bool respawning = false) : base(id, hash, type, pos, map, limits, respawning)
        {
        }

        private Reward BoxReward;

        public void RandomiseReward()
        {
            var randomInstance = RandomInstance.getInstance(this);
            var num = randomInstance.NextDouble();
            if (num < 0.005)
            {
                BoxReward = new Reward(RewardType.URIDIUM, 32);
                BoxReward.Rewards.Add((short)0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_laser_ucb-100"));
                BoxReward.Rewards.Add(395);
            }
            else if (num < 0.02)
            {
                BoxReward = new Reward(RewardType.CREDITS, 50000);
                BoxReward.Rewards.Add((short)0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_laser_rsb-75"));
                BoxReward.Rewards.Add(100);
            }
            else if (num < 0.1)
            {

                BoxReward = new Reward(RewardType.CREDITS, 7500);
                BoxReward.Rewards.Add((short) 0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_rocketlauncher_sar-02"));
                BoxReward.Rewards.Add(6);
                BoxReward.Rewards.Add((short) 0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_rocketlauncher_hstrm-01"));
                BoxReward.Rewards.Add(4);

            }
            else if (num < 0.2)
            {
                BoxReward = new Reward(new Dictionary<RewardType, int> { { RewardType.GALAXY_GATES_ENERGY, 1 } });
                BoxReward.Rewards.Add((short)0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_laser_mcb-25"));
                BoxReward.Rewards.Add(580);
            }
            else if (num < 0.3)
            {
                BoxReward = new Reward(RewardType.AMMO, Item.Find("ammunition_laser_mcb-50"), 150);
            }
            else
            {
                BoxReward = new Reward(RewardType.AMMO, Item.Find("ammunition_laser_mcb-25"), 250);
            }
        }

        protected override void Reward(Player player)
        {
            RandomiseReward();
            BoxReward.ParseRewards(player);
        }
    }
}
