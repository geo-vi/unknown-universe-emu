using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Main.global.objects;
using NettyBaseReloadedController.Networking;
using NettyBaseReloadedController.Utils;

namespace NettyBaseReloadedController.Main.netty.handlers
{
    class PlayerDataCommandHandler : IHandler
    {
        public void execute(ControllerClient client, ByteParser parser)
        {
            var id = parser.Int();
            var name = parser.UTF();
            var rankId = parser.Int();
            var factionId = parser.Int();
            var mapId = parser.Int();
            var x = parser.Int();
            var y = parser.Int();

            var map = Controller.Global.StorageManager.Spacemaps[mapId];
            if (map.Entities.ContainsKey(id))
            {
                map.Entities[id].Position = new Vector(x, y);
            }
            else map.Entities.Add(id, new Player(id, name, rankId, factionId, map, new Vector(x,y)));
        }
    }
}
