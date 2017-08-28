using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;

namespace NettyBaseReloaded.Socketty.handlers
{
    class RefreshRequestHandler : IHandler
    {
        public void execute(string param)
        {
            var split = param.Split('|');

            var playerId = Convert.ToInt32(split[1]);

            World.StorageManager.GameSessions[playerId]?.Player.BasicDbRefresh(true);
        }
    }
}
