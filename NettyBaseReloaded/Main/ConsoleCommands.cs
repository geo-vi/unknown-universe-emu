using System;
using System.Collections.Generic;
using NettyBaseReloaded.Main.commands;

namespace NettyBaseReloaded.Main
{
    public class ConsoleCommands
    {
        public static Dictionary<string, Command> Commands = new Dictionary<string, Command>();

        public static void Add()
        {
            Commands.Add("help", new Help());
            Commands.Add("clean", new Clean());
            Commands.Add("player", new Player());
            Commands.Add("temp", new Temp());
            Commands.Add("createdemizone", new CreateDemiZone());
            Commands.Add("debug", new Debug());
        }

        public static void Handle(string txt)
        {
            var packet = txt.Replace("/", "");
            if (packet.Contains(" "))
            {
                var splitted = packet.Split(' ');
                if (Commands.ContainsKey(splitted[0]))
                    Commands[splitted[0]].Execute(splitted);
            }
            else if (Commands.ContainsKey(packet))
                Commands[packet].Execute();
            else Console.WriteLine("ERROR: Command doesn't exist");
        }
    }
}