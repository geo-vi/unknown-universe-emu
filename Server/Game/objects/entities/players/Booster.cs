using Server.Game.objects.enums;

namespace Server.Game.objects.entities.players
{
    abstract class Booster
    {
        public int Id { get; }

        public BoostTypes BoostType;
        
        private BoosterTypes Type;
        
        protected int DamageBoost;

        protected int ShieldBoost;

        protected int HealthBoost;

        protected int BoxRewardBoost;

        protected Booster(int id, BoostTypes typeOfBoost, BoosterTypes boosterType, int damageBoost, int shieldBoost, int healthBoost,
            int boxRewardBoost)
        {
            Id = id;
            BoostType = typeOfBoost;
            Type = boosterType;
            DamageBoost = damageBoost;
            ShieldBoost = shieldBoost;
            HealthBoost = healthBoost;
            BoxRewardBoost = boxRewardBoost;
        }

        public float GetDamageMultiplier()
        {
            return 0;
        }

        public float GetShieldMultiplier()
        {
            return 0;
        }

        public float GetHealthMultiplier()
        {
            return 0;
        }

        public float GetBoxRewardMultiplier()
        {
            return 0;
        }
    }
}