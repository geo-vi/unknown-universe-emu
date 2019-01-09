using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;

namespace NettyBaseReloaded.Main.commands
{
    class CloseCommand : Command
    {
        public CloseCommand() : base("close", "Closes the server", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            Program.Exit();
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            throw new NotImplementedException();
        }
    }
}
