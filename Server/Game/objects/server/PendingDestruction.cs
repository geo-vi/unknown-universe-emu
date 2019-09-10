using System;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.server
{
    class PendingDestruction
    {
        public AbstractAttacker Attacker { get; set; }
        
        public AbstractAttackable Target { get; set; }
        
        public DestructionTypes DestructionType { get; set; }
        
        public ExplosionTypes ExplosionType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="attacker"></param>
        /// <param name="destructionType"></param>
        /// <param name="explosionType"></param>
        public PendingDestruction(AbstractAttackable target, AbstractAttacker attacker,
            DestructionTypes destructionType, ExplosionTypes explosionType) : this(target, destructionType,
            explosionType)
        {
            Attacker = attacker;
        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="destructionType"></param>
        /// <param name="explosionType"></param>
        public PendingDestruction(AbstractAttackable target, DestructionTypes destructionType, ExplosionTypes explosionType)
        {
            Target = target;
            DestructionType = destructionType;
            ExplosionType = explosionType;
        }
    }
}