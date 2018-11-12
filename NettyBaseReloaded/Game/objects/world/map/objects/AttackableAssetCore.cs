using System;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    sealed class AttackableAssetCore : IAttackable
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

        public AttackableAssetCore(int id, Asset baseAsset, int hp, int maxHp, int shield, int maxShield, int nano, int maxNano, double abs, double pen) : base(id)
        {
            BaseAsset = baseAsset;
            CurrentHealth = hp;
            MaxHealth = maxHp;
            CurrentShield = shield;
            MaxShield = maxShield;
            CurrentNanoHull = nano;
            MaxNanoHull = maxNano;
            ShieldAbsorption = abs;
            ShieldPenetration = pen;
        }

        public override void Tick()
        {
            Update();
            TickVisuals();
        }

        public void Update()
        {
            if (CurrentHealth < 0) CurrentHealth = 0;
            if (CurrentShield < 0) CurrentShield = 0;
        }

        public event EventHandler<Asset> Destroyed;
        public override void Destroy()
        {
            EntityState = EntityStates.DEAD;
            Destroyed?.Invoke(this, BaseAsset);
        }

        public override void Destroy(Character destroyer)
        {
            Destroy();
        }
    }
}
