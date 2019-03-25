using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Helper.packets.commands
{
    class PongCommand : ICommand
    {
        public override string Prefix => "pong";

        public PongCommand()
        {
            AddParam("Pong!");
        }
    }
}
