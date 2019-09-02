using System;
using Server.Game.controllers.players;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Main.objects;
using Server.Utils;

namespace Server.Main.commands.server
{
    class WhereAmICommand : GlobalCommand
    {
        public WhereAmICommand() : base("whereami", "Showing position depending on where you are", true, new SubHelp[]
        {
            new SubHelp("id", "ID of player"),
            new SubHelp("Name", "Player name"), 
        })
        {
        }

        public override void Execute(string[] args = null)
        {
            try
            {
                if (int.TryParse(args[1], out var id))
                {
                    var session = GameStorageManager.Instance.FindSession(id);
                    if (session == null)
                    {
                        Out.QuickLog($"Session for ID '{id}' not found", LogKeys.SERVER_LOG);
                    }
                    else
                    {
                        PrebuiltLegacyCommands.Instance.ServerMessage(session.Player, "Position of " + session.Player.Name + " is (" + session.Player.Position.X + "; " + session.Player.Position.Y + ")");
                    }
                }
            }
            catch (Exception)
            {
                Out.QuickLog("Something went wrong with position command", LogKeys.ERROR_LOG);
            }
        }
    }
}