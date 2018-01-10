using System;
using System.Collections.Generic;
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

        public Boosters BoosterType { get; set; }

        public Types Type { get; set; }

        public DateTime FinishTime { get; set; }

        protected List<Booster> Stacked = new List<Booster>();

        protected Booster(Player player, DateTime finishTime, Boosters boosterType, Types type) : base(player)
        {
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
            if (player.EntityState == EntityStates.DEAD || !player.Controller.Active)
                return;

            double addedDamage = 0;
            foreach (var booster in player.Boosters.Where(x => x.Type == Types.DAMAGE))
            {
                addedDamage += booster.GetBoost();
            }
            foreach (var booster in player.InheritedBoosters.Where(x => x.Value.Type == Types.DAMAGE))
            {
                addedDamage += booster.Value.GetSharedBoost();
            }
            player.BoostedDamage = 0;
            player.BoostDamage(addedDamage);
            if (addedDamage > 0)
                Packet.Builder.AttributeBoosterUpdateCommand(World.StorageManager.GetGameSession(player.Id));
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
                //TODO: Remove booster from Player
            }
        }
    }
}
