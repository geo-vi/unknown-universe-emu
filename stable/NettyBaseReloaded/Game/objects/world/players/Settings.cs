using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects.world.players.settings;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Settings
    {
        public Gameplay Gameplay { get; set; }
        public Slotbar Slotbar { get; set; }
        public Window Window { get; set; }

        public string CurrentAmmo { get; set; }

        public string CurrentRocket { get; set; }

        public Settings()
        {
            Gameplay = new Gameplay();
            Slotbar = new Slotbar();
            Window = new Window();
        }
    }
}
