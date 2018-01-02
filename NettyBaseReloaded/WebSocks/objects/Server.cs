using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.WebSocks.objects
{
    class Server
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public string Name { get; set; }
        public string Shortcut { get; set; }
        public string Ip { get; set; }
        public bool Open { get; set; }
    }
}
