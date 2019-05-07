using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players.extra.boosters;

namespace NettyBaseReloaded.Game.objects.world.players.extra
{
    abstract class Booster : PlayerBaseClass
    {
        public enum Boosters
        {
            DMG_B01 = 0,
            DMG_B02 = 1,
            EP_B01 = 2,
            EP_B02 = 3,
            EP50 = 4,
            HON_B01 = 5,
            HON_B02 = 6,
            HON50 = 7,
            HP_B01 = 8,
            HP_B02 = 9,
            REP_B01 = 10,
            REP_B02 = 11,
            REP_S01 = 12,
            RES_B01 = 13,
            RES_B02 = 14,
            SHD_B01 = 15,
            SHD_B02 = 16,
            SREG_B01 = 17,
            SREG_B02 = 18,
            BB_01 = 19,
            QR_01 = 20,
            CD_B01 = 21,
            CD_B02 = 22,
            KAPPA_B01 = 23,
            HONM_1 = 24,
            XPM_1 = 25,
            DMGM_1 = 26
        }

        public enum Types
        {
            EP = 0,
            HONOUR = 1,
            DAMAGE = 2,
            SHIELD = 3,
            REPAIR = 4,
            SHIELDRECHARGE = 5,
            RESOURCE = 6,
            MAXHP = 7,
            ABILITY_COOLDOWN = 8,
            BONUSBOXES = 9,
            QUESTREWARD = 10
        }

        public int Id { get; set; }

        public Boosters BoosterType { get; set; }

        public Types Type { get; set; }

        public DateTime FinishTime { get; set; }

        protected Booster(int id, Player player, DateTime finishTime, Boosters boosterType, Types type) : base(player)
        {
            Id = id;
            FinishTime = finishTime;
            BoosterType = boosterType;
            Type = type;
        }

        public virtual void Tick()
        {
            if (FinishTime < DateTime.Now)
                Dispose();
        }

        public abstract double GetBoost();
        public abstract double GetSharedBoost();

        public static void CalculateTotalBoost(Player player)
        {
            try
            {
                if (player.EntityState == EntityStates.DEAD || !player.Controller.Active)
                    return;

                double addedDamage = 0;
                double addedShd = 0;
                double addedHp = 0;
                double addedQuestReward = 0;
                double addedBoxReward = 0;
                double addedRepairBoost = 0;
                double addedResourceBoost = 0;
                double addedEpBoost = 0;
                double addedHonBoost = 0;
                foreach (var booster in player.Boosters)
                {
                    switch (booster.Value.Type)
                    {
                        case Types.DAMAGE:
                            addedDamage += booster.Value.GetBoost();
                            break;
                        case Types.SHIELD:
                            addedShd += booster.Value.GetBoost();
                            break;
                        case Types.MAXHP:
                            addedHp += booster.Value.GetBoost();
                            break;
                        case Types.QUESTREWARD:
                            addedQuestReward += booster.Value.GetBoost();
                            break;
                        case Types.BONUSBOXES:
                            addedBoxReward += booster.Value.GetBoost();
                            break;
                        case Types.REPAIR:
                            addedRepairBoost += booster.Value.GetBoost();
                            break;
                        case Types.RESOURCE:
                            addedResourceBoost += booster.Value.GetBoost();
                            break;
                        case Types.EP:
                            addedEpBoost += booster.Value.GetBoost();
                            break;
                        case Types.HONOUR:
                            addedHonBoost += booster.Value.GetBoost();
                            break;
                    }
                }

                foreach (var booster in player.InheritedBoosters)
                {
                    switch (booster.Value.Type)
                    {
                        case Types.DAMAGE:
                            addedDamage += booster.Value.GetSharedBoost();
                            break;
                        case Types.SHIELD:
                            addedShd += booster.Value.GetSharedBoost();
                            break;
                        case Types.MAXHP:
                            addedHp += booster.Value.GetSharedBoost();
                            break;
                        case Types.QUESTREWARD:
                            break;
                        case Types.BONUSBOXES:
                            break;
                        case Types.REPAIR:
                            addedRepairBoost += booster.Value.GetSharedBoost();
                            break;
                        case Types.RESOURCE:
                            addedResourceBoost += booster.Value.GetSharedBoost();
                            break;
                        case Types.EP:
                            addedEpBoost += booster.Value.GetSharedBoost();
                            break;
                        case Types.HONOUR:
                            addedHonBoost += booster.Value.GetSharedBoost();
                            break;
                    }
                }

                if (player.BoostedDamage == addedDamage && player.BoostedShield == addedShd &&
                    player.BoostedHealth == addedHp
                    && player.BoostedQuestReward == addedQuestReward && player.BoostedBoxRewards == addedBoxReward &&
                    player.BoostedRepairSpeed == addedRepairBoost && player.BoostedResources == addedResourceBoost &&
                    player.BoostedExpReward == addedEpBoost && player.BoostedHonorReward == addedHonBoost)
                {
                    return;
                }

                player.BoostedDamage = 0;
                player.BoostDamage(addedDamage);

                player.BoostedHealth = 0;
                player.BoostHealth(addedHp);

                player.BoostedShield = 0;
                player.BoostShield(addedShd);

                player.BoostedQuestReward = 0;
                player.BoostQuestRewards(addedQuestReward);

                player.BoostedBoxRewards = 0;
                player.BoostBoxRewards(addedBoxReward);

                player.BoostedRepairSpeed = 0;
                player.BoostRepairSpeeds(addedRepairBoost);

                player.BoostedResources = 0;
                player.BoostResourceCollection(addedResourceBoost);

                player.BoostedExpReward = 0;
                player.BoostExpReward(addedEpBoost);

                player.BoostedHonorReward = 0;
                player.BoostHonReward(addedHonBoost);

                player.Updaters.Update();

                Packet.Builder.AttributeBoosterUpdateCommand(World.StorageManager.GetGameSession(player.Id));
            }
            catch
            {
                Debug.WriteLine("Booster error");
            }
        }

        public void AddTime(int timeInMs)
        {
            FinishTime = FinishTime.AddMilliseconds(timeInMs);
        }

        public void AddTime(TimeSpan time)
        {
            FinishTime = FinishTime + time;
        }

        public virtual void Dispose()
        {
            if (FinishTime < DateTime.Now)
            {
                Booster booster;
                Player.Boosters.TryRemove(Id, out booster);
            }
        }
    }
}
