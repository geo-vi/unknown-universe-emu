using System;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Main.commands
{
    class PlayerCommand : Command
    {
        public PlayerCommand() : base("player", "Player [id + sub]", true, new []
        {
            new SubHelp("tp", "Teleports player [x, y]"),
            new SubHelp("stats", "Player stats"),
//            new SubHelp("sethp", "Sets HP"),1
//            new SubHelp("setshd", "Sets shield"),
        })
        {

        }

        public override void Execute(string[] args)
        {
            try
            {

                if (args == null) return;
                if (args.Length < 3) return;

                var id = int.Parse(args[1]);

                if (World.StorageManager.GetGameSession(id) == null)
                {
                    Console.WriteLine("Player is not connected or wrong ID");
                    return;
                }

                var player = World.StorageManager.GetGameSession(id).Player;

                switch (args[2])
                {
                    case "tp":
                        if (args[3] == "destination") player.SetPosition(player.Destination);
                        else player.SetPosition(new Vector(int.Parse(args[3]), int.Parse(args[4])));
                        break;
                    case "heal":
                        player.Controller.Heal.Execute(player.MaxHealth);
                        player.Controller.Heal.Execute(player.MaxShield, 0, HealType.SHIELD);
                        break;
                    case "stats":
                        Console.WriteLine("Player {0}->{1}", player.Id, player.Name);
                        Console.WriteLine("HP " + player.CurrentHealth + " SHD " + player.CurrentShield);
                        Console.WriteLine("Position " + player.Position);
                        break;
                    case "givepet":
                        //todo
                        break;
                    default:
                        Console.WriteLine("Invalid arguement");
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid args");
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