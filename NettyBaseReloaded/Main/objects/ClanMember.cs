using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Main.objects
{
    class ClanMember
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Player Player => World.StorageManager.GetGameSession(Id)?.Player;

        public ClanMember(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
