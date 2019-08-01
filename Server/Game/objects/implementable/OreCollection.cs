namespace Server.Game.objects.implementable
{
    class OreCollection
    {
        public int Prometium { get; set; }
        public int Endurium { get; set; }
        public int Terbium { get; set; }
        public int Prometid { get; set; }
        public int Duranium { get; set; }
        public int Xenomit { get; set; }
        public int Promerium { get; set; }
        public int Seprom { get; set; }
        public int Palladium { get; set; }

        public OreCollection(int prometium, int endurium, int terbium, int prometid, int duranium, int xenomit,
            int promerium, int seprom,
            int palladium)
        {
            Prometium = prometium;
            Endurium = endurium;
            Terbium = terbium;
            Prometid = prometid;
            Duranium = duranium;
            Xenomit = xenomit;
            Promerium = promerium;
            Seprom = seprom;
            Palladium = palladium;
        }

        public OreCollection()
        {
            
        }
    }
}