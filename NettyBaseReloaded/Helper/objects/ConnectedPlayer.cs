using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Helper.objects
{
    class ConnectedPlayer
    {
        public int Id { get; set; }

        public int GlobalId { get; set; }

        public string Name { get; set; }

        public ConnectedPlayer(int id, int globalId, string name)
        {
            Id = id;
            GlobalId = globalId;
            Name = name;
        }
    }
}
