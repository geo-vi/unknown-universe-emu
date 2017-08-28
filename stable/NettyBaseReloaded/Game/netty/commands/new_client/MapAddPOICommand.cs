using System.Collections.Generic;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class MapAddPOICommand
    {
        public const short ID = 18769;

        public static byte[] write(string poiId, POITypeModule poiType, string poiTypeSpecification, POIDesignModule design,
            short shape, List<int> shapeCoordinates, bool inverted, bool active)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(active);
            cmd.Integer(shapeCoordinates.Count);
            foreach (var _loc2_ in shapeCoordinates)
            {
                cmd.Integer(_loc2_ >> 7 | _loc2_ << 25);
            }
            cmd.AddBytes(design.write());
            cmd.AddBytes(poiType.write());
            cmd.UTF(poiId);
            cmd.UTF(poiTypeSpecification);
            cmd.Short(-26695);
            cmd.Short(shape);
            cmd.Boolean(inverted);
            return cmd.ToByteArray();
        }
    }
}