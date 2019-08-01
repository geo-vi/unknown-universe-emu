using System;
using Newtonsoft.Json;
using Server.Game.managers;
using Server.Game.netty.commands;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.packet.prebuiltCommands
{
    class PrebuiltPlayerCommands : AbstractPrebuiltCommand
    {
        public static PrebuiltPlayerCommands Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PrebuiltPlayerCommands();
                }
                return _instance;
            }
        }

        private static PrebuiltPlayerCommands _instance;

        public void ShipInitializationCommand(Player player)
        {
            try
            {
                var session = GameStorageManager.Instance.FindSession(player);
                if (session == null) return;
                
                var client = session.GameClient;

                if (player.UsingNewClient)
                {
                }
                else
                {
                    Packet.Builder.BuildCommand(client, Commands.SHIP_INITIALIZATION_COMMAND, false,
                        player.Id, player.Name, player.Hangar.ShipDesign.Id, player.Speed, player.CurrentShield,
                        player.MaxShield, player.CurrentHealth, player.MaxHealth, player.Cargo.CargoSpaceLeft,
                        player.Cargo.MaxCargoSpace,
                        player.CurrentNanoHull, player.MaxNanoHull, player.Position.X, player.Position.Y,
                        player.Spacemap.Id, player.FactionId, player.Clan.Id, Int32.MaxValue - 1, Int32.MaxValue - 1, player.GetCurrentConfiguration().ExpansionStage,
                        player.Information.Premium, player.Information.Experience, player.Information.Honor, player.Information.Level.Id, player.Information.Credits, 
                        player.Information.Uridium, player.Information.Jackpot, player.RankId, player.Clan.Tag, player.Gates.GetGateRings(), false);
                }
                Out.WriteLog("Ship Initialized", LogKeys.PLAYER_LOG, player.Id);
            }
            catch (Exception)
            {
                Out.WriteLog("Failed sending a ship initializer", LogKeys.ERROR_LOG, player.Id);
            }
        }

        public void CreateShipCommand(Player player, Character createdInstance)
        {
            try
            {
                //todo..
            }
            catch (Exception)
            {
                Out.QuickLog("Failed creating a ship for player " + player.Id, LogKeys.ERROR_LOG);
            }
        }

        public void CreateSettingsCommand(Player player)
        {
            var session = GameStorageManager.Instance.FindSession(player);
            if (session == null)
            {
                return;
            }
            
            var qualitySettings = player.Settings.GetSettings<QualitySettings>();
            var qualitySettingsModule = new netty.commands.old_client.QualitySettingsModule(qualitySettings.Unset, qualitySettings.QualityAttack,
                qualitySettings.QualityBackground, qualitySettings.QualityPresetting, qualitySettings.QualityCustomized,
                qualitySettings.QualityPOIzone, qualitySettings.QualityShip, qualitySettings.QualityEngine, qualitySettings.QualityExplosion,
                qualitySettings.QualityCollectables, qualitySettings.QualityEffect);

            var displaySettings = player.Settings.GetSettings<DisplaySettings>();
            var displaySettingsModule = new commands.old_client.DisplaySettingsModule(displaySettings.Unset, displaySettings.DisplayPlayerName,
                displaySettings.DisplayResource, displaySettings.DisplayBoxes, displaySettings.DisplayHitpointBubbles, displaySettings.DisplayChat,
                displaySettings.DisplayDrones, displaySettings.DisplayCargoboxes, displaySettings.DisplayPenaltyCargoboxes, 
                displaySettings.DisplayWindowBackground, displaySettings.DisplayNotifications, displaySettings.PreloadUserShips, 
                displaySettings.AlwaysDraggableWindows, displaySettings.ShipHovering, displaySettings.ShowSecondQuickslotBar, 
                displaySettings.UseAutoQuality);
            
            var audioSettings = player.Settings.GetSettings<AudioSettings>();
            var audioSettingsModule = new commands.old_client.AudioSettingsModule(audioSettings.Unset, audioSettings.Sound, audioSettings.Music);

            var windowSettings = player.Settings.GetSettings<WindowSettings>();
            var windowSettingsModule = new commands.old_client.WindowSettingsModule(windowSettings.Unset, windowSettings.ClientResolutionId, 
                Convert.ToString(windowSettings.WindowSettingsString), Convert.ToString(windowSettings.ResizableWindowsString), 
                windowSettings.MinimapScale, Convert.ToString(windowSettings.MainmenuPosition), windowSettings.BarStatus, 
                Convert.ToString(windowSettings.SlotmenuPosition), Convert.ToString(windowSettings.SlotMenuOrder),
                Convert.ToString(windowSettings.SlotmenuPremiumPosition), Convert.ToString(windowSettings.SlotMenuPremiumOrder));

            var gameplaySettings = player.Settings.GetSettings<GameplaySettings>();
            var gameplaySettingsModule = new commands.old_client.GameplaySettingsModule(gameplaySettings.Unset, gameplaySettings.AutoBoost,
                gameplaySettings.AutoRefinement, gameplaySettings.QuickslotStopAttack, gameplaySettings.DoubleclickAttack, 
                gameplaySettings.AutoChangeAmmo, gameplaySettings.AutoStart, gameplaySettings.AutoBuyGreenBootyKeys);

            Packet.Builder.BuildCommand(session.GameClient, Commands.USER_SETTINGS_COMMAND, player.UsingNewClient,
                qualitySettingsModule, displaySettingsModule, audioSettingsModule, windowSettingsModule, gameplaySettingsModule);
        }

        public void CreateKeyBindingsCommand(Player player)
        {
            var session = GameStorageManager.Instance.FindSession(player);
            if (session == null)
            {
                return;
            }

            Packet.Builder.BuildCommand(session.GameClient, Commands.HOTKEYS_COMMAND, player.UsingNewClient);
        }
        
        public void CreateSlotbarSettings(Player player)
        {            
            var session = GameStorageManager.Instance.FindSession(player);
            if (session == null)
            {
                return;
            }
            
            var slotbarSettings = player.Settings.GetSettings<SlotbarSettings>();

            Packet.Builder.BuildCommand(session.GameClient, Commands.SHIP_SETTINGS_COMMAND, player.UsingNewClient, slotbarSettings.QuickbarSlots,
                slotbarSettings.QuickbarSlotsPremium, AmmoConvertManager.GetLootAmmoId(slotbarSettings.SelectedLaserAmmo), AmmoConvertManager.GetLootAmmoId(slotbarSettings.SelectedRocketAmmo), AmmoConvertManager.GetLootAmmoId(slotbarSettings.SelectedHellstormRocketAmmo));
        }

        public void CreateAmmunition(Player player)
        {
            var session = GameStorageManager.Instance.FindSession(player);
            if (session == null)
            {
                return;
            }

            Packet.Builder.BuildCommand(session.GameClient, Commands.AMMUNITION_COUNT_UPDATE_COMMAND, player.UsingNewClient);
        }
    }
}