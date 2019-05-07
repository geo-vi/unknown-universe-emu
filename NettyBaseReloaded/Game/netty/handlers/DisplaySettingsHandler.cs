using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class DisplaySettingsHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new DisplaySettingsRequest();
            cmd.readCommand(buffer);

            var pDisplay = gameSession.Player.Settings.OldClientUserSettingsCommand.DisplaySettingsModule;

            pDisplay.alwaysDraggableWindows = cmd.alwaysDraggableWindows;
            pDisplay.displayBoxes = cmd.displayBoxes;
            pDisplay.displayCargoboxes = cmd.displayCargoboxes;
            pDisplay.displayChat = cmd.displayChat;
            pDisplay.displayDrones = cmd.displayDrones;
            pDisplay.displayHitpointBubbles = cmd.displayHitpointBubbles;
            pDisplay.displayNotifications = cmd.displayNotifications;
            pDisplay.displayPenaltyCargoboxes = cmd.displayPenaltyCargoboxes;
            pDisplay.displayPlayerName = cmd.displayPlayerName;
            pDisplay.displayResources = cmd.displayResources;
            pDisplay.displayWindowBackground = cmd.displayWindowBackground;

            gameSession.Player.Settings.SaveSettings();
        }
    }
}
