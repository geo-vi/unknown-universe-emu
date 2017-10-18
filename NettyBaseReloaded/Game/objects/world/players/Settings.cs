using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.players.settings;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Settings : PlayerBaseClass
    {
        public Slotbar Slotbar { get; set; }
        public Window Window { get; set; }

        #region JSON Loaded Stuff

        public netty.commands.new_client.UserSettingsCommand NewClientUserSettingsCommand;
        public netty.commands.old_client.UserSettingsCommand OldClientUserSettingsCommand;

        public List<Hotkey> Hotkeys;

        #endregion

        public Ammunition CurrentAmmo { get; set; }

        public Ammunition CurrentRocket { get; set; }

        public Settings(Player player) : base(player)
        {
            Slotbar = new Slotbar();
            Window = new Window();

            CurrentAmmo = Player.Information.Ammunitions["ammunition_laser_lcb-10"];
            CurrentRocket = Player.Information.Ammunitions["ammunition_rocket_r-310"];
        }
    }
}
