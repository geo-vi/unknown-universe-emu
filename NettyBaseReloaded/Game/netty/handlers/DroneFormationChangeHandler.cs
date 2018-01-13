using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players.settings;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class DroneFormationChangeHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var formationId = 0;
            if (!gameSession.Player.UsingNewClient)
            {
                var cmd = new DroneFormationChangeRequest();
                cmd.readCommand(bytes);
                formationId = cmd.selectedFormationId;
            }
            
            gameSession.Player.Controller.Miscs.UseItem(Slotbar.Items.FormationIds[formationId]);
        }
    }
}
