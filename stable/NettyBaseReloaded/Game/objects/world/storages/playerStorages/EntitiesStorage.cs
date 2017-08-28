using System.Collections.Generic;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;

namespace NettyBaseReloaded.Game.objects.world.storages.playerStorages
{
    class EntitiesStorage
    {
        public Dictionary<int, Object> LoadedObjects = new Dictionary<int,Object>();

        public Dictionary<string, POI> LoadedPOI = new Dictionary<string, POI>();
    }
}