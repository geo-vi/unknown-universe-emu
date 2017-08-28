using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class SelectBatteryHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            if (gameSession == null) return;

            parser.Short();
            var battery = parser.Short();

            gameSession.Player.Settings.Slotbar.SelectedLaser = battery;
        }
    }
}
