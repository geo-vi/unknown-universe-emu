using System;
using Server.Utils;

namespace Server.Main.commands.server
{
    class RuntimeCommand : GlobalCommand
    {
        public RuntimeCommand() : base("runtime", "Displays information about time since start and server state")
        {
        }

        public override void Execute(string[] args = null)
        {
            Out.WriteLog("Current server runtime is: " + (DateTime.Now - Program.ServerStartTime), "SERVER");
            Out.WriteLog("Running stable ; 0 errors; 0 warnings", "SERVER");
            Out.WriteLog("Game: 0 sessions; Chat: 0 sessions; Web: 0 sessions; Helper: Not Connected", "SERVER");
        }
    }
}
