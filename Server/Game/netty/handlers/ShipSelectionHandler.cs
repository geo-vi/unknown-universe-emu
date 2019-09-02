using System;
using DotNetty.Buffers;
using Server.Game.controllers.characters;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class ShipSelectionHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var selectionCommand = new ShipSelectionRequest();
            selectionCommand.readCommand(buffer);

            var targetId = selectionCommand.targetId;

            if (!gameSession.Player.Spacemap.Entities.ContainsKey(targetId))
            {
                Out.QuickLog("Cannot find entity id '" + targetId + "' on current spacemap", LogKeys.ERROR_LOG);
                throw new Exception("Selection ID does not exist on the current spacemap");
            }

            if (!gameSession.Player.RangeView.CharactersInRenderRange.ContainsKey(targetId))
            {
                //anticheatum?
                Out.WriteLog("Trying to select entity id '" + targetId + "' that isn't rendered yet", LogKeys.PLAYER_LOG, gameSession.Player.Id);
                return;
            }

            var target = gameSession.Player.RangeView.CharactersInRenderRange[targetId];
            
            gameSession.Player.Controller.GetInstance<CharacterSelectionController>().SelectAttackable(target);
        }
    }
}