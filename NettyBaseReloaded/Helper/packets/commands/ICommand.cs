using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Helper.packets.commands
{
    abstract class ICommand
    {
        public abstract string Prefix { get; }

        public string Packet { get; private set; }

        protected ICommand()
        {
            Packet = Prefix;
        }

        protected void AddParam(string param)
        {
            Packet += "|" + param;
        }
    }
}
