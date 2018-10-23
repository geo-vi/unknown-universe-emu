using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;

namespace NettyBaseReloaded.Main.commands
{
    class RestartCommand : Command
    {
        public RestartCommand(string name, string desc, bool display = true, SubHelp[] subHelps = null) : base(name, desc, display, subHelps)
        {
        }

        public override void Execute(string[] args = null)
        {
            
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            if (session.Player.RCON) Execute(args);
        }
    }
}
