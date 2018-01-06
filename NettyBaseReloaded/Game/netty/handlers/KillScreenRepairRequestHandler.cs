using System;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.settings;
using NettyBaseReloaded.Utils;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.netty.commands.old_client;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class KillScreenRepairRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient)
            {
                new NotImplementedException();
            }
            else
            {
                var player = gameSession.Player;

                var cmd = new commands.old_client.requests.KillScreenRepairRequest();
                cmd.readCommand(bytes);

                short repairTypeValue = cmd.selection.repairTypeValue;

                switch (repairTypeValue)
                {
                    case KillScreenOptionTypeModule.BASIC_REPAIR:
                        player.Controller.Destruction.RespawnPlayer();
                        break;
                    case KillScreenOptionTypeModule.AT_JUMPGATE_REPAIR:
                        //TODO spawn at portal (no)
                        break;
                    case KillScreenOptionTypeModule.AT_DEATHLOCATION_REPAIR:
                        //TODO spawn at death location (no)
                        break;
                } // for now we won't add those 2. we want to make more time for repairing
                // make like economy :D
                // 1 hour repair / 30 mins dependsing on ship
                // and every ship repair the ship max hp will decrease by 1-5% random
                // if max hp gets < 1000 ship is not usable anymore

            // you like it?
            //you are crazy :D whybecause i like that
            // =))
            }
        }
    }
}