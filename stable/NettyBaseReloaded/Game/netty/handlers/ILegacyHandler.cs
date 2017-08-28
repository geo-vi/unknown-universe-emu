using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    interface ILegacyHandler
    {
        void execute(GameSession gameSession, string[] param);
    }
}
