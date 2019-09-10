using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.server
{
    class PendingHeal
    {
        public AbstractAttackable To { get; set; }
        
        public IGameEntity From { get; set; }
        
        public HealingTypes HealingType { get; set; }
        
        public int Amount { get; set; }
        
        public int AffectedDistance { get; set; }
        
        public CalculationTypes CalculationType { get; set; }

        /// <summary>
        /// Primary constructor
        /// Targeted heal
        /// </summary>
        /// <param name="to"></param>
        /// <param name="healType"></param>
        /// <param name="calculationType"></param>
        /// <param name="amount"></param>
        public PendingHeal(AbstractAttackable to, HealingTypes healType, CalculationTypes calculationType, int amount)
        {
            To = to;
            HealingType = healType;
            CalculationType = calculationType;
            Amount = amount;
        }

        /// <summary>
        /// Constructor
        /// Targeted heal coming from entity
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="healType"></param>
        /// <param name="calculationType"></param>
        /// <param name="amount"></param>
        public PendingHeal(AbstractAttackable to, IGameEntity from, HealingTypes healType, CalculationTypes calculationType,
            int amount) : this(to, healType, calculationType, amount)
        {
            From = from;
        }

        /// <summary>
        /// Primary constructor
        /// Area heal
        /// </summary>
        /// <param name="from">Coming from</param>
        /// <param name="affectedDistance">Distance from center</param>
        /// <param name="amount">Amount</param>
        /// <param name="calculationType">Calculation type</param>
        /// <param name="healType">Healing type</param>
        public PendingHeal(IGameEntity from, int affectedDistance, int amount, CalculationTypes calculationType, HealingTypes healType)
        {
            From = from;
            AffectedDistance = affectedDistance;
            Amount = amount;
            CalculationType = calculationType;
            HealingType = healType;
        }

        /// <summary>
        /// Checking if its area heal
        /// </summary>
        /// <returns>It is an area heal or not (t/f)</returns>
        public bool IsAreaHeal()
        {
            return HealingType == HealingTypes.HEALTH_AREA || HealingType == HealingTypes.SHIELD_AREA;
        }
    }
}