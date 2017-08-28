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
        public Slotbar Slotbar { get; set; }
        public Window Window { get; set; }

        #region JSON Loaded Stuff

        public netty.commands.new_client.UserSettingsCommand NewClientUserSettingsCommand;
        public netty.commands.old_client.UserSettingsCommand OldClientUserSettingsCommand;

        public List<Hotkey> Hotkeys;

        #endregion

        public string CurrentAmmo { get; set; }

        public string CurrentRocket { get; set; }

        public Settings()
        {
            Slotbar = new Slotbar();
            Window = new Window();
        }
    }
}
