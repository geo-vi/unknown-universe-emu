using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.server
{
    class PendingAttack
    {
        public AbstractAttacker From { get; }

        public AbstractAttackable To { get; }
        
        public AttackTypes AttackType { get; }

        public string LootId { get; set; }

        public int Amount { get; set; }

        public PendingAttack(AbstractAttacker from, AbstractAttackable to, AttackTypes attackType, string lootId, int amount)
        {
            From = from;
            To = to;
            AttackType = attackType;
            LootId = lootId;
            Amount = amount;
        }
    }
}