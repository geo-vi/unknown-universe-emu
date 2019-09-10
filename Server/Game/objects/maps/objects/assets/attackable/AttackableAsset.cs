using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps.objects.assets.attackable
{
    class AttackableAsset : AbstractAttackable
    {
        public AttackableAsset(int id) : base(id)
        {
        }

        public override Vector Position { get; set; }
        public override Spacemap Spacemap { get; set; }
        public override Factions FactionId { get; set; }
        public override int CurrentHealth { get; set; }
        public override int MaxHealth { get; set; }
        public override int CurrentNanoHull { get; set; }
        public override int MaxNanoHull { get; }
        public override int CurrentShield { get; set; }
        public override int MaxShield { get; set; }
        public override double ShieldAbsorption { get; set; }
        public override double ShieldPenetration { get; set; }
    }
}
