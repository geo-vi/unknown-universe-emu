using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;

namespace NettyBaseReloaded.Main.commands
{
    class UsersCommand : Command
    {
        public UsersCommand(string name, string desc, bool display = true, SubHelp[] subHelps = null) : base(name, desc, display, subHelps)
        {
        }

        public override void Execute(string[] args = null)
        {
            throw new NotImplementedException();
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            throw new NotImplementedException();
        }
    }
}
