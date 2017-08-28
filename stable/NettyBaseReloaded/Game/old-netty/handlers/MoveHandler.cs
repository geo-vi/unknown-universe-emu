using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty.newcommands;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Utils;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class MoveHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var newVector = new Vector(0,0);

            if (gameSession.Player.UsingNewClient)
            {
                var simpleCmd = new SimpleCommand(bytes);
                simpleCmd.readShort();
                var cmd = new MovementRequest(simpleCmd);
                cmd.readCommand();
                newVector = new Vector(cmd.NewX, cmd.NewY);
                Console.WriteLine(JsonConvert.SerializeObject(newVector));
            }
            else
            {
                var parser = new ByteParser(bytes);

                int currentPosX = parser.Int();
                int targetPosY = parser.Int();
                int targetPosX = parser.Int();
                int currentPosY = parser.Int();
                newVector = new Vector(targetPosX, targetPosY);
            }
            MovementController.Move(gameSession.Player, newVector);   
        }
    }
}
