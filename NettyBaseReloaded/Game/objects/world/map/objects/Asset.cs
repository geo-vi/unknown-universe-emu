using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class Asset : Object
    {
        /// <summary>
        /// Asset type
        /// </summary>
        public AssetTypes Type { get; set; }

        public string Name { get; set; }

        public Faction Faction { get; set; }

        public int DesignId { get; set; }

        public Clan Clan { get; set; }

        public bool Invisible { get; set; }

        /// <summary>
        /// Lasers (so on)
        /// </summary>
        public int ExpansionStage { get; set; }

        /// <summary>
        /// Warning radar (minimap)
        /// </summary>
        public bool VisibleOnWarnRadar { get; set; }
        public bool DetectedByWarnRadar { get; set; }

        public Asset(int id, string name, AssetTypes type, Faction faction, Clan clan, int designId, int expansionStage, Vector position, Spacemap map, bool invisible,
            bool visibleOnWarnRadar, bool detectedByWarnRadar) : base(id, position, map)
        {
            Type = type;
            Name = name;
            Faction = faction;
            Clan = clan;
            DesignId = designId;
            ExpansionStage = expansionStage;
            Invisible = invisible;
            VisibleOnWarnRadar = visibleOnWarnRadar;
            DetectedByWarnRadar = detectedByWarnRadar;
        }

        public override void execute(Character character)
        {
            
        }

        public virtual void OnDestroyed()
        {
            
        }
    }
}
