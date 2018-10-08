using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.commands
{
    class InfoCommand : Command
    {
        public InfoCommand() : base("info", "Info about server")
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args == null || args.Length < 2)
                return;
            switch (args[1])
            {
                case "players":
                    StringBuilder worldSessions = new StringBuilder();
                    foreach (var worldSession in World.StorageManager.GameSessions)
                        worldSessions.Append($"{worldSession.Key}:{worldSession.Value.Player.Name} ");
                    StringBuilder chatSessions = new StringBuilder();
                    foreach (var chatSession in Chat.Chat.StorageManager.ChatSessions)
                        worldSessions.Append($"{chatSession.Key}:{chatSession.Value.Player.Name} ");
                    Console.WriteLine($"World sessions ({World.StorageManager.GameSessions.Count}): {worldSessions}\nChat sessions ({Chat.Chat.StorageManager.ChatSessions.Count}): {chatSessions}");
                    break;
                case "mysql":
                    break;
            }
        }

        public override void Execute(ChatSession session, string[] args = null) => Execute(args);
    }
}
