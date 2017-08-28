using System;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Main.commands
{
    class Clean : Command
    {
        public Clean() : base("clean", "Clears the console")
        {

        }

        public override void Execute(string[] args = null)
        {
            Draw.Logo();
        }
    }
}