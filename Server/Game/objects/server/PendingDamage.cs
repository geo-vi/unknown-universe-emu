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
        
        public PendingDamage(AbstractAttackable target, AbstractAttacker attacker, int damage, int absorbDamage,
            DamageCalculationTypes calculationType, AttackTypes attackType)
        {
            Target = target;
            Attacker = attacker;
            Damage = damage;
            AbsorbDamage = absorbDamage;
            CalculationType = calculationType;
            AttackType = attackType;
        }

        public PendingDamage(AbstractAttackable target, int damage, int absorbDamage, DamageCalculationTypes calculationType, 
            AttackTypes attackType)
        {
            Target = target;
            Damage = damage;
            AbsorbDamage = absorbDamage;
            CalculationType = calculationType;
            AttackType = attackType;
        }
    }
}