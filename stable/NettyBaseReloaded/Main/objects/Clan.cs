namespace NettyBaseReloaded.Main.objects
{
    class Clan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }

        // TODO: Add owned BattleStations

        public Clan(int id, string name, string tag)
        {
            Id = id;
            Name = name;
            Tag = tag;
        }
    }
}
