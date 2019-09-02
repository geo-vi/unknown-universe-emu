using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Utils;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class MoveHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var newVector = new Vector(0, 0);

            if (gameSession.Player.UsingNewClient)
            {
                var movementCommand = new commands.new_client.requests.MoveRequest();
                movementCommand.readCommand(buffer);

                newVector = new Vector(movementCommand.NewX, movementCommand.NewY);
            }
            else
            {
                var movementCommand = new commands.old_client.requests.MoveRequest();
                movementCommand.readCommand(buffer);

                newVector = new Vector(movementCommand.targetX, movementCommand.targetY);
            }

            //Console.WriteLine("{0}, {1}", gameSession.Player.Id, newVector);
            MovementController.Move(gameSession.Player, newVector);
        }

    }
}
