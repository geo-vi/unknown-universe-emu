using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;

namespace NettyBaseReloaded.Main.commands
{
    class UnbugCommand : Command
    {
        public UnbugCommand() : base("unbug", "Unbugs a user", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            throw new NotImplementedException();
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            var gameSession = session.GetEquivilentGameSession();
            if (gameSession != null)
            {
                gameSession.Player.MoveToMap(gameSession.Player.Spacemap, gameSession.Player.Position, 0);
            }
        }
    }
}
