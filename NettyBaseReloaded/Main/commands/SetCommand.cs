using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Main.commands
{
    class SetCommand : Command
    {
        public SetCommand() : base("set", "Set player certain amount of shit", true)
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args != null && args.Length == 4)
            {
                int playerId = 0;
                GameSession gs = null;
                if (int.TryParse(args[2], out playerId))
                {
                    playerId = int.Parse(args[2]);
                    gs = World.StorageManager.GetGameSession(playerId);
                }
                else gs = World.StorageManager.GetGameSession(args[2]);
                if (gs == null) return;
                var value = args[3];
                switch (args[1])
                {
                    case "uridium":
                        gs.Player.Information?.Uridium.Set(int.Parse(value));
                        break;
                    case "credits":
                        gs.Player.Information?.Credits.Set(int.Parse(value));
                        break;

                }
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
