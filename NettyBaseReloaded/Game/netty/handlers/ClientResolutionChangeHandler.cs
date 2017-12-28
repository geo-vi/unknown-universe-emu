using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ClientResolutionChangeHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var cmd = new ClientResolutionChangeRequest();
            cmd.readCommand(bytes);
            gameSession.Player.Settings.ASSET_VERSION = cmd.resolutionId;
            gameSession.Player.Settings.SaveSettings();
        }
    }
}
