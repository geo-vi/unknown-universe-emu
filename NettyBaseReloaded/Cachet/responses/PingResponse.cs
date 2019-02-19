using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Cachet.responses
{
    class PingResponse : IResponse
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public override string Data
        {
            get;
            set;
        }

        public bool Valid => Data == "Pong!";
    }
}
