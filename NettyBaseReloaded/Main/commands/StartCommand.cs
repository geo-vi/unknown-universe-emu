using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.events;

namespace NettyBaseReloaded.Main.commands
{
    class StartCommand : Command
    {
        public StartCommand() : base("start", "Starting a game-event")
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args == null || args.Length < 2) return;
            switch (args[1])
            {
                case "event":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Choose which event would you like to start?");
                        foreach (var gameEvent in World.StorageManager.Events)
                        {
                            Console.WriteLine($"{gameEvent.Key}:{gameEvent.Value.Name}");
                        }
                        break;
                    }
                    int eventId = 0;
                    if (int.TryParse(args[2], out eventId))
                    {
                        if (World.StorageManager.Events.ContainsKey(eventId))
                            World.StorageManager.Events[eventId].Start();
                    }
                    break;
                default:
                    Console.WriteLine("Invalid arg\nPossible args: event");
                    break;
            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            var id = session.Player.Id;
            var sessionId = session.Player.SessionId;
            var worldSession = World.StorageManager.GetGameSession(id);
            if (worldSession != null && worldSession.Player.Id == id && worldSession.Player.SessionId == sessionId &&
                worldSession.Player.RankId == Rank.ADMINISTRATOR)
            {
                Execute(args);
            }

        }
    }
}
