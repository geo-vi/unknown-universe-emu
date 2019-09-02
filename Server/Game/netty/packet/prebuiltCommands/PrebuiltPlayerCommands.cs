using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Server.Game.managers;
using Server.Game.netty.commands;
using Server.Game.netty.commands.old_client;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.packet.prebuiltCommands
{
    class PrebuiltPlayerCommands : PrebuiltCommandBase
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

        public override void AddCommands()
        {
            Packet.Builder.OldCommands.Add(Commands.SHIP_INITIALIZATION_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 33, out actionParams);
                var visuals = (List<commands.old_client.VisualModifierCommand>) actionParams[32] ??
                              new List<commands.old_client.VisualModifierCommand>();
                await client.Send(
                    commands.old_client.ShipInitializationCommand.write(
                        Convert.ToInt32(actionParams[0]), Convert.ToString(actionParams[1]),
                        Convert.ToInt32(actionParams[2]), Convert.ToInt32(actionParams[3]),
                        Convert.ToInt32(actionParams[4]), Convert.ToInt32(actionParams[5]),
                        Convert.ToInt32(actionParams[6]), Convert.ToInt32(actionParams[7]),
                        Convert.ToInt32(actionParams[8]), Convert.ToInt32(actionParams[9]),
                        Convert.ToInt32(actionParams[10]), Convert.ToInt32(actionParams[11]),
                        Convert.ToInt32(actionParams[12]), Convert.ToInt32(actionParams[13]),
                        Convert.ToInt32(actionParams[14]), Convert.ToInt32(actionParams[15]),
                        Convert.ToInt32(actionParams[16]), Convert.ToInt32(actionParams[17]),
                        Convert.ToInt32(actionParams[18]), Convert.ToInt32(actionParams[19]),
                        Convert.ToBoolean(actionParams[20]), Convert.ToDouble(actionParams[21]),
                        Convert.ToDouble(actionParams[22]),
                        Convert.ToInt32(actionParams[23]), Convert.ToDouble(actionParams[24]),
                        Convert.ToDouble(actionParams[25]), Convert.ToSingle(actionParams[26]),
                        Convert.ToInt32(actionParams[27]), Convert.ToString(actionParams[28]),
                        Convert.ToInt32(actionParams[29]), Convert.ToBoolean(actionParams[30]),
                        Convert.ToBoolean(actionParams[31]), visuals).Bytes);

            });

            Packet.Builder.OldCommands.Add(Commands.USER_SETTINGS_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 5, out actionParams);

                var qualitySettings = actionParams[0] as QualitySettingsModule ?? new QualitySettingsModule();

                var displaySettings = actionParams[1] as DisplaySettingsModule ?? new DisplaySettingsModule();

                var audioSettings = actionParams[2] as AudioSettingsModule ?? new AudioSettingsModule();

                var windowSettings = actionParams[3] as WindowSettingsModule ?? new WindowSettingsModule();

                var gameplaySettings = actionParams[4] as GameplaySettingsModule ?? new GameplaySettingsModule();

                await client.Send(commands.old_client.UserSettingsCommand.write(qualitySettings,
                    displaySettings, audioSettings, windowSettings,
                    gameplaySettings).Bytes);
            });

            Packet.Builder.OldCommands.Add(Commands.HOTKEYS_COMMAND,
                async (client, actionParams) =>
                {
                    await client.Send(
                        new commands.old_client.UserKeyBindingsUpdate(new List<UserKeyBindingsModule>(), false)
                            .write());
                });

            Packet.Builder.OldCommands.Add(Commands.SHIP_SETTINGS_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 5, out actionParams);

                await client.Send(new commands.old_client.ShipSettingsCommand(Convert.ToString(actionParams[0]),
                    Convert.ToString(actionParams[1]), Convert.ToInt32(actionParams[2]),
                    Convert.ToInt32(actionParams[3]), Convert.ToInt32(actionParams[4])).write().Bytes);
            });

            Packet.Builder.OldCommands.Add(Commands.AMMUNITION_COUNT_UPDATE_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 1, out actionParams);
                var ammoList = actionParams[0] as List<AmmunitionCountModule> ?? new List<AmmunitionCountModule>();

                await client.Send(commands.old_client.AmmunitionCountUpdateCommand.write(ammoList).Bytes);
            });
        }

        public void ShipInitializationCommand(Player player)
        {
            if (GetSession(player, out var session))
            {
                Packet.Builder.BuildCommand(session.GameClient, Commands.SHIP_INITIALIZATION_COMMAND, false,
                    player.Id, player.Name, player.Hangar.ShipDesign.Id, player.Speed, player.CurrentShield,
                    player.MaxShield, player.CurrentHealth, player.MaxHealth, player.Cargo.CargoSpaceLeft,
                    player.Cargo.MaxCargoSpace,
                    player.CurrentNanoHull, player.MaxNanoHull, player.Position.X, player.Position.Y,
                    player.Spacemap.Id, player.FactionId, player.Clan.Id, 999999999,999999999,
                    player.GetCurrentConfiguration().ExpansionStage,
                    player.Information.Premium, player.Information.Experience, player.Information.Honor,
                    player.Information.Level.Id, player.Information.Credits,
                    player.Information.Uridium, player.Information.Jackpot, player.RankId, player.Clan.Tag,
                    player.Gates.GetGateRings(), false);
            }
        }

        public void CreateSettingsCommand(Player player)
        {
            if (!GetSession(player, out var session)) return;
            var qualitySettings = player.Settings.GetSettings<QualitySettings>();
            var qualitySettingsModule = new netty.commands.old_client.QualitySettingsModule(qualitySettings.Unset,
                qualitySettings.QualityAttack,
                qualitySettings.QualityBackground, qualitySettings.QualityPresetting,
                qualitySettings.QualityCustomized,
                qualitySettings.QualityPOIzone, qualitySettings.QualityShip, qualitySettings.QualityEngine,
                qualitySettings.QualityExplosion,
                qualitySettings.QualityCollectables, qualitySettings.QualityEffect);

            var displaySettings = player.Settings.GetSettings<DisplaySettings>();
            var displaySettingsModule = new commands.old_client.DisplaySettingsModule(displaySettings.Unset,
                displaySettings.DisplayPlayerName,
                displaySettings.DisplayResource, displaySettings.DisplayBoxes,
                displaySettings.DisplayHitpointBubbles, displaySettings.DisplayChat,
                displaySettings.DisplayDrones, displaySettings.DisplayCargoboxes,
                displaySettings.DisplayPenaltyCargoboxes,
                displaySettings.DisplayWindowBackground, displaySettings.DisplayNotifications,
                displaySettings.PreloadUserShips,
                displaySettings.AlwaysDraggableWindows, displaySettings.ShipHovering,
                displaySettings.ShowSecondQuickslotBar,
                displaySettings.UseAutoQuality);

            var audioSettings = player.Settings.GetSettings<AudioSettings>();
            var audioSettingsModule = new commands.old_client.AudioSettingsModule(audioSettings.Unset,
                audioSettings.Sound, audioSettings.Music);

            var windowSettings = player.Settings.GetSettings<WindowSettings>();
            var windowSettingsModule = new commands.old_client.WindowSettingsModule(windowSettings.Unset,
                windowSettings.ClientResolutionId,
                Convert.ToString(windowSettings.WindowSettingsString),
                Convert.ToString(windowSettings.ResizableWindowsString),
                windowSettings.MinimapScale, Convert.ToString(windowSettings.MainmenuPosition),
                windowSettings.BarStatus,
                Convert.ToString(windowSettings.SlotmenuPosition), Convert.ToString(windowSettings.SlotMenuOrder),
                Convert.ToString(windowSettings.SlotmenuPremiumPosition),
                Convert.ToString(windowSettings.SlotMenuPremiumOrder));

            var gameplaySettings = player.Settings.GetSettings<GameplaySettings>();
            var gameplaySettingsModule = new commands.old_client.GameplaySettingsModule(gameplaySettings.Unset,
                gameplaySettings.AutoBoost,
                gameplaySettings.AutoRefinement, gameplaySettings.QuickslotStopAttack,
                gameplaySettings.DoubleclickAttack,
                gameplaySettings.AutoChangeAmmo, gameplaySettings.AutoStart,
                gameplaySettings.AutoBuyGreenBootyKeys);

            Packet.Builder.BuildCommand(session.GameClient, Commands.USER_SETTINGS_COMMAND, player.UsingNewClient,
                qualitySettingsModule, displaySettingsModule, audioSettingsModule, windowSettingsModule,
                gameplaySettingsModule);
        }

        public void CreateKeyBindingsCommand(Player player)
        {
            if (GetSession(player, out var session))
            {
                Packet.Builder.BuildCommand(session.GameClient, Commands.HOTKEYS_COMMAND, player.UsingNewClient);
            }
        }

        public void CreateSlotbarSettings(Player player)
        {
            if (GetSession(player, out var session))
            {
                var slotbarSettings = player.Settings.GetSettings<SlotbarSettings>();

                Packet.Builder.BuildCommand(session.GameClient, Commands.SHIP_SETTINGS_COMMAND, player.UsingNewClient,
                    slotbarSettings.QuickbarSlots,
                    slotbarSettings.QuickbarSlotsPremium,
                    AmmoConvertManager.GetLootAmmoId(slotbarSettings.SelectedLaserAmmo),
                    AmmoConvertManager.GetLootAmmoId(slotbarSettings.SelectedRocketAmmo),
                    AmmoConvertManager.GetLootAmmoId(slotbarSettings.SelectedHellstormRocketAmmo));
            }
        }

        public void CreateAmmunition(Player player)
        {
            if (GetSession(player, out var session))
            {

                var ammoList = new List<AmmunitionCountModule>();
                foreach (var item in player.Ammunition.Ammo)
                {
                    var ammunitiounCountModule =
                        new AmmunitionCountModule(AmmoConvertManager.ToAmmoType(item.Key), item.Value.Amount);
                    ammoList.Add(ammunitiounCountModule);
                }

                Packet.Builder.BuildCommand(session.GameClient, Commands.AMMUNITION_COUNT_UPDATE_COMMAND,
                    player.UsingNewClient, ammoList);
            }
        }
    }
}