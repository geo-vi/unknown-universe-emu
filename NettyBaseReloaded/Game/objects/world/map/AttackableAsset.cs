using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.objects;

namespace NettyBaseReloaded.Game.objects.world.map
{
    class AttackableAsset : IAttackable
    {
        public Asset BaseAsset;

        public override Vector Position
        {
            get => BaseAsset.Position;
            set => BaseAsset.Position = value;
        }
        public override Spacemap Spacemap
        {
            get => BaseAsset.Spacemap;
            set => BaseAsset.Spacemap = value;
        }

        public override Faction FactionId { get; set; }
        public override int CurrentHealth { get; set; }
        public override int MaxHealth { get; set; }
        public override int CurrentNanoHull { get; set; }
        public override int MaxNanoHull { get; }
        public override int CurrentShield { get; set; }
        public override int MaxShield { get; set; }
        public override double ShieldAbsorption { get; set; }
        public override double ShieldPenetration { get; set; }

        public AttackableAsset(int id, Asset baseAsset) : base(id)
        {
            BaseAsset = baseAsset;
        }

        public override void Tick()
        {
        }

        public override void Destroy()
        {
        }

        public override void Destroy(Character destroyer)
        {
        }
    }
}
