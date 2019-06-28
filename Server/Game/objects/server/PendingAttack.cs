using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.server
{
    class PendingAttack
    {
        public IAttackable From;

        public IAttackable To;
        
        public AttackTypes AttackType;

        public string LootId = "";

        public int Amount = 0;

        public PendingAttack(IAttackable from, IAttackable to, AttackTypes attackType, string lootId, int amount)
        {
            From = from;
            To = to;
            AttackType = attackType;
            LootId = lootId;
            Amount = amount;
        }
    }
}