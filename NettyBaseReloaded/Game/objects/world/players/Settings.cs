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

        public netty.commands.new_client.UserSettingsCommand NewClientUserSettingsCommand { get; set; }
        public netty.commands.old_client.UserSettingsCommand OldClientUserSettingsCommand { get; set; }

        public netty.commands.old_client.ShipSettingsCommand OldClientShipSettingsCommand { get; set; }
        //public netty.commands.new_client.ShipSettingsCommand NewShipSettingsCommand { get; set; }

        public List<Hotkey> Hotkeys { get; set; }

        #endregion

        public Ammunition CurrentAmmo { get; set; }

        public Ammunition CurrentRocket { get; set; }

        public int ASSET_VERSION = 0;

        public Settings(Player player) : base(player)
        {
            Slotbar = new Slotbar();
            Window = new Window();

            CurrentAmmo = Player.Information.Ammunitions["ammunition_laser_lcb-10"];
            CurrentRocket = Player.Information.Ammunitions["ammunition_rocket_r-310"];

            LoadSettings();
        }

        public void LoadSettings()
        {
            if (Player.UsingNewClient)
            {
                throw new NotImplementedException();
            }
            else
            {
                OldClientUserSettingsCommand = World.DatabaseManager.GetPlayerGameplaySettings(Player) as netty.commands.old_client.UserSettingsCommand;
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
                    SaveSettings();

                ASSET_VERSION = World.DatabaseManager.GetPlayerAssetsVersion(Player);
            }
        }

        public void SaveSettings()
        {
            World.DatabaseManager.SavePlayerGameplaySettings(this);
            World.DatabaseManager.SavePlayerShipSettings(this);
            World.DatabaseManager.SetPlayerAssetsVersion(this);
        }
    }
}
