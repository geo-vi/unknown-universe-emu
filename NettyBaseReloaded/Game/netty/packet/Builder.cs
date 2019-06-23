using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using NettyBaseReloaded.Chat.objects.chat.rooms;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.objects.stations;
using NettyBaseReloaded.Game.objects.world.pets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.events;
using NettyBaseReloaded.Game.objects.world.players.extra;
using NettyBaseReloaded.Game.objects.world.players.settings;
using NettyBaseReloaded.Utils;
using Global = NettyBaseReloaded.Main.Global;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Game.objects.world.players.quests;
using NettyBaseReloaded.Main.objects;
using Newtonsoft.Json;
using NettyBaseReloaded.Game.objects.world.players.quests.serializables;

namespace NettyBaseReloaded.Game.netty.packet
{
    class Builder
    {
        #region UserSettingsCommand

        public void UserSettingsCommand(GameSession gameSession)
        {
            var player = gameSession.Player;
            if (player.UsingNewClient)
            {
                var qs = new commands.new_client.QualitySettingsModule(false, 3, 3, 3, false, 3, 3, 3, 3, 3, 3);
                var asm = new commands.new_client.AudioSettingsModule(false, 50, 0, 50, false);
                var ws = new commands.new_client.WindowSettingsModule(8,
                    "23,1|24,1|25,1|27,1|26,1|100,1|34,1|36,1|33,1|35,1|39,1|38,1|37,1|32,1|", false);
                var gm = new commands.new_client.GameplaySettingsModule(false, false, false, false, false, true, false,
                    false, false, false);
                var z9 = new commands.new_client.QuestSettingsModule(false, true, true, false, false, false);
                var ds = new commands.new_client.DisplaySettingsModule(true, true, true, true, true, true, true, true,
                    true, true, true, true, true, true, true, true, true, true, true, 3, 4, 4, 3, 3, 4, 3, 3, true,
                    true, true, true);

                //TODO: Integrate it into the Settings 
                gameSession.Client.Send(commands.new_client.UserSettingsCommand.write(qs, asm, ws, gm, z9, ds).Bytes);
            }
            else
            {
                gameSession.Client.Send(player.Settings.OldClientUserSettingsCommand.write().Bytes);
            }
        }

        #endregion

        #region Slotbars / Windows

        public void SendUserSettings(GameSession gameSession)
        {
            var player = gameSession.Player;
            if (player.UsingNewClient)
                SendWindows(gameSession);
            SendSlotbars(gameSession);
        }

        private void SendWindows(GameSession gameSession)
        {
            var topLeftWindows = new List<Window.New_Client>
            {
                new Window.New_Client("cli", "title_cli", false, Window.New_Client.RED),
                new Window.New_Client("user", "title_user"),
                new Window.New_Client("ship", "title_ship"),
                new Window.New_Client("ship_warp", "ttip_shipWarp_btn"),
                new Window.New_Client("chat", "title_chat"),
                new Window.New_Client("group", "title_group"),
                new Window.New_Client("minimap", "title_map"),
                new Window.New_Client("spacemap", "title_spacemap"),
                new Window.New_Client("quests", "title_quests"),
                new Window.New_Client("refinement", "title_refinement"),
                new Window.New_Client("log", "title_log"),
                new Window.New_Client("pet", "title_pet"),
                new Window.New_Client("contacts", "title_contacts", false),
                new Window.New_Client("booster", "title_booster", false),
                new Window.New_Client("spaceball", "title_spaceball", false),
                new Window.New_Client("invasion", "title_invasion", false),
                new Window.New_Client("ctb", "title_ctb", false),
                new Window.New_Client("tdm", "title_tdm", false),
                new Window.New_Client("ranked_hunt", "title_ranked_hunt", false),
                new Window.New_Client("highscoregate", "title_highscoregate", false),
                new Window.New_Client("scoreevent", "title_scoreevent",
                    gameSession.Player.EventsPraticipating.Any(x => x.Value is ScoreMageddon)),
                new Window.New_Client("infiltration", "title_ifg", false),
                new Window.New_Client("word_puzzle", "title_wordpuzzle", false),
                new Window.New_Client("sectorcontrol", "title_sectorcontrol_ingame_status", false),
                new Window.New_Client("jackpot_status_ui", "title_jackpot_status_ui", false),
                new Window.New_Client("curcubitor", "httip_countdownHalloweenNPCs", false),
                new Window.New_Client("influence", "httip_influence", false),
                new Window.New_Client("traininggrounds", "title_traininggrounds", false),
                new Window.New_Client("spaceplague", "title_spaceplague", false)
            };

            var topRightWindows = new List<Window.New_Client>
            {
                new Window.New_Client("shop", "title_shop", false),
                new Window.New_Client("fullscreen", "ttip_fullscreen_btn"),
                new Window.New_Client("settings", "title_settings"),
                new Window.New_Client("help", "title_help", false),
                new Window.New_Client("logout", "title_logout")
            };

            //Foreach window adds it to a list
            var topLeftButtons = topLeftWindows.Select(window => window.Window).ToList();
            var topRightButtons = topRightWindows.Select(window => window.Window).ToList();

            var slotbars = new List<commands.new_client.commandKn>
            {
                new commands.new_client.commandKn(commands.new_client.commandKn.GAME_FEATURE_BAR, topLeftButtons, "0,0",
                    "0"),
                new commands.new_client.commandKn(commands.new_client.commandKn.GENERIC_FEATURE_BAR, topRightButtons,
                    "100,0", "0")
            };

            gameSession.Client.Send(commands.new_client.WindowsCommand.write(slotbars).Bytes);

        }

        public void SendSlotbars(GameSession gameSession)
        {
            var player = gameSession.Player;
            if (player.UsingNewClient)
            {
                var slotbars = new List<commands.new_client.SlotbarQuickslotModule>
                {
                    new commands.new_client.SlotbarQuickslotModule("standardSlotBar",
                        player.Settings.Slotbar.QuickslotItems, "50,85|0,40",
                        "0", true),
                    new commands.new_client.SlotbarQuickslotModule("premiumSlotBar",
                        player.Settings.Slotbar.PremiumQuickslotItems, "50,85|0,80", "0", true)
                };
                gameSession.Client.Send(
                    commands.new_client.SlotbarsCommand
                        .write(player.Settings.Slotbar.GetCategories(), "50,85", slotbars)
                        .Bytes);

            }
            else
            {
                var ammo = new List<netty.commands.old_client.AmmunitionCountModule>();
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(
                        netty.commands.old_client.AmmunitionTypeModule.X1),
                    player.Information.Ammunitions["ammunition_laser_lcb-10"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(
                        netty.commands.old_client.AmmunitionTypeModule.X2),
                    player.Information.Ammunitions["ammunition_laser_mcb-25"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(
                        netty.commands.old_client.AmmunitionTypeModule.X3),
                    player.Information.Ammunitions["ammunition_laser_mcb-50"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(
                        netty.commands.old_client.AmmunitionTypeModule.X4),
                    player.Information.Ammunitions["ammunition_laser_ucb-100"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .SAB), player.Information.Ammunitions["ammunition_laser_sab-50"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .CBO), player.Information.Ammunitions["ammunition_laser_cbo-100"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .RSB), player.Information.Ammunitions["ammunition_laser_rsb-75"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .JOB100), player.Information.Ammunitions["ammunition_laser_job-100"].Get()));
                gameSession.Client.Send(netty.commands.old_client.AmmunitionCountUpdateCommand.write(ammo).Bytes);
                ammo.Clear();
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .R310), player.Information.Ammunitions["ammunition_rocket_r-310"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .PLT2021), player.Information.Ammunitions["ammunition_rocket_plt-2021"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .PLT2026), player.Information.Ammunitions["ammunition_rocket_plt-2026"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .PLT3030), player.Information.Ammunitions["ammunition_rocket_plt-3030"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .PLASMA), player.Information.Ammunitions["ammunition_specialammo_pld-8"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .WIZARD), player.Information.Ammunitions["ammunition_specialammo_wiz-x"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .DECELERATION), player.Information.Ammunitions["ammunition_specialammo_dcr-250"].Get()));
                gameSession.Client.Send(netty.commands.old_client.AmmunitionCountUpdateCommand.write(ammo).Bytes);
                ammo.Clear();
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .ECO_ROCKET), player.Information.Ammunitions["ammunition_rocketlauncher_eco-10"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .HELLSTORM), player.Information.Ammunitions["ammunition_rocketlauncher_hstrm-01"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .UBER_ROCKET), player.Information.Ammunitions["ammunition_rocketlauncher_ubr-100"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .SAR01), player.Information.Ammunitions["ammunition_rocketlauncher_sar-01"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .SAR02), player.Information.Ammunitions["ammunition_rocketlauncher_sar-02"].Get()));
                gameSession.Client.Send(netty.commands.old_client.AmmunitionCountUpdateCommand.write(ammo).Bytes);
                ammo.Clear();
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .MINE), 100));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .MINE_EMP), 100));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .MINE_SAB), 100));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .MINE_DD), 100));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .MINE_SL), 100));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .SMARTBOMB), 100));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .INSTANT_SHIELD), 100));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(
                    new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule
                        .EMP), 100));
                gameSession.Client.Send(netty.commands.old_client.AmmunitionCountUpdateCommand.write(ammo).Bytes);
                
                Packet.Builder.LegacyModule(gameSession, "0|A|FWX|FWL|100|100|100", true);

                player.Settings.Slotbar.GetCategories();
                gameSession.Client.Send(player.Settings.OldClientShipSettingsCommand.write().Bytes);
            }
        }

        #endregion

        #region ShipInitializationCommand

        public void ShipInitializationCommand(GameSession gameSession)
        {
            var player = gameSession.Player;

            if (gameSession.Player.UsingNewClient)
                gameSession.Client.Send(
                    commands.new_client.ShipInitializationCommand.write(
                        player.Id,
                        player.Name,
                        player.Hangar.ShipDesign.ToStringLoot(),
                        player.Speed,
                        player.CurrentShield,
                        player.MaxShield,
                        player.CurrentHealth,
                        player.MaxHealth,
                        player.Information.Cargo.TotalSpace - player.Information.Cargo.UsedSpace,
                        player.Information.Cargo.TotalSpace,
                        player.CurrentNanoHull,
                        player.MaxNanoHull,
                        player.Position.X,
                        player.Position.Y,
                        player.Spacemap.Id,
                        (int) player.FactionId,
                        player.Clan.Id, //clan
                        player.Equipment.GetCurrentConfig().ExpansionStage, //idk
                        player.Information.Premium.Active,
                        player.Information.Experience.Get(),
                        player.Information.Honor.Get(),
                        player.Information.Level.Id,
                        player.Information.Credits.Get(),
                        player.Information.Uridium.Get(),
                        0, //Jackpot
                        (int) player.RankId,
                        player.Clan.Tag, //clanTag
                        player.Gates.CalculateRings(), // player GG rings
                        true,
                        player.Invisible, //cloaked
                        true,
                        VisualEffect.ToNewModifierCommand(player)
                    ).Bytes);
            else
                gameSession.Client.Send(
                    commands.old_client.ShipInitializationCommand.write(
                        player.Id,
                        player.Name,
                        player.Hangar.ShipDesign.Id,
                        player.Speed,
                        player.CurrentShield,
                        player.MaxShield,
                        player.CurrentHealth,
                        player.MaxHealth,
                        player.Information.Cargo.TotalSpace - player.Information.Cargo.UsedSpace,
                        player.Information.Cargo.TotalSpace,
                        player.CurrentNanoHull,
                        player.MaxNanoHull,
                        player.Position.X,
                        player.Position.Y,
                        player.Spacemap.Id,
                        (int) player.FactionId,
                        player.Clan.Id,
                        0,
                        0,
                        player.Equipment.GetCurrentConfig().ExpansionStage,
                        player.Information.Premium.Active,
                        player.Information.Experience.Get(),
                        player.Information.Honor.Get(),
                        player.Information.Level.Id,
                        player.Information.Credits.Get(),
                        player.Information.Uridium.Get(),
                        0,
                        (int) player.RankId,
                        player.Clan.Tag,
                        player.Gates.CalculateRings(),
                        true,
                        player.Invisible,
                        VisualEffect.ToOldModifierCommand(player)).Bytes);
        }

        #endregion

        #region commandX35

        public void commandX35(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.commandX35.write().Bytes);
            }
        }

        #endregion

        #region LegacyModule

        #endregion

        #region ShipCreateCommand

        public void ShipCreateCommand(GameSession gameSession, Character character)
        {
            if (Properties.Game.DEBUG_ENTITIES)
                Console.WriteLine("ID: {0} Type: {1} Position: {2}", character.Id, character.Hangar.Ship.Id,
                    character.Position);

            byte[] bytes = null;
            if (gameSession.Player.UsingNewClient)
            {
                if (character is Player)
                {
                    var pChar = (Player) character;
                    bytes = commands.new_client.ShipCreateCommand.write(pChar.Id,
                        pChar.Hangar.ShipDesign.ToStringLoot(), pChar.Equipment.GetCurrentConfig().ExpansionStage, pChar.Clan.Tag,
                        pChar.Name, pChar.Position.X,
                        pChar.Position.Y,
                        (int) pChar.FactionId, 0, (int) pChar.RankId, false,
                        new commands.new_client.ClanRelationModule(pChar.Clan.GetRelation(gameSession.Player.Clan)), pChar.Gates.CalculateRings(),
                        false, false, pChar.Invisible, 0, 0, new List<commands.new_client.VisualModifierCommand>(),
                        new commands.new_client.commandK13(commands.new_client.commandK13.DEFAULT)).Bytes;
                }
                else if (character is Pet)
                {
                    PetActivationCommand(gameSession, character as Pet);
                    return;
                }
                else
                {
                    bytes = commands.new_client.ShipCreateCommand.write(character.Id,
                        character.Hangar.ShipDesign.ToStringLoot(), 0, character.Clan.Tag, character.Name,
                        character.Position.X,
                        character.Position.Y,
                        (int) character.FactionId, character.Clan.Id, 0, false,
                        new commands.new_client.ClanRelationModule(character.Clan.GetRelation(gameSession.Player.Clan)),
                        0,
                        false, true, character.Invisible, 0, 0, new List<commands.new_client.VisualModifierCommand>(),
                        new commands.new_client.commandK13(commands.new_client.commandK13.DEFAULT)).Bytes;
                }
            }
            else
            {
                if (character is Player)
                {
                    var pChar = (Player) character;
                    bytes = commands.old_client.ShipCreateCommand.write(pChar.Id,
                            pChar.Hangar.ShipDesign.Id, pChar.Equipment.GetCurrentConfig().ExpansionStage, pChar.Clan.Tag, pChar.Name,
                            pChar.Position.X,
                            pChar.Position.Y,
                            (int) pChar.FactionId, pChar.Clan.Id, (int) pChar.RankId, pChar.HasWarnBox(),
                            new commands.old_client.ClanRelationModule(pChar.Clan.GetRelation(gameSession.Player.Clan)),
                            pChar.Gates.CalculateRings(),
                            false, false, character.Invisible, 0, 0,
                            VisualEffect.ToOldModifierCommand(pChar))
                        .Bytes;
                }
                else if (character is Pet)
                {
                    PetActivationCommand(gameSession, character as Pet);
                    return;
                }
                else if (character is Npc npc)
                {
                    bytes = commands.old_client.ShipCreateCommand.write(character.Id,
                            character.Hangar.ShipDesign.Id, 0, character.Clan.Tag, character.Name, character.Position.X,
                            character.Position.Y,
                            (int) character.FactionId, character.Clan.Id, 0, character.HasWarnBox(),
                            new commands.old_client.ClanRelationModule(0),
                            0,
                            false, true, character.Invisible, npc.GetMotherShipId(), 0,
                            VisualEffect.ToOldModifierCommand(npc))
                        .Bytes;
                }
            }

            gameSession.Client.Send(bytes);
        }

        #endregion

        #region ShipRemoveCommand

        public void ShipRemoveCommand(GameSession gameSession, Character character)
        {
            if (Properties.Game.DEBUG_ENTITIES)
                Console.WriteLine("ID: {0} Type: {1} Position: {2}", character.Id, character.Hangar.Ship.Id,
                    character.Position);

            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.ShipRemoveCommand.write(character.Id).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.ShipRemoveCommand.write(character.Id).Bytes);
            }
        }

        #endregion

        #region MoveCommand

        public void MoveCommand(GameSession gameSession, Character character, int time)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(
                    commands.new_client.MoveCommand.write(character.Id, character.Destination.X,
                        character.Destination.Y,
                        time).Bytes);
            }
            else
            {
                gameSession.Client.Send(
                    commands.old_client.MoveCommand.write(character.Id, character.Destination.X,
                        character.Destination.Y,
                        time).Bytes);
            }
        }

        #endregion

        #region DronesCommand

        public void DronesCommand(GameSession gameSession, Character character)
        {
            if (character.Hangar.Drones.Count <= 0) return;
            var command = "0|n|d|" + character.Id + "|" + (int) character.Formation;

            foreach (var d in character.Hangar.Drones.Values)
            {
                command += "|" + (int) d.DroneType + "|" + d.Level.Id + "|" + d.GetDroneDesign();
            }

            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.LegacyModule.write(command).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.LegacyModule.write(command).Bytes);
            }
        }

        #endregion

        #region JumpgateCreateCommand

        public void JumpgateCreateCommand(GameSession gameSession, Jumpgate portal)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.JumpgateCreateCommand.write(portal.Id, (int) portal.Faction,
                    (int)portal.Gfx, portal.Position.X, portal.Position.Y,
                    portal.GetVisibility(gameSession.Player), portal.Working, new List<int>()).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.LegacyModule.write(portal.ToPacket(gameSession.Player)).Bytes);
            }
        }

        #endregion

        #region JumpgateRemoveCommand

        public void JumpgateRemoveCommand(GameSession gameSession, Jumpgate portal)
        {
            LegacyModule(gameSession, "0|n|p|REM|" + portal.Id, true);
        }

        #endregion

        #region ActivatePortalCommand

        public void ActivatePortalCommand(GameSession gameSession, Jumpgate portal)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.ActivatePortalCommand
                    .write(gameSession.Player.Spacemap.Id, portal.Id).Bytes);
            }
            else
            {
                LegacyModule(gameSession,
                    $"0|{ServerCommands.PLAY_PORTAL_ANIMATION}|{portal.DestinationMapId}|{portal.Id}", true);
            }
        }

        #endregion

        #region MapAssetActionAvailableCommand

        public void MapAssetActionAvailableCommand(GameSession gameSession, Object _object, bool toggled,
            bool activatable)
        {
            if (gameSession.Player.UsingNewClient)
            {
                short state = commands.new_client.MapAssetActionAvailableCommand.OFF;
                if (toggled) state = commands.new_client.MapAssetActionAvailableCommand.ON;

                gameSession.Client.Send(commands.new_client.MapAssetActionAvailableCommand.write(_object.Id, state,
                    activatable,
                    new commands.new_client.ClientUITooltip(new List<commands.new_client.ClientUITooltipTextFormat>()),
                    new commands.new_client.commandu1C()).Bytes);
            }
        }

        #endregion

        #region LegacyModule

        public void LegacyModule(GameSession gameSession, string message, bool toOldClientOnly = false)
        {
            try
            {
                if (gameSession.Player.UsingNewClient)
                {
                    if (toOldClientOnly) return;
                    gameSession.Client.Send(commands.new_client.LegacyModule.write(message).Bytes);
                }
                else
                {
                    gameSession.Client.Send(commands.old_client.LegacyModule.write(message).Bytes);
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region ShipSelectionCommand

        public void ShipSelectionCommand(GameSession gameSession, Character character)
        {
            if (gameSession.Player.UsingNewClient)
            {
                if (character == null)
                    gameSession.Client.Send(commands.new_client.ShipSelectionCommand
                        .write(0, 0, 0, 0, 0, 0, 0, 0, false).Bytes);
                else
                    gameSession.Client.Send(commands.new_client.ShipSelectionCommand.write(character.Id,
                        character.Hangar.ShipDesign.Id, character.CurrentShield, character.MaxShield,
                        character.CurrentHealth, character.MaxHealth, character.CurrentNanoHull, character.MaxNanoHull,
                        false).Bytes);
            }
            else
            {
                if (character == null)
                {
                    ShipDeselectionCommand(gameSession);
                    //gameSession.Client.Send(commands.old_client.ShipSelectionCommand
                    //    .write(0, 0, 0, 0, 0, 0, 0, 0, false).Bytes);
                }
                else
                    gameSession.Client.Send(commands.old_client.ShipSelectionCommand.write(character.Id,
                        character.Hangar.ShipDesign.Id, character.CurrentShield, character.MaxShield,
                        character.CurrentHealth, character.MaxHealth, character.CurrentNanoHull, character.MaxNanoHull,
                        false).Bytes);
            }
        }

        #endregion

        #region ShipDeselectionCommand

        public void ShipDeselectionCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.ShipDeselectionCommand.write().Bytes);
            }
        }
        #endregion


        #region AssetCreateCommand

        public void AssetCreateCommand(GameSession gameSession, Asset asset)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.AssetCreateCommand.write(
                    new commands.new_client.AssetTypeModule((short) asset.Type), asset.Name, (int) asset.Faction,
                    asset.Clan.Tag, asset.Id, asset.DesignId,
                    asset.ExpansionStage, asset.Position.X, asset.Position.Y, asset.Clan.Id, asset.Invisible,
                    asset.VisibleOnWarnRadar, asset.DetectedByWarnRadar, true,
                    new commands.new_client.ClanRelationModule(asset.Clan.GetRelation(gameSession.Player.Clan)),
                    new List<commands.new_client.VisualModifierCommand>()).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AssetCreateCommand.write(
                    new commands.old_client.AssetTypeModule((short) asset.Type), asset.Name, (int) asset.Faction,
                    asset.Clan.Tag, asset.Id, asset.DesignId,
                    asset.ExpansionStage, asset.Position.X, asset.Position.Y, asset.Clan.Id, asset.Invisible,
                    asset.VisibleOnWarnRadar, asset.DetectedByWarnRadar,
                    new commands.old_client.ClanRelationModule(asset.Clan.GetRelation(gameSession.Player.Clan)),
                    new List<commands.old_client.VisualModifierCommand>()).Bytes);
            }
        }

        #endregion

        #region StationCreateCommand

        public void StationCreateCommand(GameSession gameSession, Station station)
        {
            if (gameSession.Player.UsingNewClient)
            {
                if (station is PirateStation)
                {
                    gameSession.Client.Send(commands.new_client.commandY2c.write(station.Id, 6, 1500,
                        station.Position.X, station.Position.Y));
                }
                else if (station is HealthStation)
                {
                    gameSession.Client.Send(commands.new_client.commandY2c.write(station.Id, 4, 1500,
                        station.Position.X, station.Position.Y));
                }
                else if (station is ReadyRelayStation)
                {
                    gameSession.Client.Send(commands.new_client.commandY2c.write(station.Id, 5, 1500,
                        station.Position.X, station.Position.Y));
                }
                else
                {
                    foreach (var module in station.Modules)
                    {
                        AssetCreateCommand(gameSession,
                            new Asset(module.Id, "", module.Type, station.Faction, Global.StorageManager.Clans[0], 0, 0,
                                module.Position, station.Spacemap, false, false, false));
                    }
                }
            }
            else
            {
                LegacyModule(gameSession, station.GetString());
            }
        }

        #endregion

        #region AttackLaserRunCommand

        public void AttackLaserRunCommand(GameSession gameSession, int attackerId, int targetId, int laserColor,
            bool isDiminishedBySkillShield, bool skilledLaser)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.AttackLaserRunCommand.write(attackerId, targetId,
                    laserColor, isDiminishedBySkillShield, skilledLaser).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AttackLaserRunCommand.write(attackerId, targetId,
                    laserColor, isDiminishedBySkillShield, skilledLaser).Bytes);
            }
        }

        #endregion

        #region AttackHitCommand

        public void AttackHitCommand(GameSession gameSession, int attackerId, IAttackable target, int damage,
            short effect)
        {
            if (gameSession.Player.UsingNewClient)
            {
                if (damage == 0)
                    gameSession.Client.Send(commands.new_client.AttackMissedCommand
                        .write(new commands.new_client.AttackTypeModule(effect), target.Id, 0).Bytes);
                else
                    gameSession.Client.Send(commands.new_client.AttackHitCommand.write(
                            new commands.new_client.AttackTypeModule(effect), attackerId,
                            target.Id, target.CurrentHealth, target.CurrentShield, target.CurrentNanoHull, damage, true)
                        .Bytes);
            }
            else
            {
                if (damage == 0)
                    gameSession.Client.Send(commands.old_client.AttackMissedCommand
                        .write(new commands.old_client.AttackTypeModule(effect), target.Id, 0).Bytes);
                else
                    gameSession.Client.Send(commands.old_client.AttackHitCommand.write(
                            new commands.old_client.AttackTypeModule(effect), attackerId,
                            target.Id, target.CurrentHealth, target.CurrentShield, target.CurrentNanoHull, damage, true)
                        .Bytes);
            }

        }

        #endregion

        #region HitpointInfoCommand

        public void HitpointInfoCommand(GameSession gameSession, int hitpoints, int hitpointsMax, int nanoHull,
            int nanoHullMax)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.HitpointInfoCommand
                    .write(hitpoints, hitpointsMax, nanoHull, nanoHullMax).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.HitpointInfoCommand
                    .write(hitpoints, hitpointsMax, nanoHull, nanoHullMax).Bytes);
            }
        }

        #endregion

        #region AttributeShieldUpdateCommand

        public void AttributeShieldUpdateCommand(GameSession gameSession, int shieldNow, int shieldMax)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.AttributeShieldUpdateCommand.write(shieldNow, shieldMax)
                    .Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AttributeShieldUpdateCommand.write(shieldNow, shieldMax)
                    .Bytes);
            }
        }

        #endregion

        #region AttributeShipSpeedUpdateCommand

        public void AttributeShipSpeedUpdateCommand(GameSession gameSession, int newSpeed)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.AttributeShipSpeedUpdateCommand
                    .write(newSpeed, gameSession.Player.Speed)
                    .Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AttributeShipSpeedUpdateCommand.write(newSpeed).Bytes);
            }
        }

        #endregion

        #region PetInitializationCommand

        public void PetInitializationCommand(GameSession gameSession, Pet pet)
        {
            bool hasPet = pet != null;
            bool petIsAlive = false;
            bool hasFuel = false;
            if (hasPet)
            {
                hasFuel = pet.HasFuel();
                petIsAlive = pet.EntityState == EntityStates.ALIVE;
                if (hasFuel && petIsAlive && pet.Controller != null && pet.Controller.Active)
                {
                    Packet.Builder.PetStatusCommand(gameSession, pet);
                    return;
                }
            }

            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.PetInitializationCommand.write(hasPet, hasFuel, petIsAlive)
                    .Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetInitializationCommand.write(hasPet, hasFuel, petIsAlive)
                    .Bytes);
            }
        }

        #endregion

        #region PetActivationCommand

        public void PetActivationCommand(GameSession gameSession, Pet pet)
        {
            if (pet.GetOwner() == gameSession.Player)
            {
                PetHeroActivationCommand(gameSession, pet);
            }
            else
            {
                if (gameSession.Player.UsingNewClient)
                {
                    gameSession.Client.Send(commands.new_client.PetActivationCommand.write(pet.GetOwner().Id, pet.Id,
                        (short)pet.Hangar.Ship.Id, pet.ExpansionStage, pet.Name,
                        (short) pet.FactionId, pet.Clan.Id, (short) pet.Level.Id, pet.Clan.Tag,
                        new commands.new_client.ClanRelationModule(pet.Clan.GetRelation(gameSession.Player.Clan)),
                        pet.Position.X, pet.Position.Y, pet.Speed, false, !pet.GetOwner().Invisible,
                        new commands.new_client.commandK13(0)).Bytes);
                }
                else
                {
                    gameSession.Client.Send(commands.old_client.PetActivationCommand.write(pet.GetOwner().Id,
                            pet.Id,
                            (short) pet.Hangar.Ship.Id, pet.ExpansionStage, pet.Name,
                            (short) pet.FactionId, pet.Clan.Id, (short) pet.Level.Id, pet.Clan.Tag,
                            new commands.old_client.ClanRelationModule(pet.Clan.GetRelation(gameSession.Player.Clan)),
                            pet.Position.X, pet.Position.Y, pet.Speed, false, !pet.GetOwner().Invisible).Bytes);
                }
            }
        }

        #endregion

        #region PetHeroActivationCommand

        public void PetHeroActivationCommand(GameSession gameSession, Pet pet)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetHeroActivationCommand.write(pet.GetOwner().Id,
                    pet.Id,
                    (short) pet.Hangar.Ship.Id, pet.ExpansionStage, pet.Name, (short) pet.FactionId, pet.Clan.Id,
                    (short) pet.Level.Id, pet.Clan.Tag,
                    pet.Position.X, pet.Position.Y, pet.Speed).Bytes);
            }
        }

        #endregion

        #region PetStatusCommand

        public void PetStatusCommand(GameSession gameSession, Pet pet)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.PetStatusCommand.write(pet.Id, pet.Level.Id, pet.Experience,
                    pet.Level.Experience, pet.CurrentHealth,
                    pet.MaxHealth, pet.CurrentShield, pet.MaxShield, pet.Fuel, 50000, pet.Speed, pet.Name).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetStatusCommand.write(pet.Id, pet.Level.Id, pet.Experience,
                    pet.Level.Experience, pet.CurrentHealth,
                    pet.MaxHealth, pet.CurrentShield, pet.MaxShield, pet.Fuel, 50000, pet.Speed, pet.Name).Bytes);
            }
        }

        #endregion

        #region CreateBoxCommand

        public void CreateBoxCommand(GameSession gameSession, Collectable collectable)
        {
            if (collectable.Disposed) return;
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.CreateBoxCommand.write(collectable.Type.ToString(),
                        new commands.new_client.BoxModule(collectable.Hash, collectable.Position.X,
                            collectable.Position.Y))
                    .Bytes);
            }
            else
            {
                LegacyModule(gameSession,
                    "0|c|" + collectable.Hash + "|" + collectable.GetTypeId(gameSession.Player) + "|" +
                    collectable.Position.ToPacket());
            }
        }

        #endregion

        #region BeaconCommand

        public void BeaconCommand(GameSession gameSession)
        {
            var player = gameSession.Player;
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.BeaconCommand.write(0, 0, 0, 0,
                    player.State.InDemiZone, player.Controller.Repairing, player.Controller.Repairing,
                    player.Equipment.GetRobot(), player.State.InRadiationArea).Bytes);
            }
            else
            {
                LegacyModule(gameSession, "0|" + commands.ServerCommands.BEACON + "|" + player.Position.X + "|" +
                                          player.Position.Y + "|" + Convert.ToInt32(player.State.InDemiZone) + "|0|"
                                          + Convert.ToInt32(player.State.InTradeArea) + "|"
                                          + Convert.ToInt32(player.State.InRadiationArea) + "|" +
                                          Convert.ToInt32(player.State.InPortalArea) + "|0");
                //LegacyModule(gameSession, "0|A" +
                //                          "|RS|" +
                //                          "S|" + player.Controller.Repairing);
                //EquipReadyCommand(gameSession, player.State.InEquipmentArea);
            }
        }

        #endregion

        #region HotkeyCommand

        public void HotkeysCommand(GameSession gameSession)
        {
            var newClient = gameSession.Player.UsingNewClient;
            //TODO create this array somewhere reading from json
            var hotkeys = new List<Hotkey>
            {
                new Hotkey(Hotkey.ACTIVATE_LASER, (int) Keys.ControlKey, newClient),
                new Hotkey(Hotkey.CHANGE_CONFIG, (int) Keys.C, newClient),
                new Hotkey(Hotkey.JUMP, (int) Keys.J, newClient),
                new Hotkey(Hotkey.LAUNCH_ROCKET, (int) Keys.Space, newClient),
                new Hotkey(Hotkey.PERFORMANCE_MONITORING, (int) Keys.F, newClient),
                new Hotkey(Hotkey.PET_ACTIVATE, (int) Keys.E, newClient),
                new Hotkey(Hotkey.PET_GUARD_MODE, (int) Keys.R, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D1, 0, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D2, 1, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D3, 2, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D4, 3, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D5, 4, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D6, 5, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D7, 6, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D8, 7, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D9, 8, newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D0, 9, newClient),
                new Hotkey(Hotkey.TOGGLE_WINDOWS, (int) Keys.H, newClient),
                new Hotkey(Hotkey.LOGOUT, (int) Keys.L, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F1, 0, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F2, 1, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F3, 2, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F4, 3, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F5, 4, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F6, 5, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F7, 6, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F8, 7, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F9, 8, newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F10, 9, newClient),
                new Hotkey(Hotkey.ZOOM_IN, (int) Keys.Oemplus, newClient),
                new Hotkey(Hotkey.ZOOM_OUT, (int) Keys.OemMinus, newClient),
            };

            var keys = hotkeys.Select(hotkey => hotkey.Object).ToList();

            if (newClient)
                gameSession.Client.Send(commands.new_client.UserKeyBindingsUpdate.write(keys, false));
            else gameSession.Client.Send(gameSession.Player.Settings.OldClientKeyBindingsCommand.write());
        }

        #endregion

        #region MapAddPOICommand

        public void MapAddPOICommand(GameSession gameSession, POI poi)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.MapAddPOICommand.write(poi.Id,
                    new commands.new_client.POITypeModule((short) poi.Type), poi.TypeSpecification,
                    new commands.new_client.POIDesignModule((short) poi.Design), (short) poi.Shape,
                    poi.ShapeCordsToInts(), poi.Inverted, poi.Active));
            }
            else
            {
                gameSession.Client.Send(commands.old_client.MapAddPOICommand.write(poi.Id,
                    new commands.old_client.POITypeModule((short) poi.Type), poi.TypeSpecification,
                    new commands.old_client.POIDesignModule((short) poi.Design), (short) poi.Shape,
                    poi.ShapeCordsToInts(), poi.Inverted, poi.Active));
                gameSession.Client.Send(commands.old_client.POIReadyCommand.write());
            }
        }

        #endregion

        #region VideoWindowCreateCommand

        public void VideoWindowCreateCommand(GameSession gameSession, int windowID, string windowAlign,
            bool showButtons, List<string> languageKeys,
            int videoID, short videoType)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.VideoWindowCreateCommand.write(windowID, windowAlign,
                    showButtons, languageKeys, videoID, videoType));
            }
            else
            {
                gameSession.Client.Send(commands.old_client.VideoWindowCreateCommand.write(windowID, windowAlign,
                    showButtons, languageKeys, videoID, videoType));
            }
        }

        #endregion

        #region ShipWarpWindowCreateCommand

        public void ShipWarpWindowCreateCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                var ships = new List<commands.new_client.ShipWarpModule>();

                foreach (var hangar in gameSession.Player.Equipment.Hangars.Where(x => !x.Value.Active))
                {
                    ships.Add(new commands.new_client.ShipWarpModule(hangar.Value.Ship.Id,
                        hangar.Value.Ship.ToStringLoot(), hangar.Value.Ship.Name, 0, 0, hangar.Key,
                        hangar.Value.Ship.Name));
                }

                gameSession.Client.Send(commands.new_client.ShipWarpWindowCommand.write(0,
                        (int) gameSession.Player.Information.Uridium.Get(), gameSession.Player.State.InEquipmentArea,
                        ships)
                    .Bytes);
            }
            else
            {
                var ships = new List<commands.old_client.ShipWarpModule>();

                foreach (var hangar in gameSession.Player.Equipment.Hangars)
                {
                    ships.Add(new commands.old_client.ShipWarpModule(hangar.Value.Ship.Id, hangar.Value.Ship.Id,
                        hangar.Value.Ship.Name, 0, 0, hangar.Key, hangar.Value.Ship.Name));
                }

                gameSession.Client.Send(commands.old_client.ShipWarpWindowCommand.write(0,
                        (int) gameSession.Player.Information.Uridium.Get(), gameSession.Player.State.InEquipmentArea,
                        ships)
                    .Bytes);
            }
        }

        #endregion

        #region MapAssetActionAvailableCommand

        public void MapAssetActionAvailableCommand(GameSession gameSession, Object obj, bool clickable)
        {
            var player = gameSession.Player;
            if (player.UsingNewClient)
            {
                short toShort = (short) (clickable ? 0 : 1);
                gameSession.Client.Send(commands.new_client.MapAssetActionAvailableCommand.write(obj.Id, toShort, true,
                    new commands.new_client.ClientUITooltip(new List<commands.new_client.ClientUITooltipTextFormat>()),
                    new commands.new_client.commandu1C()).Bytes);
            }
        }

        #endregion

        #region MineCreateCommand

        public void MineCreateCommand(GameSession gameSession, string hash, int mineType, Vector pos, bool pulse, bool shockWave = false)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.MineCreateCommand
                    .write(hash, pulse, false, mineType, pos.Y, pos.X).Bytes);
            }
            else
            {
                LegacyModule(gameSession,
                    $"0|L|{hash}|{mineType}|{pos.X}|{pos.Y}|{Convert.ToInt32(pulse)}|{Convert.ToInt32(shockWave)}");
            }
        }

        #endregion

        #region BattleStationNoClanUiInitializationCommand

        public void BattleStationNoClanUiInitializationCommand(GameSession gameSession, Asteroid asteroid)
        {
            if (!gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.old_client.BattleStationNoClanUiInitializationCommand
                    .write(asteroid.Id).Bytes);
            }
        }

        #endregion

        #region DisposeBoxCommand

        public void DisposeBoxCommand(GameSession gameSession, Collectable collectable)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.DisposeBoxCommand.write(collectable.Hash, true).Bytes);
            }
            else
            {
                LegacyModule(gameSession, "0|2|" + collectable.Hash, true);
            }
        }

        public void DisposeBoxCommand(GameSession gameSession, Ore collectable)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.DisposeBoxCommand.write(collectable.Hash, true).Bytes);
            }
            else
            {
                LegacyModule(gameSession, "0|2|" + collectable.Hash, true);
            }
        }

        #endregion

        #region AddOreCommand

        public void AddOreCommand(GameSession gameSession, Ore ore)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.AddOreCommand
                    .write(new commands.new_client.BoxModule(ore.Hash, ore.Position.X, ore.Position.Y),
                        new commands.new_client.OreTypeModule((short) ore.Type)).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AddOreCommand.write(ore.Hash,
                    new commands.old_client.OreTypeModule((short) ore.Type), ore.Position.X, ore.Position.Y).Bytes);
            }
        }

        #endregion

        #region PetGearAddCommand

        public void PetGearAddCommand(GameSession gameSession, PetGear gear)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Update PetGearAddCommand");
                //throw new NotImplementedException();
            }
            else
            {
                gameSession.Client.Send(netty.commands.old_client.PetGearAddCommand
                    .write(new commands.old_client.PetGearTypeModule((short) gear.TypeOfGear), gear.Level, gear.Amount,
                        gear.Enabled).Bytes);
            }
        }

        #endregion

        #region PetGearSelectCommand

        public void PetGearSelectCommand(GameSession gameSession, PetGear gear)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Update PetGearSelectCommand");
                //throw new NotImplementedException();
            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetGearSelectCommand
                    .write(new commands.old_client.PetGearTypeModule((short) gear.TypeOfGear), gear.OptionalParams).Bytes);
            }
        }

        #endregion

        #region MapChangeCommand

        public void MapChangeCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
                Console.WriteLine("TODO: Find MapChangeCommand for new client");
            else
                LegacyModule(gameSession, "0|z");
        }

        #endregion

        #region PetDeactivationCommand

        public void PetDeactivationCommand(GameSession gameSession, Pet pet)
        {
            if (gameSession.Player.UsingNewClient)
                Console.WriteLine("TODO: PetDeactivation for new client");
            //throw new NotImplementedException();
            else
                gameSession.Client.Send(commands.old_client.PetDeactivationCommand.write(pet.Id).Bytes);
        }

        #endregion

        #region LevelUpCommand

        public void LevelUpCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find levelup for new client");
                //throw new NotImplementedException();
            }
            else
            {
                gameSession.Client.Send(commands.old_client.LevelUpCommand
                    .write(gameSession.Player.Id, gameSession.Player.Information.Level.Id).Bytes);
            }
        }

        #endregion

        #region AmmunitionCountUpdateCommand

        public void AmmunitionCountUpdateCommand(GameSession gameSession, string lootId, int amount)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Player.Settings.Slotbar._items[lootId].CounterValue = amount;
                //Console.WriteLine("TODO: Find ammo count update command for new client");
                //throw new NotImplementedException();
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AmmunitionCountUpdateCommand
                    .write(new List<commands.old_client.AmmunitionCountModule>()
                    {
                        new commands.old_client.AmmunitionCountModule(AmmoConverter.ToAmmoType(lootId), amount)
                    }).Bytes);
            }
        }

        #endregion

        #region HellstormStatusCommand

        public void HellstormStatusCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
            }
            else
            {
                var player = gameSession.Player;

                if (player.RocketLauncher != null)
                {
                    gameSession.Client.Send(commands.old_client.HellstormStatusCommand.write(player.RocketLauncher.GetLaunchersInt(), AmmoConverter
                        .ToAmmoType(gameSession.Player.RocketLauncher.LoadLootId), player.RocketLauncher.LoadedRockets).Bytes);
                }
            }
        }

        #endregion

        #region AttributeBoosterUpdateCommand

        public void AttributeBoosterUpdateCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find booster attribute for new client");
            }
            else
            {
                var boostList = new List<commands.old_client.BoosterUpdateModule>();
                if (gameSession.Player.BoostedDamage > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short) Booster.Types.DAMAGE),
                        Convert.ToSingle(gameSession.Player.BoostedDamage * 100),
                        new List<commands.old_client.BoosterTypeModule>()));
                }

                if (gameSession.Player.BoostedHealth > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.MAXHP),
                        Convert.ToSingle(gameSession.Player.BoostedHealth * 100),
                        new List<commands.old_client.BoosterTypeModule>()));

                }

                if (gameSession.Player.BoostedShield > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.SHIELD),
                        Convert.ToSingle(gameSession.Player.BoostedShield * 100),
                        new List<commands.old_client.BoosterTypeModule>()));

                }

                if (gameSession.Player.BoostedQuestReward > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.QUESTREWARD),
                        Convert.ToSingle(gameSession.Player.BoostedQuestReward * 100),
                        new List<commands.old_client.BoosterTypeModule>()));
                }

                if (gameSession.Player.BoostedBoxRewards > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.BONUSBOXES),
                        Convert.ToSingle(gameSession.Player.BoostedBoxRewards * 100),
                        new List<commands.old_client.BoosterTypeModule>()));
                }

                if (gameSession.Player.BoostedRepairSpeed > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.REPAIR),
                        Convert.ToSingle(gameSession.Player.BoostedRepairSpeed * 100),
                        new List<commands.old_client.BoosterTypeModule>()));

                }

                if (gameSession.Player.BoostedHonorReward > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.HONOUR),
                        Convert.ToSingle(gameSession.Player.BoostedHonorReward * 100),
                        new List<commands.old_client.BoosterTypeModule>()));

                }

                if (gameSession.Player.BoostedExpReward > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.EP),
                        Convert.ToSingle(gameSession.Player.BoostedExpReward * 100),
                        new List<commands.old_client.BoosterTypeModule>()));

                }

                if (gameSession.Player.BoostedResources > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.RESOURCE),
                        Convert.ToSingle(gameSession.Player.BoostedResources * 100),
                        new List<commands.old_client.BoosterTypeModule>()));

                }

                if (gameSession.Player.BoostedShieldRegen > 0)
                {
                    boostList.Add(new commands.old_client.BoosterUpdateModule(
                        new commands.old_client.BoostedAttributeTypeModule((short)Booster.Types.SHIELDRECHARGE),
                        Convert.ToSingle(gameSession.Player.BoostedShieldRegen * 100),
                        new List<commands.old_client.BoosterTypeModule>()));

                }

                var pBoosterList = gameSession.Player.Boosters.Values.Concat(gameSession.Player.InheritedBoosters.Values);
                foreach (var booster in pBoosterList)
                {
                    boostList.Find(x => x.attributeType.typeValue == (short)booster.Type).boosterTypes
                        .Add(new commands.old_client.BoosterTypeModule((short)booster.BoosterType));
                }

                gameSession.Client.Send(commands.old_client.AttributeBoosterUpdateCommand.write(boostList).Bytes);
            }
        }

        #endregion

        #region UIButtonShowFlashCommand

        public void UIButtonShowFlashCommand(GameSession gameSession, int buttonId, bool arrow, int flashingTimes = -1)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find UIFlash");
            }
            else
            {
                Packet.Builder.LegacyModule(gameSession,
                    $"0|UI|B|SF|{flashingTimes}|{Convert.ToInt32(arrow)}|{buttonId}");
            }
        }

        #endregion

        #region UIButtonHideFlashCommand

        public void UIButtonHideFlashCommand(GameSession gameSession, int buttonId, bool arrow, int flashingTimes = -1)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find HideFlash for new client");
            }
            else
            {
                Packet.Builder.LegacyModule(gameSession, $"0|UI|B|HF|-1|-1|{buttonId}");
            }
        }

        #endregion

        #region LogoutCommand


        public void LogoutCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find logout command");
            }
            else
            {
                LegacyModule(gameSession, "0|l");
            }
        }

        #endregion

        #region GroupInviteCommand

        public void GroupInviteCommand(GameSession gameSession, GameSession invited)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find invite group cmd for new client (1)");
            }
            else
            {
                //LegacyModule(gameSession, "0|ps|inv|new|" + invited.Player.Id + "|" + Encode.Base64(invited.Player.Name) + "|" + invited.Player.Hangar.Ship.Id + "|" + gameSession.Player.Id + "|" + Encode.Base64(gameSession.Player.Name) + "|" + gameSession.Player.Hangar.Ship.Id);
                LegacyModule(gameSession,
                    "0|ps|inv|new|" + gameSession.Player.Id + "|" + Encode.Base64(gameSession.Player.Name) + "|" +
                    gameSession.Player.Hangar.Ship.Id + "|" + invited.Player.Id + "|" +
                    Encode.Base64(invited.Player.Name) + "|" + invited.Player.Hangar.Ship.Id);
            }

            if (invited.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find invite group cmd for new client (2)");
            }
            else
            {
                LegacyModule(invited,
                    "0|ps|inv|new|" + gameSession.Player.Id + "|" + Encode.Base64(gameSession.Player.Name) + "|" +
                    gameSession.Player.Hangar.Ship.Id + "|" + invited.Player.Id + "|" +
                    Encode.Base64(invited.Player.Name) + "|" + invited.Player.Hangar.Ship.Id);
            }
        }

        #endregion

        #region GroupDeleteInvitationCommand

        public void GroupDeleteInvitationCommand(GameSession gameSession, Player inviter)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find group delete invitation for new client");
            }
            else
            {
                LegacyModule(gameSession, $"0|ps|inv|del|none|{inviter.Id}");
                LegacyModule(gameSession, $"0|ps|inv|del|none|{gameSession.Player.Id}|{inviter.Id}");
            }
        }

        #endregion

        #region GroupInitializationCommand

        public void GroupInitializationCommand(GameSession gameSession)
        {
            var player = gameSession.Player;
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find groupinit for new client");
            }
            else
            {
                if (gameSession.Player.Group != null)
                {
                    StringBuilder builder =
                        new StringBuilder(
                            $"0|ps|init|grp|{player.Group.Id}|{player.Group.Members.Count + 1}|{Group.DEFAULT_MAX_GROUP_SIZE}|{Convert.ToInt32(player.Group.LeaderInvitesOnly)}|{player.Group.LootMode}");
                    var groupLeader = player.Group.Leader;

                    builder.Append(
                        $"|{Encode.Base64(groupLeader.Name)}|{groupLeader.Id}|{groupLeader.CurrentHealth}|{groupLeader.MaxHealth}|{groupLeader.CurrentNanoHull}|{groupLeader.MaxNanoHull}|{groupLeader.CurrentShield}|{groupLeader.MaxShield}|{groupLeader.Spacemap.Id}|{groupLeader.Position.X}|{groupLeader.Position.Y}|{groupLeader.Information.Level.Id}|0|{Convert.ToInt32(groupLeader.Invisible)}|{Convert.ToInt32(groupLeader.Controller.Attack.Attacking)}|{Convert.ToInt32(groupLeader.FactionId)}|{Convert.ToInt32((groupLeader.Selected as Player)?.Hangar.Ship.Id)}|{groupLeader.Clan.Tag}|{groupLeader.Hangar.Ship.Id}|{Convert.ToInt32(World.StorageManager.GetGameSession(groupLeader.Id) == null)}|");

                    foreach (var grpMember in player.Group.Members)
                    {
                        var groupMember = grpMember.Value;
                        if (groupMember.Id == player.Group.Leader.Id) continue;

                        builder.Append(
                            $"|{Encode.Base64(groupMember.Name)}|{groupMember.Id}|{groupMember.CurrentHealth}|{groupMember.MaxHealth}|{groupMember.CurrentNanoHull}|{groupMember.MaxNanoHull}|{groupMember.CurrentShield}|{groupMember.MaxShield}|{groupMember.Spacemap.Id}|{groupMember.Position.X}|{groupMember.Position.Y}|{groupMember.Information.Level.Id}|0|{Convert.ToInt32(groupMember.Invisible)}|{Convert.ToInt32(groupMember.Controller.Attack.Attacking)}|{Convert.ToInt32(groupMember.FactionId)}|{Convert.ToInt32((groupLeader.Selected as Player)?.Hangar.Ship.Id)}|{groupMember.Clan.Tag}|{groupMember.Hangar.Ship.Id}|{Convert.ToInt32(World.StorageManager.GetGameSession(groupMember.Id) == null)}|");
                    }

                    LegacyModule(gameSession, builder.ToString());
                }
                else
                {
                    LegacyModule(gameSession, "0|ps|nüscht");
                }
            }
        }

        #endregion

        #region GroupUpdateCommand

        public void GroupUpdateCommand(GameSession gameSession, Player updatedPlayer, XElement xml)
        {
            var player = gameSession.Player;
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find group update for new client");
            }
            else
            {
                LegacyModule(gameSession, "0|ps|upd|" + updatedPlayer.Id + "|" + xml.ToString(SaveOptions.None));
            }
        }

        #endregion

        #region HeroMoveCommand

        public void HeroMoveCommand(GameSession gameSession, Vector destination)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO Find hero move for new client");
            }
            else
            {
                gameSession.Client.Send(commands.old_client.HeroMoveCommand.write(destination.X, destination.Y).Bytes);
            }
        }

        #endregion

        #region DroneFormationAvailableFormationsCommand

        public void DroneFormationAvailableFormationsCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find a way to integrate DroneFormationAvailable for new client");
            }
            else
            {
                var formations = new List<int>();
                foreach (var droneFormation in gameSession.Player.Equipment.GetDroneFormations())
                {
                    formations.Add((int)droneFormation);
                }
                gameSession.Client.Send(commands.old_client.DroneFormationAvailableFormationsCommand.write(formations)
                    .Bytes);
            }
        }

        #endregion

        #region KillScreenCommand

        public void KillScreenCommand(GameSession gameSession, Killscreen killscreen)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("Find a way for killscreen to work on new client");
            }
            else
            {
                gameSession.Client.Send(commands.old_client.KillScreenPostCommand.write(killscreen.KillerName,
                    killscreen.KillerLink, killscreen.Alias,
                    new netty.commands.old_client.DestructionTypeModule(killscreen.GetDestructionType()),
                    killscreen.GetOldOptions()).Bytes);
            }
        }

        #endregion

        #region KillScreenUpdateCommand

        public void KillScreenUpdateCommand(GameSession gameSession,
            List<commands.old_client.KillScreenOptionModule> options)
        {
            if (gameSession.Player.UsingNewClient)
            {
                //throw new NotImplementedException();
            }
            else
            {
                gameSession.Client.Send(commands.old_client.KillScreenUpdateCommand.write(options).Bytes);
            }
        }

        #endregion

        #region UpdateScoremageddonWindow

        public void UpdateScoremageddonWindow(GameSession gameSession, ScoreMageddon scoreMageddon)
        {
            LegacyModule(gameSession,
                $"0|A|SCE|{scoreMageddon.Lives}|{scoreMageddon.GetMaxLives()}|{scoreMageddon.Combo}|{scoreMageddon.GetMaxCombo()}|{scoreMageddon.GetComboTimeLeft()}|{ScoreMageddon.MAX_COMBO_TIME}|{scoreMageddon.Score}");

        }

        #endregion

        #region AttributeCurrencyCommand

        public void AttributeCurrencyCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO Find match for new client");
            }
            else
            {
                Packet.Builder.LegacyModule(gameSession,
                    "0|A|C|" + gameSession.Player.Information.Credits.Get() + "|" +
                    gameSession.Player.Information.Uridium.Get());
            }
        }

        #endregion

        #region TitleCommand

        public void TitleCommand(GameSession gameSession, Player targetPlayer)
        {
            LegacyModule(gameSession,
                $"0|n|t|{targetPlayer.Id}|{targetPlayer.Information.Title.ColorId}|{targetPlayer.Information.Title.Key}");
        }

        #endregion

        #region QuickSlotPremiumCommand

        public void QuickSlotPremiumCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Find QuickSlotPremium for new client");
            }
            else
            {
                gameSession.Client.Send(commands.old_client.QuickSlotPremiumCommand
                    .write(gameSession.Player.Information.Premium.Active).Bytes);
            }
        }

        #endregion

        #region TechStatusCommand

        public void TechStatusCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                //throw new NotImplementedException();
            }
            else
            {
                int repairRobotStatus = gameSession.Player.Storage.BattleRepairRobotActivated ? 2 : 1;
                int energyLeechStatus = gameSession.Player.Storage.EnergyLeechActivated ? 2 : 1;
                int precisionTargeterStatus = gameSession.Player.Storage.PrecisionTargeterActivated ? 2 : 1;
                gameSession.Client.Send(commands.old_client.LegacyModule
                    .write("0|TX|S|" + energyLeechStatus + "|99|0|1|99|0|" + precisionTargeterStatus + "|99|0|1|99|0|" +
                           repairRobotStatus + "|99|0").Bytes);
            }
        }

        #endregion

        #region AssetInfoCommand

        public void AssetInfoCommand(GameSession gameSession, AttackableAsset asset)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AssetInfoCommand.write(asset.Id,
                    new commands.old_client.AssetTypeModule((short) asset.Type), asset.DesignId, asset.ExpansionStage,
                    asset.Core.CurrentHealth, asset.Core.MaxHealth, true, asset.Core.CurrentShield,
                    asset.Core.MaxShield).Bytes);
            }
        }

        #endregion

        #region EventActivationStateCommand

        public void EventActivationStateCommand(GameSession gameSession, short type, bool active)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.EventActivationStateCommand.write(type, active).Bytes);
            }
        }

        #endregion

        #region MapAssetAddBillboardCommand

        public void MapAssetAddBillboardCommand(GameSession gameSession, Billboard billboard)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.MapAssetAddBillboardCommand.write("",
                    new commands.old_client.AssetTypeModule(commands.old_client.AssetTypeModule.BILLBOARD_ASTEROID),
                    new commands.old_client.PartnerTypeModule(billboard.Advertiser), billboard.Position.X,
                    billboard.Position.Y, billboard.Id).Bytes);
            }
        }

        #endregion

        #region EquipReadyCommand

        public void EquipReadyCommand(GameSession gameSession, bool ready)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.EquipReadyCommand.write(ready).Bytes);
            }
        }

        #endregion

        #region BattleStationBuildingUiInitializationCommand

        public void BattleStationBuildingUiInitializationCommand(GameSession gameSession, Asteroid asteroid)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                //List<commands.old_client.StationModuleModule> installedModules = new List<StationModuleModule>();
                //foreach (var moduleEquipped in asteroid.EquippedModules.Values.Where(x =>
                //    x.Clan == gameSession.Player.Clan))
                //{
                //    installedModules.Add(new commands.old_client.StationModuleModule(asteroid.Id, moduleEquipped.Id,
                //        moduleEquipped.SlotId, (short) moduleEquipped.ModuleType, moduleEquipped.Core.CurrentHealth,
                //        moduleEquipped.Core.MaxHealth, moduleEquipped.Core.CurrentShield, moduleEquipped.Core.MaxShield,
                //        moduleEquipped.UpgradeLevel, moduleEquipped.Owner.Name, moduleEquipped.GetInstallationSeconds(),
                //        moduleEquipped.GetInstallationSecondsLeft(), moduleEquipped.GetEmergencyRepairSecondsLeft(),
                //        moduleEquipped.GetEmergencyRepairSeconds(), moduleEquipped.EmergencyRepairCost));
                //}

                //List<commands.old_client.StationModuleModule> ownedModules = new List<StationModuleModule>();
                //foreach (var ownedModule in gameSession.Player.Equipment.Modules.Values)
                //{
                //    if (!ownedModule.Equipped)
                //        ownedModules.Add(new commands.old_client.StationModuleModule(
                //            ownedModule.EquippedBattleStationId, ownedModule.Item.Id, -1,
                //            (short) ownedModule.ModuleType, 1000, 1000, 1000, 1000, 16, gameSession.Player.Name, -1, -1,
                //            -1, -1, 1));
                //}

                //var bestProgresing = asteroid.BestProgressingClan();
                //var bestClan = bestProgresing.Item1 ?? gameSession.Player.Clan;
                //gameSession.Client.Send(commands.old_client.BattleStationBuildingUiInitializationCommand.write(
                //    asteroid.Id, asteroid.AssignedBattleStationId, asteroid.Name,
                //    new commands.old_client.AsteroidProgressCommand(asteroid.AssignedBattleStationId,
                //        asteroid.GetClanProgress(gameSession.Player.Clan), asteroid.GetClanProgress(bestClan),
                //        gameSession.Player.Clan.Name, bestClan.Name,
                //        new commands.old_client.EquippedModulesModule(installedModules),
                //        asteroid.Buildable(gameSession.Player)),
                //    new commands.old_client.AvailableModulesCommand(ownedModules), 0, 120, 1).Bytes);
            }
        }

        #endregion

        #region BattleStationBuildingStateCommand

        public void BattleStationBuildingStateCommand(GameSession gameSession, Asteroid asteroid)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.BattleStationBuildingStateCommand.write(asteroid.Id,
                    asteroid.AssignedBattleStationId, asteroid.Name, asteroid.EndOfBuild, asteroid.BuildTime,
                    asteroid.Clan.Name, new commands.old_client.FactionModule(2)).Bytes);
            }
        }

        #endregion

        #region BattleStationManagementUiInitializationCommand

        public void BattleStationManagementUiInitializationCommand(GameSession gameSession,
            ClanBattleStation battleStation)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                //List<commands.old_client.StationModuleModule> equipment = new List<StationModuleModule>();
                //foreach (var moduleEquipped in battleStation.EquippedModules.Values)
                //{
                //    equipment.Add(new commands.old_client.StationModuleModule(battleStation.BattleStationId,
                //        moduleEquipped.Id, moduleEquipped.SlotId, (short) moduleEquipped.ModuleType,
                //        moduleEquipped.Core.CurrentHealth, moduleEquipped.Core.MaxHealth,
                //        moduleEquipped.Core.CurrentShield, moduleEquipped.Core.MaxShield, moduleEquipped.UpgradeLevel,
                //        moduleEquipped.Owner.Name, moduleEquipped.GetInstallationSeconds(),
                //        moduleEquipped.GetInstallationSecondsLeft(), moduleEquipped.GetEmergencyRepairSecondsLeft(),
                //        moduleEquipped.GetEmergencyRepairSeconds(), moduleEquipped.EmergencyRepairCost));
                //}

                //List<commands.old_client.StationModuleModule> availableModules = new List<StationModuleModule>();
                //foreach (var ownedModule in gameSession.Player.Equipment.Modules.Values)
                //{
                //    if (!ownedModule.Equipped)
                //        availableModules.Add(new commands.old_client.StationModuleModule(
                //            ownedModule.EquippedBattleStationId, ownedModule.Item.Id, -1,
                //            (short) ownedModule.ModuleType, 1000, 1000, 1000, 1000, 16, gameSession.Player.Name, -1, -1,
                //            -1, -1, 1));
                //}

                //gameSession.Client.Send(commands.old_client.BattleStationManagementUiInitializationCommand.write(
                //    battleStation.Id, battleStation.BattleStationId, battleStation.Name, battleStation.Clan.Name,
                //    new commands.old_client.FactionModule((short) battleStation.Faction),
                //    new commands.old_client.BattleStationStatusCommand(battleStation.Id, battleStation.BattleStationId,
                //        battleStation.Name, battleStation.DeflectorShieldActive,
                //        battleStation.GetDeflectorShieldSeconds(), battleStation.DeflectorShieldSecondsMax(),
                //        battleStation.GetAttackRating(), battleStation.GetDefenceRating(),
                //        battleStation.GetRepairRating(), battleStation.GetHonorBoostRating(),
                //        battleStation.GetExperienceBoostRating(), battleStation.GetDamageBoostRating(),
                //        battleStation.DeflectorShieldRate, battleStation.RepairPrice,
                //        new commands.old_client.EquippedModulesModule(equipment)),
                //    new commands.old_client.AvailableModulesCommand(availableModules), battleStation.DeflectorShieldMin,
                //    battleStation.DeflectorShieldMax, battleStation.DeflectorShieldIncrement,
                //    battleStation.DeflectorDeactivationPossible()).Bytes);
            }
        }

        #endregion

        #region AttackHitAssetCommand

        public void AttackHitAssetCommand(GameSession gameSession, int assetId, int hitpointsNow, int hitpointsMax)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AttackHitAssetCommand
                    .write(assetId, hitpointsNow, hitpointsMax).Bytes);
            }
        }

        #endregion

        #region AbilityStatusFullCommand

        public void AbilityStatusFullCommand(GameSession gameSession, List<Ability> abilities)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                List<commands.old_client.AbilityStatusSingleCommand> abilitiesModule =
                    new List<AbilityStatusSingleCommand>();
                foreach (var ability in abilities)
                {
                    abilitiesModule.Add(
                        new commands.old_client.AbilityStatusSingleCommand(ability.AbilityId, ability.Enabled));
                }

                gameSession.Client.Send(commands.old_client.AbilityStatusFullCommand.write(abilitiesModule).Bytes);
            }
        }

        #endregion

        #region AbilityStartCommand

        public void AbilityStartCommand(GameSession gameSession, Ability ability)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AbilityStartCommand
                    .write(ability.AbilityId, ability.Player.Id, ability.IsStoppable).Bytes);
            }
        }

        #endregion

        #region AbilityStopCommand

        public void AbilityStopCommand(GameSession gameSession, Ability ability)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AbilityStopCommand
                    .write(ability.AbilityId, ability.Player.Id, ability.TargetIds).Bytes);
            }
        }

        #endregion

        #region AbilityEffectActivationCommand

        public void AbilityEffectActivationCommand(GameSession gameSession, Ability ability)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AbilityEffectActivationCommand
                    .write(ability.AbilityId, ability.ActivatorId, ability.TargetIds).Bytes);
            }
        }

        #endregion

        #region AbilityEffectDeActivationCommand

        public void AbilityEffectDeActivation(GameSession gameSession, Ability ability)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AbilityEffectDeActivationCommand
                    .write(ability.AbilityId, ability.ActivatorId, ability.TargetIds).Bytes);
            }
        }

        #endregion

        #region AbilityStatusSingleCommand

        public void AbilityStatusSingleCommand(GameSession gameSession, Ability ability)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AbilityStatusSingleCommand
                    .write(ability.AbilityId, ability.Enabled).Bytes);
            }
        }

        #endregion

        #region MapEventOreCommand

        public void MapEventOreCommand(GameSession gameSession, Ore ore, OreCollection eventType)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.MapEventOreCommand.write((short) eventType,
                    new commands.old_client.OreTypeModule((short) ore.Type), ore.Hash).Bytes);
            }
        }

        #endregion

        #region AssetRemoveCommand

        public void AssetRemoveCommand(GameSession gameSession, Asset asset)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AssetRemoveCommand
                    .write(new commands.old_client.AssetTypeModule((short) asset.Type), asset.Id).Bytes);
            }
        }

        #endregion

        #region QuestGiversAvailableCommand

        public void QuestGiversAvailableCommand(GameSession gameSession, QuestGiver questGiver)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.QuestGiversAvailableCommand
                    .write(new List<commands.old_client.QuestGiverModule>
                    {
                        new commands.old_client.QuestGiverModule(questGiver.QuestGiverId, questGiver.Id)
                    }).Bytes);
            }
        }

        #endregion

        #region QuestListCommand

        public void QuestListCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                var quests = World.StorageManager.Quests.Values;

                var list = new List<commands.old_client.QuestSlimInfoModule>();
                foreach (var quest in quests)
                {
                    list.Add(new commands.old_client.QuestSlimInfoModule(
                        quest.Id, quest.Root.Id, 0,
                        new List<commands.old_client.QuestTypeModule>
                        {
                            new commands.old_client.QuestTypeModule((short) quest.QuestType)
                        },
                        new commands.old_client.QuestIconModule((short) quest.Icon),
                        new commands.old_client.QuestAcceptabilityStatus(
                            (short) quest.GetAcceptabilityStatus(gameSession.Player)),
                        new List<QuestRequirementModule>()));
                }

                gameSession.Client.Send(commands.old_client.QuestListCommand.write(list, false, 5, 0).Bytes);
            }
        }

        #endregion

        #region QuestInfoCommand

        public void QuestInfoCommand(GameSession gameSession, Quest quest)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                var elements = QuestElement.ParseElementsOld(gameSession.Player, quest.Root.Elements);
                var definition = new QuestDefinitionModule(quest.Id,
                    new List<QuestTypeModule> {new QuestTypeModule((short) quest.QuestType)},
                    new QuestCaseModule(quest.Root.Id, quest.Root.Active, quest.Root.Mandatory, quest.Root.Ordered,
                        quest.Root.MandatoryCount, elements),
                    quest.GetOldLootModule(), new List<QuestIconModule> {new QuestIconModule((short) quest.Icon)});

                gameSession.Client.Send(commands.old_client.QuestInfoCommand.write(definition,
                    new List<QuestChallengeRatingModule>(),
                    new QuestChallengeRatingModule("Shock", "http://google.bg", 21, 5, 5)).Bytes);
            }
        }

        #endregion

        #region QuestInitializationCommand

        public void QuestInitializationCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                foreach (var quest in gameSession.Player.QuestData.GetActiveQuests())
                {
                    var elements = QuestElement.ParseElementsOld(gameSession.Player, quest.Root.Elements);
                    var definition = new QuestDefinitionModule(quest.Id, new List<QuestTypeModule> { new QuestTypeModule((short)quest.QuestType) },
                        new QuestCaseModule(quest.Root.Id, quest.Root.Active, quest.Root.Mandatory, quest.Root.Ordered, quest.Root.MandatoryCount, elements),
                        quest.GetOldLootModule(), new List<QuestIconModule> { new QuestIconModule((short)quest.Icon) });
                    gameSession.Client.Send(commands.old_client.QuestInitializationCommand.write(definition).Bytes);
                }
            }
        }
        #endregion

        #region QuestCompletedCommand

        public void QuestCompletedCommand(GameSession gameSession, Quest quest)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.QuestCompletedCommand.write(quest.Id, 5).Bytes);
            }
        }
        #endregion

        #region QuestConditionUpdateCommand

        public void QuestConditionUpdateCommand(GameSession gameSession, QuestSerializableState state)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.QuestConditionUpdateCommand.write(state.ConditionId, new commands.old_client.QuestConditionStateModule(state.CurrentValue, state.Active, state.Completed)).Bytes);
            }
        }
        #endregion

        #region QuestCancelledCommand
        
        public void QuestCancelledCommand(GameSession gameSession, int questId)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.QuestCancelledCommand.write(questId).Bytes);
            }
        }

        #endregion

        #region PetBuffCommand
        public void PetBuffCommand(GameSession gameSession, bool add, BuffPattern effectId, List<int> addingParameters)
        {
            if (gameSession.Player.UsingNewClient) { }
            else
            {
                gameSession.Client.Send(netty.commands.old_client.PetBuffCommand.write((short)(add ? 0 : 1), (short)effectId, addingParameters).Bytes);
            }
        }
        #endregion

        #region PetFuelUpdateCommand

        public void PetFuelUpdateCommand(GameSession gameSession, Pet pet)
        {
            if (gameSession.Player.UsingNewClient) { }
            else
            {
                gameSession.Client.Send(commands.old_client.PetFuelUpdateCommand.write(pet.Fuel, pet.MaxFuel).Bytes);
            }
        }

        #endregion

        #region PetLevelUpdateCommand

        public void PetLevelUpdateCommand(GameSession gameSession, Pet pet)
        {
            if (gameSession.Player.UsingNewClient) { }
            else
            {
                gameSession.Client.Send(commands.old_client.PetLevelUpdateCommand.write((short)pet.Level.Id, pet.Level.Experience, (short)pet.Hangar.Ship.Id, pet.ExpansionStage).Bytes);
            }
        }

        #endregion

        #region PetIsDestroyedCommand

        public void PetIsDestroyedCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetIsDestroyedCommand.write().Bytes);
            }
        }

        #endregion

        #region PetRepairCompleteCommand

        public void PetRepairCompleteCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetRepairCompleteCommand.write().Bytes);
            }
        }
        #endregion

        #region PetUIRepairButtonCommand

        public void PetUIRepairButtonCommand(GameSession gameSession, bool enabled, int repairCost)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetUIRepairButtonCommand.write(enabled, repairCost).Bytes);
            }
        }
        #endregion

        #region PetShieldUpdateCommand

        public void PetShieldUpdateCommand(GameSession gameSession, int petShieldNow, int petShieldMax)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetShieldUpdateCommand.write(petShieldNow, petShieldMax).Bytes);
            }
        }
        #endregion

        #region PetHitpointsUpdateCommand

        public void PetHitpointsUpdateCommand(GameSession gameSession, int hitpointsNow, int hitpointsMax, bool useRepairGear)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetHitpointsUpdateCommand.write(hitpointsNow, hitpointsMax, useRepairGear).Bytes);
            }
        }
        #endregion

        #region AttributeOreCountUpdateCommand

        public void AttributeOreCountUpdateCommand(GameSession gameSession, Cargo cargo)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.AttributeOreCountUpdateCommand.write(Cargo.ParseOld(cargo.Palladium, cargo.Seprom, cargo.Promerium, cargo.Xenomit, cargo.Duranium, cargo.Prometid, cargo.Terbium, cargo.Endurium, cargo.Prometium)).Bytes);
            }
        }
        #endregion

        #region LMCollectResourcesCommand

        public void LMCollectResourcesCommand(GameSession gameSession,int palla, int seprom, int promerium, int xenomit, int duranium, int prometid, int terbium, int endurium, int prometium)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.LMCollectResourcesCommand.write(new LogMessengerPriorityModule(0), Cargo.ParseOld(palla, seprom, promerium, xenomit, duranium, prometid, terbium, endurium, prometium)).Bytes);
            }
        }
        #endregion

        #region VisualModifierCommand

        public void VisualModifierCommand(GameSession gameSession, VisualEffect visual)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.VisualModifierCommand.write(visual.Entity.Id, (short)visual.Visual, visual.Attribute, visual.Active).Bytes);
            }
        }
        #endregion

        #region LabUpdateItemCommand

        public void LabUpdateItemCommand(GameSession gameSession, Skylab skylab)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.LabUpdateItemCommand.write(skylab.UpdateInfoOld()).Bytes);
            }
        }
        #endregion

        #region TradeWindowActivationCommand

        public void TradeWindowActivationCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                LegacyModule(gameSession, "0|UI|W|MAW|6"); // trade window
            }
        }

        #endregion

        #region ClanTagChangedCommand

        public void ClanTagChangedCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.ClanTagChangedCommand.write(gameSession.Player.Clan.Tag).Bytes);
            }
        }
        #endregion

        #region QuestPrivilegeCommand

        public void QuestPrivilegeCommand(GameSession gameSession, int questId)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.QuestPrivilegeCommand.write(questId).Bytes);
            }
        }
        #endregion

        #region OrePriceCommand

        public void OrePriceCommand(GameSession gameSession, int prometium, int endurium, int terbium, int prometid,
            int duranium, int promerium, int palladium)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                LegacyModule(gameSession, "0|g|" + prometium + "|" + endurium + "|" + terbium + "|" + prometid + "|" + duranium + "|" + promerium + "|" + palladium);
            }
        }
        #endregion

        #region JumpCPUFeedbackCommand

        public void JumpCPUFeedbackCommand(GameSession gameSession, int mapId, int uridiumPrice, bool possible)
        {
            LegacyModule(gameSession, "0|A|JCPU|C|" + mapId + "|" + uridiumPrice + "|" + Convert.ToInt32(possible));
        }
        #endregion

        #region SpaceBallInitializeScoreCommand

        public void SpaceBallInitializeScoreCommand(GameSession gameSession, Spaceball ball)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.SpaceBallInitializeScoreCommand.write(ball.ScoreMmo, ball.ScoreEic, ball.ScoreVru, ball.Owner, ball.Speed).Bytes);
            }
        }
        #endregion

        #region ClanWindowInitCommand

        public void ClanWindowInitCommand(GameSession gameSession)
        {
            var clan = gameSession.Player.Clan;
            if (clan.Id == 0) return;
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                var list = new List<commands.old_client.ClanMemberModule>();
                var onMyMap = new List<int>();
                foreach (var clanMember in clan.Members)
                {
                    if (clanMember.Key == gameSession.Player.Id) continue;
                    list.Add(new commands.old_client.ClanMemberModule(clanMember.Value.Id, clanMember.Value.Name));
                    if (clanMember.Value.Player != null)
                    {
                        onMyMap.Add(clanMember.Value.Id);
                    }
                }

                gameSession.Client.Send(commands.old_client.ClanWindowInitCommand.write(clan.Id, clan.Name, clan.Tag, list, onMyMap).Bytes);
            }
        }
        #endregion

        #region ClanMemberOnlineInfoCommand

        public void ClanMemberOnlineInfoCommand(GameSession session, ClanMember onlineMember)
        {
            if (session.Player.UsingNewClient)
            {

            }
            else
            {
                session.Client.Send(commands.old_client.ClanMemberOnlineInfoCommand.write(onlineMember.Id, onlineMember.Player == null).Bytes);
            }
        }
        #endregion

        #region ClanMemberMapInfoCommand

        public void ClanMemberMapInfoCommand(GameSession session, ClanMember member)
        {
            if (session.Player.UsingNewClient)
            {

            }
            else
            {
                session.Client.Send(commands.old_client.ClanMemberMapInfoCommand.write(member.Id, member.Player.Spacemap.Id).Bytes);
            }
        }

        #endregion

        #region SpaceBallUpdateSpeedCommand

        public void SpaceBallUpdateSpeedCommand(GameSession gameSession, Spaceball spaceballEvent)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.SpaceBallUpdateSpeedCommand.write(spaceballEvent.Owner, spaceballEvent.Speed).Bytes);
            }
        }

        #endregion

        #region SpaceBallUpdateScoreCommand

        public void SpaceBallUpdateScoreCommand(GameSession gameSession, Faction faction, int score, int gate)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.SpaceBallUpdateScoreCommand.write((int) faction, score, gate).Bytes);
            }
        } 

        #endregion
        
        #region PetExperiencePointsUpdateCommand

        public void PetExperiencePointsUpdateCommand(GameSession gameSession, double exp, double maxExp)
        {
            if (gameSession.Player.UsingNewClient)
            {
                
            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetExperiencePointsUpdateCommand.write(exp, maxExp).Bytes);
            }
        }
        #endregion

        #region SendErrorCommand
        public void SendErrorCommand(GameSession gameSession, SessionErrors error)
        {
            LegacyModule(gameSession, "ERR|" + (int)error);
        }
        #endregion

        #region MapRemovePOICommand

        public void MapRemovePOICommand(GameSession gameSession, POI poi)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.MapRemovePOICommand.write(poi.Id).Bytes);
            }
        }
        #endregion

        #region CreateBannerAd

        public void CreateBannerAd(GameSession gameSession, string paymentId, string direction = "n")
        {
            Packet.Builder.LegacyModule(gameSession, "0|UI|CW|AD|||" + direction + "|" + paymentId);
        }
        #endregion

        #region PetGearResetCommand

        public void PetGearResetCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetGearResetCommand.write().Bytes);
            }
        }
        #endregion

        #region ShipWarpCanceledCommand

        public void ShipWarpCanceledCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.ShipWarpCanceledCommand.write().Bytes);
            }
        }
        #endregion

        #region ShipWarpCompletedCommand

        public void ShipWarpCompletedCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.ShipWarpCompletedCommand.write().Bytes);
            }
        }
        #endregion

        #region ShipWarpNotAllowedCommand

        public void ShipWarpNotAllowedCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.ShipWarpNotAllowedCommand.write().Bytes);
            }
        }
        #endregion

        #region HellstormAttackCommand

        public void HellstormAttackCommand(GameSession gameSession, int attackerId, int targetId, bool hit, int currentLoad, short rocketType)
        {
            if (gameSession.Player.UsingNewClient)
            {

            }
            else
            {
                gameSession.Client.Send(commands.old_client.HellstormAttackCommand.write(attackerId, targetId, hit, currentLoad, new commands.old_client.AmmunitionTypeModule(rocketType)).Bytes);
            }
        }
        #endregion

    }
}