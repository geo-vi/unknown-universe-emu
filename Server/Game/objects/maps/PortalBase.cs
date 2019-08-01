namespace Server.Game.objects.maps
{
    class PortalBase
    {
        public int Id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int newX { get; set; }
        public int newY { get; set; }
        public int Map { get; set; }

        public PortalBase(int Id, int x, int y, int newX, int newY, int Map)
        {
            this.Id = Id;
            this.x = x;
            this.y = y;
            this.newX = newX;
            this.newY = newY;
            this.Map = Map;
        }
    }
}