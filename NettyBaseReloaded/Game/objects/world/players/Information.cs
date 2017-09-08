using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.informations;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Information : PlayerBaseClass
    {
        public BaseInfo Experience { get; set; }

        public BaseInfo Honor { get; set; }

        public BaseInfo Credits { get; set; }

        public BaseInfo Uridium { get; set; }

        public Level Level { get; set; }

        public bool Premium { get; set; }

        public Information(Player player) : base(player)
        {
            Experience = new Experience(player);
            Honor = new Honor(player);
            Credits = new Credits(player);
            Uridium = new Uridium(player);

            UpdateAll();
        }

        public void UpdateAll()
        {
            Experience.Refresh();
            Honor.Refresh();
            Credits.Refresh();
            Uridium.Refresh();
        }
    }
}
