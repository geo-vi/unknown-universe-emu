using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class PirateBooty : Collectable
    {
        public static int SPAWN_COUNT = 12;

        private Reward BoxReward;

        private bool Respawning { get; }

        private const int COLLECTION_TIME = 1; // default 5 

        public PirateBooty(int id, string hash, Types type, Vector pos, Spacemap map, Vector[] limits, bool respawning) : base(id, hash, type, pos, map, limits)
        {
            Respawning = respawning;
        }

        public void RandomiseReward()
        {
            var randomInstance = RandomInstance.getInstance(this);
            var num = randomInstance.NextDouble();
            if (num < 0.005)
            {
                BoxReward = new Reward(RewardType.ITEM, Item.Find("equipment_weapon_laser_lf-4"), 1);
            }
            //else if (num < 0.02) TODO: add design rewards 
            //{
            //    BoxReward = new Reward(RewardType.DESIGN, Item.Find("ship_goliath_design_solace"), 1);
            //}
            //else if (num < 0.03)
            //{
            //    BoxReward = new Reward(RewardType.DESIGN, Item.Find("ship_vengeance_design_lightning"), 1);
            //}
            else if (num < 0.06)
            {
                BoxReward = new Reward(RewardType.ITEM, Item.Find("equipment_weapon_laser_lf-3"), 1);
            }
            else if (num < 0.07)
            {
                BoxReward = new Reward(RewardType.ITEM, Item.Find("equipment_generator_shield_sg3n-b02"), 1);
            }
            else if (num < 0.17)
            {
                BoxReward = new Reward(RewardType.ITEM, Item.Find("equipment_generator_speed_g3n-7900"), 1);
            }
            else if (num < 0.2)
            {
                BoxReward = new Reward(new Dictionary<RewardType, int>{ {RewardType.GALAXY_GATES_ENERGY, 8}});
                BoxReward.Rewards.Add((short)0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_laser_rsb-75"));
                BoxReward.Rewards.Add(580);
            }
            else if (num < 0.3)
            {
                BoxReward = new Reward(RewardType.URIDIUM, 320);
                BoxReward.Rewards.Add((short)0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_laser_ucb-100"));
                BoxReward.Rewards.Add(395);
            }
            else
            {
                BoxReward = new Reward(RewardType.URIDIUM, 43);
                BoxReward.Rewards.Add((short)0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_rocketlauncher_sar-02"));
                BoxReward.Rewards.Add(75);
                BoxReward.Rewards.Add((short)0);
                BoxReward.Rewards.Add(RewardType.AMMO);
                BoxReward.Rewards.Add(Item.Find("ammunition_rocketlauncher_hstrm-01"));
                BoxReward.Rewards.Add(32);
            }
        }

        public override void Dispose()
        {
            Collector = null;
            base.Dispose();
            if (Respawning)
                Respawn();
        }

        public override void Collect(Character character)
        {
            if (Collector != null) return;
            if (character is Player player)
            {
                if (player.Information.BootyKeys[0] == 0)
                {
                    //locked
                    return;
                }

                Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|SLA|0|" + player.Id + "|" + COLLECTION_TIME);
                Collector = character;
            }

            Task.Factory.StartNew(CollectionThread);
        }

        private Character Collector;

        private async void CollectionThread()
        {
            var player = Collector as Player;
            if (player == null) return;
            var startCollection = DateTime.Now;
            while (startCollection.AddSeconds(COLLECTION_TIME) > DateTime.Now)
            {
                MovementController.ActualPosition(Collector);
                if (Collector == null || Collector.Moving || Collector.EntityState != EntityStates.ALIVE)
                {
                    var session = player.GetGameSession();
                    if (session != null)
                        Packet.Builder.LegacyModule(session, "0|A|SLC|0|" + player.Id);

                    Collector = null;
                    return;
                }

                await Task.Delay(500);
            }

            player.Information.BootyKeys[0] -= 1;
            World.DatabaseManager.UpdateBootyKeys(player);
            Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|BK|" + player.Information.BootyKeys[0]);
            RandomiseReward();
            BoxReward.ParseRewards(player);
            Dispose();
        }

        protected override void Reward(Player player)
        {
        }
    }
}
