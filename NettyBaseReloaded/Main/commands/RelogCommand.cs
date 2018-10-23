using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;

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

        public override void Execute(ChatSession session, string[] args = null)
        {
            session.Kick("Relogging...");
        }
    }
}
