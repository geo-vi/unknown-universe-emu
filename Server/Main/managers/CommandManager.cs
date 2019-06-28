using Server.Main.commands;
using System;
using System.Collections.Generic;

namespace Server.Main.managers
{
    class CommandManager
    {
        public static Dictionary<string, GlobalCommand> RegisteredChatCommands = new Dictionary<string, GlobalCommand>();
        public static Dictionary<string, GlobalCommand> RegisteredConsoleCommands = new Dictionary<string, GlobalCommand>();

        public void CreateCommands()
        {

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
