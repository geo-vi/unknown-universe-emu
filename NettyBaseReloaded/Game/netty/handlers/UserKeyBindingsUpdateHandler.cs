using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class UserKeyBindingsUpdateHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient) return;

            var bindings = new UserKeyBindingsUpdate();
            bindings.readCommand(bytes);
            gameSession.Player.Settings.OldClientKeyBindingsCommand = bindings;
            gameSession.Player.Settings.SaveSettings();
        }
    }
}
