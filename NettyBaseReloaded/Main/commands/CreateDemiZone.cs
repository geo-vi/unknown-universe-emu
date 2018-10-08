using System;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.zones;

namespace NettyBaseReloaded.Main.commands
{
    class CreateDemiZone : Command
    {
        public CreateDemiZone() : base("createdemizone", "Creates a demi zone") { }

        public override void Execute(string[] args = null)
        {
            if (args != null && args.Length > 4)
            {
                var mapId = int.Parse(args[1]);

                var botLeftX = int.Parse(args[2]);
                var botLeftY = int.Parse(args[3]);

                var topRightX = int.Parse(args[4]);
                var topRightY = int.Parse(args[5]);

                World.StorageManager.Spacemaps[mapId].CreateDemiZone(new Vector(botLeftX, botLeftY), new Vector(topRightX, topRightY));
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