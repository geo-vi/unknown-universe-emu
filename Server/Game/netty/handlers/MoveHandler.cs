﻿using System;
using DotNetty.Buffers;
using Server.Game.controllers;
using Server.Game.controllers.server;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.implementable;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class MoveHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var movementRequest = new MoveRequest();
            movementRequest.readCommand(buffer);

            var player = gameSession.Player;
            var actualPos = player.Position;

            if (movementRequest.positionX != actualPos.X || movementRequest.positionY != actualPos.Y)
            {
                Out.WriteLog("Something is wrong with player position", LogKeys.PLAYER_LOG, player.Id);
            }

            Console.WriteLine("destination : " + movementRequest.targetX + " " + movementRequest.targetY);
            
            ServerController.Get<MovementController>().CreateMovement(player, new Vector(movementRequest.targetX, movementRequest.targetY));
        }
    }
}