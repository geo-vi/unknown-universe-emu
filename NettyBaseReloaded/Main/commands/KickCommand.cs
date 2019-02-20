using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;

namespace NettyBaseReloaded.Main.commands
{
    class KickCommand : Command
    {
        public KickCommand(string name, string desc, bool display = true, SubHelp[] subHelps = null) : base(name, desc, display, subHelps)
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args == null || args.Length < 2)
            {
                Console.WriteLine("/kick [game/chat] [id]");
                return;
            }

            switch (args[1])
            {
                case "game":
                    break;
                case "chat":
                    break;
            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            // kick from chat
        }
    }
}
