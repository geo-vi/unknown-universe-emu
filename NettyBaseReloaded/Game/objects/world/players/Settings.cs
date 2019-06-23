using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.players.settings;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Settings : PlayerBaseClass
    {
        public Slotbar Slotbar { get; set; }
        private Window Window { get; set; }

        #region JSON Loaded Stuff

        //public netty.commands.new_client.UserSettingsCommand NewClientUserSettingsCommand { get; set; }
        public netty.commands.old_client.UserSettingsCommand OldClientUserSettingsCommand { get; set; }

        public netty.commands.old_client.ShipSettingsCommand OldClientShipSettingsCommand { get; set; }
        //public netty.commands.new_client.ShipSettingsCommand NewShipSettingsCommand { get; set; }

        public netty.commands.old_client.UserKeyBindingsUpdate OldClientKeyBindingsCommand { get; set; }

        #endregion

        public int ASSET_VERSION = 0;

        public Ammunition CurrentAmmo;

        public Ammunition CurrentRocket;

        public Ammunition CurrentHellstorm;
        
        public Settings(Player player) : base(player)
        {
            Slotbar = new Slotbar(player);
            Window = new Window();
            LoadSettings();
            CurrentAmmo = GetLaserLoot();
            CurrentRocket = GetRocketLoot();
            CurrentHellstorm = GetRocketLauncherLoot();
        }

        public void LoadSettings()
        {
            if (Player.UsingNewClient)
            {
                Console.WriteLine("TODO: New client settings");
                //throw new NotImplementedException();
            }
            else
            {
                OldClientUserSettingsCommand =
                    World.DatabaseManager.GetPlayerGameplaySettings(Player) as
                        netty.commands.old_client.UserSettingsCommand;
                if (OldClientUserSettingsCommand == null)
                {
                    var qs = new netty.commands.old_client.QualitySettingsModule(false, 3, 3, 3, true, 3, 3, 3, 3, 3,
                        3);
                    var asm = new netty.commands.old_client.AudioSettingsModule(false, false, false);
                    var ws = new netty.commands.old_client.WindowSettingsModule(false, 0,
                        "0,444,-1,0,1,1057,329,1,20,39,530,0,3,1021,528,1,5,-10,-6,0,24,463,15,0,10,101,307,0,36,100,400,0,13,315,122,0,23,1067,132,0",
                        "5,240,150,20,300,150,36,260,175,", 11, "313,480", "23,0,24,0,25,1,26,0,27,0", "313,451", "0",
                        "313,500", "0");
                    var gm = new netty.commands.old_client.GameplaySettingsModule(false, true, true, true, true, true,
                        true, true);
                    var ds = new netty.commands.old_client.DisplaySettingsModule(false, true, true, true, true, true,
                        false, true, true, true, true, true, true, true, true, true);

                    OldClientUserSettingsCommand =
                        new netty.commands.old_client.UserSettingsCommand(qs, ds, asm, ws, gm);
                }

                OldClientShipSettingsCommand =
                    World.DatabaseManager.GetPlayerShipSettings(Player) as
                        netty.commands.old_client.ShipSettingsCommand;
                if (OldClientShipSettingsCommand == null)
                    OldClientShipSettingsCommand = new netty.commands.old_client.ShipSettingsCommand("", "", 0, 0, 0);

                OldClientKeyBindingsCommand =
                    World.DatabaseManager.GetPlayerKeySettings(Player) as netty.commands.old_client.UserKeyBindingsUpdate;
                if (OldClientKeyBindingsCommand == null)
                {
                    var hotkeys = new List<Hotkey>
                    {
                        new Hotkey(Hotkey.ACTIVATE_LASER, (int) Keys.ControlKey, false),
                        new Hotkey(Hotkey.CHANGE_CONFIG, (int) Keys.C, false),
                        new Hotkey(Hotkey.JUMP, (int) Keys.J, false),
                        new Hotkey(Hotkey.LAUNCH_ROCKET, (int) Keys.Space, false),
                        new Hotkey(Hotkey.PERFORMANCE_MONITORING, (int) Keys.F, false),
                        new Hotkey(Hotkey.PET_ACTIVATE, (int) Keys.E, false),
                        new Hotkey(Hotkey.PET_GUARD_MODE, (int) Keys.R, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D1, 0, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D2, 1, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D3, 2, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D4, 3, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D5, 4, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D6, 5, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D7, 6, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D8, 7, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D9, 8, false),
                        new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D0, 9, false),
                        new Hotkey(Hotkey.TOGGLE_WINDOWS, (int) Keys.H, false),
                        new Hotkey(Hotkey.LOGOUT, (int) Keys.L, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F1, 0, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F2, 1, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F3, 2, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F4, 3, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F5, 4, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F6, 5, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F7, 6, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F8, 7, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F9, 8, false),
                        new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F10, 9, false),
                        new Hotkey(Hotkey.ZOOM_IN, (int) Keys.Oemplus, false),
                        new Hotkey(Hotkey.ZOOM_OUT, (int) Keys.OemMinus, false),
                    };

                    var keys = hotkeys.Select(hotkey => hotkey.Object).ToList();
                    OldClientKeyBindingsCommand = new netty.commands.old_client.UserKeyBindingsUpdate(keys.ConvertAll(x => x as netty.commands.old_client.UserKeyBindingsModule), false);
                }

                SaveSettings();
                //ASSET_VERSION = World.DatabaseManager.GetPlayerAssetsVersion(Player);
            }
        }

        public void SaveSettings()
        {
            World.DatabaseManager.SavePlayerGameplaySettings(this);
            World.DatabaseManager.SavePlayerShipSettings(this);
            //World.DatabaseManager.SetPlayerAssetsVersion(this);
            World.DatabaseManager.SavePlayerKeySettings(this);
        }

        private Ammunition GetLaserLoot()
        {
//            if (OldClientShipSettingsCommand == null)
//            {
//                return Player.Information.Ammunitions["ammunition_laser_lcb-10"];
//            }
//
//            var lootId = Slotbar.Items.LaserIds[OldClientShipSettingsCommand.selectedLaser - 1];
//            return Player.Information.Ammunitions[lootId];
            return Player.Information.Ammunitions["ammunition_laser_lcb-10"];
        }

        private Ammunition GetRocketLoot()
        {
//            if (OldClientShipSettingsCommand == null)
//            {
//                return Player.Information.Ammunitions["ammunition_rocket_r-310"];
//            }
//
//            var lootId = Slotbar.Items.LaserIds[OldClientShipSettingsCommand.selectedRocket - 1];
//            return Player.Information.Ammunitions[lootId];
            return Player.Information.Ammunitions["ammunition_rocket_r-310"];
        }

        private Ammunition GetRocketLauncherLoot()
        {
//            if (OldClientShipSettingsCommand == null)
//            {
//                return Player.Information.Ammunitions["ammunition_rocketlauncher_eco-10"];
//            }
//
//            Console.WriteLine("Selected HST rocket: " + OldClientShipSettingsCommand.selectedHellstormRocket);
            //var lootId = Slotbar.Items.LaserIds[OldClientShipSettingsCommand.selectedLaser - 1];
            return Player.Information.Ammunitions["ammunition_rocketlauncher_eco-10"];
        }
    }
}
