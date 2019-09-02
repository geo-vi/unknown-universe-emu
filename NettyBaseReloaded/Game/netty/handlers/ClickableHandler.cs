using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.new_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.map.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ClickableHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new ClickableRequest();
            cmd.readCommand(buffer);
            var obj = gameSession.Player.Range.Objects[cmd.clickableId];
            (obj as IClickable)?.click(gameSession.Player);
        }
    }
}
