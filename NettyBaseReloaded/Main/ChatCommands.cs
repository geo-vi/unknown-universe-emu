using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Main.commands;

namespace NettyBaseReloaded.Main
{
    class ChatCommands
    {
        public static Dictionary<string, Command> Commands = new Dictionary<string, Command>();

        public static void Add()
        {
            Commands.Add("help", new HelpCommand());
            Commands.Add("clean", new CleanCommand());
            Commands.Add("player", new PlayerCommand());
            Commands.Add("set", new SetCommand());
            Commands.Add("debug", new DebugCommand());
            Commands.Add("destroy", new DestroyCommand());
            Commands.Add("start", new StartCommand());
            Commands.Add("info", new InfoCommand());
            Commands.Add("create", new CreateCommand());
            Commands.Add("rcon", new RconCommand());
            Commands.Add("w", new WhisperCommand());
        }

        public static void Handle(ChatSession session, string txt)
        {
            var packet = txt.Replace("/", "");
            Console.WriteLine("CHAT COMMAND:>" + txt);
            if (packet.Contains(" "))
            {
                var splitted = packet.Split(' ');
                if (Commands.ContainsKey(splitted[0]))
                {
                    Commands[splitted[0]].Execute(session, splitted);
                    Console.WriteLine($"Command {splitted[0]} found. Executing...");
                }
            }
            else if (Commands.ContainsKey(packet))
                Commands[packet].Execute(session);
            else MessageController.System(session.Player, "ERROR: Command doesn't exist");
        }
    }
}
