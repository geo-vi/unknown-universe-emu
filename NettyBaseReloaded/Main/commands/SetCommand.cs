using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;

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
                var playerId = int.Parse(args[2]);
                var gs = World.StorageManager.GetGameSession(playerId);
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
    }
}
