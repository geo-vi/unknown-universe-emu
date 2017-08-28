using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloadedController.Main.global.objects
{
    class Character
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Spacemap Spacemap { get; set; }

        public Vector Position { get; set; }

        public Character(int id, string name, Spacemap spacemap, Vector pos)
        {
            Id = id;
            Name = name;
            Spacemap = spacemap;
            Position = pos;
        }
    }
}
