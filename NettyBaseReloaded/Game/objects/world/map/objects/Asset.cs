namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class Asset : Object
    {
        public short Type { get; set; }
        public string Name { get; set; }
        public int FactionId { get; set; }
        public string ClanTag { get; set; }
        public int AssetId { get; set; }
        public int DesignId { get; set; }
        public int ExpansionStage { get; set; }
        public int ClanId { get; set; }
        public bool Invisible { get; set; }
        public bool VisibleOnWarnRadar { get; set; }
        public bool DetectedByWarnRadar { get; set; }
        //public ClanRelationModule ClanRelation { get; set; }
        //public List<VisualModifierCommand> Modifiers { get; set; }

        public Asset(int id, string name, short type, int factionId, string clanTag, int assetId, int designId, int expansionStage, Vector position, int clanId, bool invisible,
            bool visibleOnWarnRadar, bool detectedByWarnRadar) : base(id, position)
        {
            Type = type;
            Name = name;
            FactionId = factionId;
            ClanTag = clanTag;
            AssetId = assetId;
            DesignId = designId;
            ExpansionStage = expansionStage;
            ClanId = clanId;
            Invisible = invisible;
            VisibleOnWarnRadar = visibleOnWarnRadar;
            DetectedByWarnRadar = detectedByWarnRadar;
        }

        public override void execute(Character character)
        {
            
        }
    }
}
