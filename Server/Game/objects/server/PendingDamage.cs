using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.server
{
    class PendingDamage
    {
        public AbstractAttackable Target { get; set; }
        
        public AbstractAttacker Attacker { get; set; }
        
        public int Damage { get; set; }
        
        public int AbsorbDamage { get; set; }
        
        public DamageCalculationTypes CalculationType { get; set; }
        
        public AttackTypes AttackType { get; set; }
        
        public DamageTypes DamageType { get; set; }
        
        public int AffectedDistance { get; set; }
        
        /// <summary>
        /// Secondary constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="attacker"></param>
        /// <param name="damage"></param>
        /// <param name="absorbDamage"></param>
        /// <param name="calculationType"></param>
        /// <param name="attackType"></param>
        public PendingDamage(AbstractAttackable target, AbstractAttacker attacker, int damage, int absorbDamage,
            DamageCalculationTypes calculationType, AttackTypes attackType) : 
            this(target, damage, absorbDamage, calculationType, attackType)
        {
            Attacker = attacker;
        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="absorbDamage"></param>
        /// <param name="calculationType"></param>
        /// <param name="attackType"></param>
        public PendingDamage(AbstractAttackable target, int damage, int absorbDamage, DamageCalculationTypes calculationType, 
            AttackTypes attackType)
        {
            Target = target;
            Damage = damage;
            AbsorbDamage = absorbDamage;
            CalculationType = calculationType;
            AttackType = attackType;
            DamageType = DamageTypes.ENTITY;
        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="affectedDistance"></param>
        /// <param name="damage"></param>
        /// <param name="absorbDamage"></param>
        /// <param name="calculationType"></param>
        /// <param name="attackType"></param>
        public PendingDamage(int affectedDistance, int damage, int absorbDamage, DamageCalculationTypes calculationType,
            AttackTypes attackType)
        {
            AffectedDistance = affectedDistance;
            Damage = damage;
            AbsorbDamage = absorbDamage;
            CalculationType = calculationType;
            AttackType = attackType;
            DamageType = DamageTypes.AREA;
        }

        /// <summary>
        /// Secondary constructor
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="affectedDistance"></param>
        /// <param name="damage"></param>
        /// <param name="absorbDamage"></param>
        /// <param name="calculationType"></param>
        /// <param name="attackType"></param>
        public PendingDamage(AbstractAttacker attacker, int affectedDistance, int damage, int absorbDamage,
            DamageCalculationTypes calculationType, AttackTypes attackType) : this(affectedDistance, damage, absorbDamage, calculationType, attackType)
        {
            Attacker = attacker;
        }
    }
}