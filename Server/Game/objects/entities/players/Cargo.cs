using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players
{
    class Cargo : PlayerImplementedClass
    {
        public OreCollection Ores { get; set; }

        public int MaxCargoSpace
        {
            get
            {
                var shipCargo = Player.Hangar.Ship.Cargo;
                if (Player.Information.Premium)
                {
                    shipCargo *= 2;
                }
                return shipCargo;
            }
        }

        public int CargoSpaceLeft
        {
            get
            {
                var usedSpace = Ores.Prometium + Ores.Endurium + Ores.Terbium 
                                + Ores.Duranium + Ores.Prometid + Ores.Promerium 
                                               + Ores.Seprom + Ores.Palladium;
                return MaxCargoSpace - usedSpace;
            }
        }


        public Cargo(Player player) : base(player)
        {
            Ores = new OreCollection();
        }
    }
}