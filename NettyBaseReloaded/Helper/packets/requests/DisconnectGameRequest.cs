using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Helper.packets.requests
{
    class DisconnectGameRequest : IRequest
    {
        public const string Prefix = "dcg";

        public void Read(string[] args = null)
        {
            
        }
    }
}
