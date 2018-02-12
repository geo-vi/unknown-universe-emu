using NettyBaseReloaded.Game;

namespace NettyBaseReloaded.Main.commands
{
    class CreateCommand : Command
    {
        public CreateCommand() : base("create", "The God creator command") { }

        public override void Execute(string[] args = null)
        {
            if (args?.Length > 1)
            {
                if (args[1] == "lowgate")
                {
                    var playerSession = World.StorageManager.GetGameSession(int.Parse(args[2]));
                    playerSession.Player.Spacemap.CreateLoW(playerSession.Player.Position);
                }
                if (args[1] == "relay")
                {
                    var playerSession = World.StorageManager.GetGameSession(int.Parse(args[2]));
                    playerSession.Player.Spacemap.CreateRelayStation(playerSession.Player.Position);
                }
            }
        }
    }
}