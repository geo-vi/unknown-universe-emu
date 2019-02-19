using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Cachet.responses
{
    abstract class IResponse
    {
        public abstract string Data { get; set; }
    }
}
