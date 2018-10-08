using System;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Main.commands
{
    class CleanCommand : Command
    {
        public CleanCommand() : base("clean", "Clears the console")
        {

        }

        public override void Execute(string[] args = null)
        {
            Draw.Logo();
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
        }
    }
}