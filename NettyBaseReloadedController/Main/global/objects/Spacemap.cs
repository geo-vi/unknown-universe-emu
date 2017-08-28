using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloadedController.Main.global.objects
{
    class Spacemap
    {
        public int Id { get; }

        public Dictionary<int, Character> Entities { get; set; }

        public Spacemap(int id)
        {
            Id = id;
            Entities = new Dictionary<int, Character>();
        }

    }
}
