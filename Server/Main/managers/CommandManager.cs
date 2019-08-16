using Server.Main.commands;
using System;
using System.Collections.Generic;
using Server.Main.commands.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Main.managers
{
    class CommandManager
    {
        public Dictionary<string, GlobalCommand> RegisteredChatCommands = new Dictionary<string, GlobalCommand>();
        public Dictionary<string, GlobalCommand> RegisteredConsoleCommands = new Dictionary<string, GlobalCommand>();

        public void CreateCommands()
        {
            RegisteredConsoleCommands.Add("help", new HelpCommand());
            RegisteredConsoleCommands.Add("runtime", new RuntimeCommand());
            RegisteredConsoleCommands.Add("msg", new MsgCommand());
            RegisteredConsoleCommands.Add("logout", new LogoutCommand());
            RegisteredConsoleCommands.Add("whereami", new WhereAmICommand());
            Out.QuickLog("Successfully added " + RegisteredConsoleCommands.Count + " console commands", LogKeys.SERVER_LOG);
        }

        public void HandleConsoleInput(string input)
        {
            var packet = input.Replace("/", "");
            if (packet.Contains(" "))
            {
                var splitted = packet.Split(' ');
                if (RegisteredConsoleCommands.ContainsKey(splitted[0]))
                    RegisteredConsoleCommands[splitted[0]].Execute(splitted);
            }
            else if (RegisteredConsoleCommands.ContainsKey(packet))
                RegisteredConsoleCommands[packet].Execute();
            else Console.WriteLine("ERROR: Command '" + packet + "' doesn't exist");

        }

        public void HandleChatInput(string input)
        {

        }
    }
}
