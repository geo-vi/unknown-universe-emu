using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NettyBaseReloaded.Chat.objects.chat.rooms;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.settings;
using Global = NettyBaseReloaded.Main.Global;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

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
                var ws = new commands.new_client.WindowSettingsModule(8, "23,1|24,1|25,1|27,1|26,1|100,1|34,1|36,1|33,1|35,1|39,1|38,1|37,1|32,1|", false);
                var gm = new commands.new_client.GameplaySettingsModule(false, false, false, false, false, true, false, false, false, false);
                var z9 = new commands.new_client.QuestSettingsModule(false, true, true, false, false, false);
                var ds = new commands.new_client.DisplaySettingsModule(true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, 3, 4, 4, 3, 3, 4, 3, 3, true, true, true, true);

                gameSession.Client.Send(commands.new_client.UserSettingsCommand.write(qs, asm, ws, gm, z9, ds).Bytes);
            }
            else
            {
                var qs = new commands.old_client.QualitySettingsModule(false, 3, 3, 3, true, 3, 3, 3, 3, 3, 3);
                var asm = new commands.old_client.AudioSettingsModule(false, false, false);
                var ws = new commands.old_client.WindowSettingsModule(false, 1,
                    "0,444,-1,0,1,1057,329,1,20,39,530,0,3,1021,528,1,5,-10,-6,0,24,463,15,0,10,101,307,0,36,100,400,0,13,315,122,0,23,1067,132,0",
                        "5,240,150,20,300,150,36,260,175,", 11, "313,480", "23,0,24,0,25,1,26,0,27,0", "313,451", "0",
                        "313,500", "0");
                var gm = new commands.old_client.GameplaySettingsModule(false, true, true, true, true, true, true, true);
                var ds = new commands.old_client.DisplaySettingsModule(false, true, true, true, true, true, false, true, true, true, true, true, true, true, true, true);

                gameSession.Client.Send(commands.old_client.UserSettingsCommand.write(qs, ds, asm, ws, gm).Bytes);
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
                new Window.New_Client("scoreevent", "title_scoreevent", false),
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
                new commands.new_client.commandKn(commands.new_client.commandKn.GAME_FEATURE_BAR, topLeftButtons, "0,0", "0"),
                new commands.new_client.commandKn(commands.new_client.commandKn.GENERIC_FEATURE_BAR, topRightButtons, "100,0", "0")
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
                    new commands.new_client.SlotbarQuickslotModule("standardSlotBar", player.Settings.Slotbar.QuickslotItems, "50,85|0,40",
                        "0", true),
                    new commands.new_client.SlotbarQuickslotModule("premiumSlotBar", player.Settings.Slotbar.PremiumQuickslotItems, "50,85|0,80", "0", true)
                };
                gameSession.Client.Send(
                    commands.new_client.SlotbarsCommand.write(player.Settings.Slotbar.GetCategories(), "50,85", slotbars)
                        .Bytes);

            }
            else
            {
                var ammo = new List<netty.commands.old_client.AmmunitionCountModule>();
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.X1), player.Information.Ammunitions["ammunition_laser_lcb-10"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.X2), player.Information.Ammunitions["ammunition_laser_mcb-25"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.X3), player.Information.Ammunitions["ammunition_laser_mcb-50"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.X4), player.Information.Ammunitions["ammunition_laser_ucb-100"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.SAB), player.Information.Ammunitions["ammunition_laser_sab-50"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.CBO), player.Information.Ammunitions["ammunition_laser_cbo-100"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.RSB), player.Information.Ammunitions["ammunition_laser_rsb-75"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.JOB100), player.Information.Ammunitions["ammunition_laser_job-100"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.EMP), player.Information.Ammunitions["ammunition_specialammo_emp-01"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.MINE), player.Information.Ammunitions["ammunition_laser_sab-50"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.INSTANT_SHIELD), player.Information.Ammunitions["ammunition_laser_sab-50"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.SMARTBOMB), player.Information.Ammunitions["ammunition_laser_sab-50"].Get()));
                ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.R310), player.Information.Ammunitions["ammunition_rocket_r-310"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.PLT2021), player.Information.Ammunitions["ammunition_rocket_plt-2021"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.PLT2026), player.Information.Ammunitions["ammunition_rocket_plt-2026"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.PLT3030), player.Information.Ammunitions["ammunition_rocket_plt-3030"].Get()));
                //ammo.Add(new netty.commands.old_client.AmmunitionCountModule(new netty.commands.old_client.AmmunitionTypeModule(netty.commands.old_client.AmmunitionTypeModule.ECO_ROCKET), player.Information.Ammunitions["ammunition_laser_sab-50"].Get()));
                gameSession.Client.Send(netty.commands.old_client.AmmunitionCountUpdateCommand.write(ammo).Bytes);
                player.Settings.Slotbar.GetCategories();
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
                        player.Hangar.Ship.ToStringLoot(),
                        player.Speed,
                        player.CurrentShield,
                        player.MaxShield,
                        player.CurrentHealth,
                        player.MaxHealth,
                        player.Hangar.Ship.Cargo, //freeCargo
                        player.Hangar.Ship.Cargo, //maxCargo
                        player.CurrentNanoHull,
                        player.MaxNanoHull,
                        player.Position.X,
                        player.Position.Y,
                        player.Spacemap.Id,
                        (int) player.FactionId,
                        player.Clan.Id, //clan
                        player.Equipment.LaserCount(), //idk
                        player.Information.Premium,
                        player.Information.Experience.Get(),
                        player.Information.Honor.Get(),
                        player.Information.Level.Id,
                        player.Information.Credits.Get(),
                        player.Information.Uridium.Get(),
                        0,//Jackpot
                        (int) player.RankId,
                        player.Clan.Tag, //clanTag
                        0, // player GG rings
                        true,
                        false, //cloaked
                        true,
                        new List<commands.new_client.VisualModifierCommand>()
                    ).Bytes);
            else
                gameSession.Client.Send(
                    commands.old_client.ShipInitializationCommand.write(
                        player.Id,
                        player.Name,
                        player.Hangar.Ship.Id,
                        player.Speed,
                        player.CurrentShield,
                        player.MaxShield,
                        player.CurrentHealth,
                        player.MaxHealth,
                        player.Hangar.Ship.Cargo,
                        player.Hangar.Ship.Cargo,
                        player.CurrentNanoHull,
                        player.MaxNanoHull,
                        player.Position.X,
                        player.Position.Y,
                        player.Spacemap.Id,
                        (int) player.FactionId,
                        player.Clan.Id,
                        0,
                        0,
                        player.Equipment.LaserCount(),
                        player.Information.Premium,
                        player.Information.Experience.Get(),
                        player.Information.Honor.Get(),
                        player.Information.Level.Id,
                        player.Information.Credits.Get(),
                        player.Information.Uridium.Get(),
                        0,
                        (int) player.RankId,
                        player.Clan.Tag, 
                        0,
                        true,
                        false,
                        new List<commands.old_client.VisualModifierCommand>()).Bytes);
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
                Console.WriteLine("ID: {0} Type: {1} Position: {2}", character.Id, character.Hangar.Ship.Id, character.Position);

            byte[] bytes = null;
            if (gameSession.Player.UsingNewClient)
            {
                if (character is Player)
                {
                    var pChar = (Player) character;
                    bytes = commands.new_client.ShipCreateCommand.write(pChar.Id,
                        pChar.Hangar.Ship.ToStringLoot(), pChar.Equipment.LaserCount(), pChar.Clan.Tag, pChar.Name, pChar.Position.X,
                        pChar.Position.Y,
                        (int) pChar.FactionId, 0, (int) pChar.RankId, false,
                        new commands.new_client.ClanRelationModule(pChar.Clan.GetRelation(gameSession.Player.Clan)), 0,
                        false, false, false, 0, 0, new List<commands.new_client.VisualModifierCommand>(),
                        new commands.new_client.commandK13(commands.new_client.commandK13.DEFAULT)).Bytes;
                }
                else if (character is Pet)
                {
                    PetActivationCommand(gameSession, character as Pet);
                }
                else
                {
                    bytes = commands.new_client.ShipCreateCommand.write(character.Id,
                        character.Hangar.Ship.ToStringLoot(), 0, character.Clan.Tag, character.Name, character.Position.X,
                        character.Position.Y,
                        (int)character.FactionId, character.Clan.Id, 0, false,
                        new commands.new_client.ClanRelationModule(character.Clan.GetRelation(gameSession.Player.Clan)), 0,
                        false, true, false, 0, 0, new List<commands.new_client.VisualModifierCommand>(),
                        new commands.new_client.commandK13(commands.new_client.commandK13.DEFAULT)).Bytes;
                }
            }
            else
            {
                if (character is Player)
                {
                    var pChar = (Player) character;
                    bytes = commands.old_client.ShipCreateCommand.write(pChar.Id,
                        pChar.Hangar.Ship.Id, pChar.Equipment.LaserCount(), pChar.Clan.Tag, pChar.Name, pChar.Position.X,
                        pChar.Position.Y,
                        (int) pChar.FactionId, pChar.Clan.Id, (int) pChar.RankId, false,
                        new commands.old_client.ClanRelationModule(pChar.Clan.GetRelation(gameSession.Player.Clan)), 0,
                        false, false, false, 0, 0, new List<commands.old_client.VisualModifierCommand>()).Bytes;
                }
                else if (character is Pet)
                {
                    PetActivationCommand(gameSession, character as Pet);
                }
                else
                {
                    bytes = commands.old_client.ShipCreateCommand.write(character.Id,
                        character.Hangar.Ship.Id, 0, character.Clan.Tag, character.Name, character.Position.X,
                        character.Position.Y,
                        (int) character.FactionId, character.Clan.Id, 0, false,
                        new commands.old_client.ClanRelationModule(character.Clan.GetRelation(gameSession.Player.Clan)), 0,
                        false, true, false, 0, 0, new List<commands.old_client.VisualModifierCommand>()).Bytes;
                }
            }
            gameSession.Client.Send(bytes);
        }
        #endregion
        #region ShipRemoveCommand

        public void ShipRemoveCommand(GameSession gameSession, Character character)
        {
            if (Properties.Game.DEBUG_ENTITIES)
                Console.WriteLine("ID: {0} Type: {1} Position: {2}", character.Id, character.Hangar.Ship.Id, character.Position);

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
                    commands.new_client.MoveCommand.write(character.Id, character.Destination.X, character.Destination.Y,
                        time).Bytes);
            }
            else
            {
                gameSession.Client.Send(
                    commands.old_client.MoveCommand.write(character.Id, character.Destination.X, character.Destination.Y,
                        time).Bytes);
            }
        }
        #endregion
        #region DronesCommand

        public void DronesCommand(GameSession gameSession, Character character)
        {
            int droneDesignId = 0; // 18 => frost
            if (character.Hangar.Drones.Count <= 0) return;
            var command = "0|n|d|" + character.Id + "|" + (int)character.Formation;

            foreach (var d in character.Hangar.Drones)
            {
                command += "|" + (int)d.DroneType + "|" + 6 + "|" + droneDesignId;
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
                gameSession.Client.Send(commands.new_client.JumpgateCreateCommand.write(portal.Id, (int)portal.Faction, portal.Gfx, portal.Position.X, portal.Position.Y,
                    portal.Visible, portal.Working, new List<int>()).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.LegacyModule.write(portal.ToPacket()).Bytes);
            }
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
                LegacyModule(gameSession, $"0|{ServerCommands.PLAY_PORTAL_ANIMATION}|{portal.DestinationMapId}|{portal.Id}", true);
            }
        }
        #endregion
        #region MapAssetActionAvailableCommand

        public void MapAssetActionAvailableCommand(GameSession gameSession, Object _object, bool toggled, bool activatable)
        {
            if (gameSession.Player.UsingNewClient)
            {
                short state = commands.new_client.MapAssetActionAvailableCommand.OFF;
                if (toggled) state = commands.new_client.MapAssetActionAvailableCommand.ON;

                gameSession.Client.Send(commands.new_client.MapAssetActionAvailableCommand.write(_object.Id, state,
                    activatable, new commands.new_client.ClientUITooltip(new List<commands.new_client.ClientUITooltipTextFormat>()), new commands.new_client.commandu1C()).Bytes);
            }
        }
        #endregion
        #region LegacyModule

        public void LegacyModule(GameSession gameSession, string message, bool toOldClientOnly = false)
        {
            if (gameSession.Player.UsingNewClient && !toOldClientOnly)
            {
                gameSession.Client.Send(commands.new_client.LegacyModule.write(message).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.LegacyModule.write(message).Bytes);
            }
        }

        #endregion
        #region ShipSelectionCommand

        public void ShipSelectionCommand(GameSession gameSession, Character character)
        {
            if (gameSession.Player.UsingNewClient)
            {
                if (character == null) gameSession.Client.Send(commands.new_client.ShipSelectionCommand.write(0,0,0,0,0,0,0,0,false).Bytes);
                else gameSession.Client.Send(commands.new_client.ShipSelectionCommand.write(character.Id, character.Hangar.Ship.Id, character.CurrentShield, character.MaxShield,
                    character.CurrentHealth, character.MaxHealth, character.CurrentNanoHull, character.MaxNanoHull, false).Bytes);
            }
            else
            {
                if (character == null) gameSession.Client.Send(commands.old_client.ShipSelectionCommand.write(0,0,0,0,0,0,0,0,false).Bytes);
                else gameSession.Client.Send(commands.old_client.ShipSelectionCommand.write(character.Id, character.Hangar.Ship.Id, character.CurrentShield, character.MaxShield,
                    character.CurrentHealth, character.MaxHealth, character.CurrentNanoHull, character.MaxNanoHull, false).Bytes);
            }
        }

        #endregion
        #region AssetCreateCommand

        public void AssetCreateCommand(GameSession gameSession, Asset asset)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.AssetCreateCommand.write(new commands.new_client.AssetTypeModule(asset.Type), asset.Name, asset.FactionId, asset.ClanTag, asset.AssetId, asset.DesignId,
                asset.ExpansionStage, asset.Position.X, asset.Position.Y, asset.ClanId, asset.Invisible, asset.VisibleOnWarnRadar, asset.DetectedByWarnRadar, true,
                    new commands.new_client.ClanRelationModule(0), new List<commands.new_client.VisualModifierCommand>()).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AssetCreateCommand.write(new commands.old_client.AssetTypeModule(asset.Type), asset.Name, asset.FactionId, asset.ClanTag, asset.AssetId, asset.DesignId,
                    asset.ExpansionStage, asset.Position.X, asset.Position.Y, asset.ClanId, asset.Invisible, asset.VisibleOnWarnRadar, asset.DetectedByWarnRadar,
                    new commands.old_client.ClanRelationModule(0), new List<commands.old_client.VisualModifierCommand>()).Bytes);
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
                    gameSession.Client.Send(commands.new_client.commandY2c.write(station.Id, 6, 1500, station.Position.X, station.Position.Y));
                }
                else
                {
                    foreach (var module in station.Modules)
                    {
                        AssetCreateCommand(gameSession, new Asset(module.Id, "", module.Type, (int)station.Faction, "", module.Id, 0, 0, module.Position, 0, false, false, false));
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
                gameSession.Client.Send(commands.new_client.AttackLaserRunCommand.write(attackerId, targetId, laserColor, isDiminishedBySkillShield, skilledLaser).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AttackLaserRunCommand.write(attackerId, targetId, laserColor, isDiminishedBySkillShield, skilledLaser).Bytes);
            }
        }

        #endregion

        #region AttackHitCommand

        public void AttackHitCommand(GameSession gameSession, Character attacker, Character target, int damage,
            short effect)
        {
            if (gameSession.Player.UsingNewClient)
            {
                if (damage == 0)
                    gameSession.Client.Send(commands.new_client.AttackMissedCommand.write(new commands.new_client.AttackTypeModule(effect), target.Id, 0).Bytes);
                else gameSession.Client.Send(commands.new_client.AttackHitCommand.write(
                    new commands.new_client.AttackTypeModule(effect), attacker.Id,
                    target.Id, target.CurrentHealth, target.CurrentShield, target.CurrentNanoHull, damage, true).Bytes);
            }
            else
            {
                if (damage == 0)
                    gameSession.Client.Send(commands.old_client.AttackMissedCommand.write(new commands.old_client.AttackTypeModule(effect), target.Id, 0).Bytes);
                else gameSession.Client.Send(commands.old_client.AttackHitCommand.write(
                    new commands.old_client.AttackTypeModule(effect), attacker.Id,
                    target.Id, target.CurrentHealth, target.CurrentShield, target.CurrentNanoHull, damage, true).Bytes);
            }
        }

        #endregion

        #region HitpointInfoCommand

        public void HitpointInfoCommand(GameSession gameSession, int hitpoints, int hitpointsMax, int nanoHull, int nanoHullMax)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.HitpointInfoCommand.write(hitpoints, hitpointsMax, nanoHull, nanoHullMax).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.HitpointInfoCommand.write(hitpoints, hitpointsMax, nanoHull, nanoHullMax).Bytes);
            }
        }

        #endregion

        #region AttributeShieldUpdateCommand

        public void AttributeShieldUpdateCommand(GameSession gameSession, int shieldNow, int shieldMax)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.AttributeShieldUpdateCommand.write(shieldNow, shieldMax).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AttributeShieldUpdateCommand.write(shieldNow, shieldMax).Bytes);
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
            if (pet == null) return;
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.PetInitializationCommand.write(true, pet.HasFuel(), !pet.Controller.Dead).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetInitializationCommand.write(true, pet.HasFuel(), !pet.Controller.Dead).Bytes);
            }
        }

        #endregion

        #region PetActivationCommand

        public void PetActivationCommand(GameSession gameSession, Pet pet)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.PetActivationCommand.write(pet.GetOwner().Id, pet.Id, 12, 1, pet.Name,
                (short)pet.FactionId, pet.Clan.Id, (short)pet.Level.Id, pet.Clan.Tag, new commands.new_client.ClanRelationModule(pet.Clan.GetRelation(gameSession.Player.Clan)),
                pet.Position.X, pet.Position.Y, pet.Speed, false, true, new commands.new_client.commandK13(0)).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetActivationCommand.write(pet.GetOwner().Id, pet.Id, 12, 1, pet.Name,
                    (short)pet.FactionId, pet.Clan.Id, (short)pet.Level.Id, pet.Clan.Tag, new commands.old_client.ClanRelationModule(pet.Clan.GetRelation(gameSession.Player.Clan)),
                    pet.Position.X, pet.Position.Y, pet.Speed, false, true).Bytes);
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
                gameSession.Client.Send(commands.old_client.PetHeroActivationCommand.write(pet.GetOwner().Id, pet.Id, 12, 1, pet.Name, (short)pet.FactionId, pet.Clan.Id, (short)pet.Level.Id, pet.Clan.Tag, pet.Position.X, pet.Position.Y, pet.Speed).Bytes);
            }
        }
#endregion

        #region PetStatusCommand

        public void PetStatusCommand(GameSession gameSession, Pet pet)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.PetStatusCommand.write(pet.Id, pet.Level.Id, pet.Experience, 1000, pet.CurrentHealth,
                pet.MaxHealth, pet.CurrentShield, pet.MaxShield, pet.Fuel, 50000, pet.Speed, pet.Name).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.PetStatusCommand.write(pet.Id, pet.Level.Id, pet.Experience, 1000, pet.CurrentHealth,
                    pet.MaxHealth, pet.CurrentShield, pet.MaxShield, pet.Fuel, 50000, pet.Speed, pet.Name).Bytes);
            }
        }

        #endregion

        #region CreateBoxCommand

        public void CreateBoxCommand(GameSession gameSession, Collectable collectable)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.CreateBoxCommand.write(collectable.Type.ToString(), new commands.new_client.BoxModule(collectable.Hash, collectable.Position.X, collectable.Position.Y)).Bytes);
            }
            else
            {
                LegacyModule(gameSession, "0|c|" + collectable.Hash + "|" + collectable.Type.GetHashCode().ToString() + "|" + collectable.Position.ToPacket());
            }
        }

        #endregion

        #region BeaconCommand

        public void BeaconCommand(GameSession gameSession)
        {
            var player = gameSession.Player;
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.BeaconCommand.write(0,0,0,0,
                    player.State.InDemiZone, player.Controller.Repairing, player.Controller.Repairing, player.Equipment.GetRobot(), player.State.InRadiationArea).Bytes);
            }
            else
            {
                LegacyModule(gameSession, "0|" + commands.ServerCommands.BEACON + "|0|0|" + Convert.ToInt32(player.State.InDemiZone) + "|0|"
                                          + Convert.ToInt32(player.State.InTradeArea) + "|"
                                          + Convert.ToInt32(player.State.InRadiationArea) + "|" + Convert.ToInt32(player.State.InPortalArea) + "|0");
                LegacyModule(gameSession, "0|A|RS|S|" + player.Controller.Repairing);
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
                new Hotkey(Hotkey.ACTIVATE_LASER, (int) Keys.ControlKey,newClient),
                new Hotkey(Hotkey.CHANGE_CONFIG, (int) Keys.C,newClient),
                new Hotkey(Hotkey.JUMP, (int) Keys.J,newClient),
                new Hotkey(Hotkey.LAUNCH_ROCKET, (int) Keys.Space,newClient),
                new Hotkey(Hotkey.PERFORMANCE_MONITORING, (int) Keys.F,newClient),
                new Hotkey(Hotkey.PET_ACTIVATE, (int) Keys.E,newClient),
                new Hotkey(Hotkey.PET_GUARD_MODE, (int) Keys.R,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D1, 0,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D2, 1,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D3, 2,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D4, 3,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D5, 4,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D6, 5,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D7, 6,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D8, 7,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D9, 8,newClient),
                new Hotkey(Hotkey.QUICKSLOT, (int) Keys.D0, 9,newClient),
                new Hotkey(Hotkey.TOGGLE_WINDOWS, (int) Keys.H,newClient),
                new Hotkey(Hotkey.LOGOUT, (int) Keys.L,newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F1, 0,newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F2, 1,newClient),
                new Hotkey(Hotkey.QUICKSLOT_PREMIUM, (int) Keys.F3, 2,newClient),
            };

            var keys = hotkeys.Select(hotkey => hotkey.Object).ToList();

            if (newClient)
                gameSession.Client.Send(commands.new_client.UserKeyBindingsUpdate.write(keys, false));
            else gameSession.Client.Send(commands.old_client.UserKeyBindingsUpdate.write(keys, false));
        }

        #endregion

        #region MapAddPOICommand

        public void MapAddPOICommand(GameSession gameSession, POI poi)
        {
            if (gameSession.Player.UsingNewClient)
            {
                 gameSession.Client.Send(commands.new_client.MapAddPOICommand.write(poi.Id, new commands.new_client.POITypeModule((short)poi.Type), poi.TypeSpecification,
                     new commands.new_client.POIDesignModule((short)poi.Design), (short)poi.Shape, poi.ShapeCordsToInts(), poi.Inverted, poi.Active));
            }
            else
            {
                gameSession.Client.Send(commands.old_client.MapAddPOICommand.write(poi.Id, new commands.old_client.POITypeModule((short)poi.Type), poi.TypeSpecification,
                    new commands.old_client.POIDesignModule((short)poi.Design), (short)poi.Shape, poi.ShapeCordsToInts(), poi.Inverted, poi.Active));
                gameSession.Client.Send(commands.old_client.POIReadyCommand.write());
            }
        }

        #endregion

        #region VideoWindowCreateCommand

        public void VideoWindowCreateCommand(GameSession gameSession, int windowID, string windowAlign, bool showButtons, List<string> languageKeys,
            int videoID, short videoType)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.VideoWindowCreateCommand.write(windowID, windowAlign,
                    showButtons, languageKeys, videoID, videoType));
            }
            else
            {
                gameSession.Client.Send(commands.old_client.VideoWindowCreateCommand.write(windowID, windowAlign, showButtons, languageKeys, videoID, videoType));
            }
        }

        #endregion

        #region ShipWarpWindowCreateCommand

        public void ShipWarpWindowCreateCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                var ships = new List<commands.new_client.ShipWarpModule>();

                foreach (var hangar in gameSession.Player.Equipment.Hangars)
                {
                    ships.Add(new commands.new_client.ShipWarpModule(hangar.Value.Ship.Id, hangar.Value.Ship.ToStringLoot(), hangar.Value.Ship.Name, 0, 0, hangar.Key, hangar.Value.Ship.Name));
                }

                gameSession.Client.Send(commands.new_client.ShipWarpWindowCommand.write(0, (int)gameSession.Player.Information.Uridium.Get(), gameSession.Player.State.InEquipmentArea, ships).Bytes);
            }
            else
            {
                var ships = new List<commands.old_client.ShipWarpModule>();

                foreach (var hangar in gameSession.Player.Equipment.Hangars)
                {
                    ships.Add(new commands.old_client.ShipWarpModule(hangar.Value.Ship.Id, hangar.Value.Ship.Id, hangar.Value.Ship.Name, 0, 0, hangar.Key, hangar.Value.Ship.Name));
                }

                gameSession.Client.Send(commands.old_client.ShipWarpWindowCommand.write(0, (int)gameSession.Player.Information.Uridium.Get(), gameSession.Player.State.InEquipmentArea, ships).Bytes);
            }
        }
        #endregion
        
        #region MapAssetActionAvailableCommand

        public void MapAssetActionAvailableCommand(GameSession gameSession, Object obj, bool clickable)
        {
            var player = gameSession.Player;
            if (player.UsingNewClient)
            {
                short toShort = (short)(clickable ? 0 : 1);
                Console.WriteLine(toShort.ToString());
                gameSession.Client.Send(commands.new_client.MapAssetActionAvailableCommand.write(obj.Id, toShort, true, 
                    new commands.new_client.ClientUITooltip(new List<commands.new_client.ClientUITooltipTextFormat>()), new commands.new_client.commandu1C()).Bytes);
            }
        }
        #endregion

        #region MineCreateCommand

        public void MineCreateCommand(GameSession gameSession, string hash, int mineType, Vector pos, bool pulse)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.new_client.MineCreateCommand.write(hash, pulse, false, mineType, pos.Y, pos.X).Bytes);
            }
            else
            {
                LegacyModule(gameSession, $"0|L|asdf|{mineType}|{gameSession.Player.Position.X}|{gameSession.Player.Position.Y}|{Convert.ToInt32(pulse)}|0");
            }
        }

        #endregion

        #region BattleStationNoClanUiInitializationCommand

        public void BattleStationNoClanUiInitializationCommand(GameSession gameSession, Asteroid asteroid)
        {
            if (!gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(commands.old_client.BattleStationNoClanUiInitializationCommand.write(asteroid.Id).Bytes);
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
                gameSession.Client.Send(commands.new_client.AddOreCommand.write(new commands.new_client.BoxModule(ore.Hash, ore.Position.X, ore.Position.Y), new commands.new_client.OreTypeModule((short)ore.Type)).Bytes);
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AddOreCommand.write(ore.Hash, new commands.old_client.OreTypeModule((short)ore.Type), ore.Position.X, ore.Position.Y).Bytes);
            }
        }
        #endregion

        #region PetGearAddCommand

        public void PetGearAddCommand(GameSession gameSession, Gear gear)
        {
            if (gameSession.Player.UsingNewClient)
            {
                throw new NotImplementedException();
            }
            else
            {
                Console.WriteLine("Gear->" + gear.Type.ToString());
                gameSession.Client.Send(netty.commands.old_client.PetGearAddCommand.write(new commands.old_client.PetGearTypeModule((short)gear.Type), gear.Level, gear.Amount, gear.Enabled).Bytes);
            }
        }
        #endregion

        #region PetGearSelectCommand

        public void PetGearSelectCommand(GameSession gameSession, Gear gear)
        {
            if (gameSession.Player.UsingNewClient)
            {
                throw new NotImplementedException();
            }
            else
            {
                Console.WriteLine("Select Gear->" + gear.Type.ToString());
                gameSession.Client.Send(commands.old_client.PetGearSelectCommand.write(new commands.old_client.PetGearTypeModule((short)gear.Type), new List<int>()).Bytes);
            }
        }

        #endregion

        #region MapChangeCommand

        public void MapChangeCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
                throw new NotImplementedException();
            else
                LegacyModule(gameSession, "0|z");
        }
        #endregion

        #region PetDeactivationCommand

        public void PetDeactivationCommand(GameSession gameSession, Pet pet)
        {
            if (gameSession.Player.UsingNewClient)
                throw new NotImplementedException();
            else
                gameSession.Client.Send(commands.old_client.PetDeactivationCommand.write(pet.Id).Bytes);
        }

        #endregion

        #region LevelUpCommand
        public void LevelUpCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                throw new NotImplementedException();
            }
            else
            {
                gameSession.Client.Send(commands.old_client.LevelUpCommand.write(gameSession.Player.Id, gameSession.Player.Information.Level.Id).Bytes);
            }
        }
        #endregion
        #region AmmunitionCountUpdateCommand

        public void AmmunitionCountUpdateCommand(GameSession gameSession, string lootId, int amount)
        {
            if (gameSession.Player.UsingNewClient)
            {
                throw new NotImplementedException();
            }
            else
            {
                gameSession.Client.Send(commands.old_client.AmmunitionCountUpdateCommand.write(new List<commands.old_client.AmmunitionCountModule>(){new commands.old_client.AmmunitionCountModule(Converter.ToAmmoType(lootId), amount)}).Bytes);
            }
        }
        #endregion

        #region HellstormStatusCommand
        public void HellstormStatusCommand(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                throw new NotImplementedException();
            }
            else
            {
                gameSession.Client.Send(commands.old_client.HellstormStatusCommand.write(new List<int>(), new commands.old_client.AmmunitionTypeModule(commands.old_client.AmmunitionTypeModule.ECO_ROCKET), 0).Bytes);
            }
        }
        #endregion

    }
}
