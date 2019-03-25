using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;

namespace NettyBaseReloaded.Main.commands
{
    class KillCommand : Command
    {
        public KillCommand() : base("kill", "Killswitch", false, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args == null || args.Length < 2)
            {
                return;
            }

            var id = Convert.ToInt32(args[2]);
            var gameSession = World.StorageManager.GetGameSession(id);
            switch (args[1])
            {
                case "controllertick":
                    Global.TickManager.Remove(gameSession.Player.Controller);
                    break;
                case "playertick":
                    Global.TickManager.Remove(gameSession.Player);
                    break;
            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            throw new NotImplementedException();
        }
    }
}
