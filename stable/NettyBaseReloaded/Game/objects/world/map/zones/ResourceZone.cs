using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.objects.world.map.zones
{
    class ResourceZone : Zone
    {
        public Types Type { get; set; }

        public double Density { get; set; }

        public ResourceZone(int id, Vector topLeft, Vector botRight) : base(id, topLeft, botRight)
        {

        }
    }
}