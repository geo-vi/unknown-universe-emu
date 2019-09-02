using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Main.commands
{
    class RelogCommand : Command
    {
        public RelogCommand() : base("relog", "Relog Command", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
        }

        public override void Execute(ChatSession chatSession, string[] args = null)
        {
            var gameSession = chatSession.GetEquivilentGameSession();
            if (gameSession != null)
            {
                var player = gameSession.Player;
                //if (player.LastCombatTime < DateTime.Now.AddSeconds(3))
                //{
                //    Packet.Builder.LegacyModule(gameSession, "Relogs on");
                //}
                //Packet.Builder.LegacyModule(gameSession, "0|A|STD|Relogging...");
                //Packet.Builder.SendErrorCommand(gameSession, Game.objects.world.SessionErrors.DISCONNECT);
            }
            chatSession.Kick("Relogging...");
        }
    }
}
