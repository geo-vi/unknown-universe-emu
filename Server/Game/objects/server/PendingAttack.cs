using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.server
{
    class PendingAttack
    {
        public AbstractAttacker From { get; set; }

        public AbstractAttackable To { get; set; }
        
        public AttackTypes AttackType { get; set; }

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