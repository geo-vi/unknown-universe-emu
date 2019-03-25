using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Helper.packets.requests
{
    class KillRequest : IRequest
    {
        public const string Prefix = "kill";

        public void Read(string[] args = null)
        {
            
        }
    }
}
