using System;
using Server.Game.controllers.players;
using Server.Game.managers;
using Server.Main.objects;
using Server.Utils;

namespace Server.Main.commands.server
{
    class LogoutCommand : GlobalCommand
    {
        public LogoutCommand() : base("logout", "Logging out the player", true, new []
        {
            new SubHelp("id", "Logging player out by ID"),
            new SubHelp("name", "Logging player out by NAME"), 
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
                        session.Player.Controller.GetInstance<LogoutController>().CreateLogoutProcess(5000);
                        Out.QuickLog($"Logout sequence (5000ms) for ID '{id}' started", LogKeys.SERVER_LOG);
                    }
                }
            }
            catch (Exception)
            {
                Out.QuickLog("Something went wrong with logout command", LogKeys.ERROR_LOG);
            }
        }
    }
}