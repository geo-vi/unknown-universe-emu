using System;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.settings;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ItemSelectionHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient)
            {
                var cmd = new commands.new_client.requests.ItemSelectionRequest();
                cmd.readCommand(bytes);
                gameSession.Player.Controller.Miscs.UseItem(cmd.itemId);
            }
            else
            {
                var selectCmd = new commands.old_client.requests.SelectRocketRequest();
                selectCmd.readCommand(bytes);
                var type = selectCmd.type;
                gameSession.Player.Controller.Miscs.UseItem(Converter.AmmoTypeToString(type));
            }
        }
    }
}