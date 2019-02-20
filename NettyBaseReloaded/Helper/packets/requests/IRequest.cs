using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Helper.packets.commands;

namespace NettyBaseReloaded.Helper.packets.requests
{
    interface IRequest
    {
        void Read(string[] args = null);
    }
}
