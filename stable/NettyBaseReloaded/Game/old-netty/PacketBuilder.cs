using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.newcommands;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.players;
using VisualModifierCommand = NettyBaseReloaded.Game.netty.commands.VisualModifierCommand;

namespace NettyBaseReloaded.Game.netty
{
    static class PacketBuilder
    {
        public static byte[] ShipInitializationCommand(Player player)
        {
            if (player.UsingNewClient)
            {
                return new newcommands.ShipInitializationCommand(
                    player.Id,
                    player.Name,
                    player.Hangar.Ship.ToStringLoot(),
                    player.Speed,
                    player.CurrentShield,
                    player.MaxShield,
                    player.CurrentHealth,
                    player.MaxHealth,
                    0, //freeCargo
                    4000, //maxCargo
                    player.CurrentNanoHull,
                    player.MaxNanoHull,
                    player.Position.X,
                    player.Position.Y,
                    player.Spacemap.Id,
                    (int) player.FactionId,
                    1, //idk
                    3, //idk
                    player.Premium,
                    player.Experience,
                    player.Honor,
                    player.Level.Id,
                    player.Credits,
                    player.Uridium,
                    player.Jackpot,
                    (int) player.RankId,
                    "PX", //clanTag
                    player.Rings,
                    true,
                    false, //cloaked
                    true,
                    new List<newcommands.VisualModifierCommand>()
                ).getBytes();
            }
            return commands.ShipInitializationCommand.write(
                player.Id,
                player.Name,
                player.Hangar.Ship.Id,
                player.Speed,
                player.CurrentShield,
                player.MaxShield,
                player.CurrentHealth,
                player.MaxHealth,
                player.Cargo.Free(player.Hangar.Ship.Cargo),
                player.Hangar.Ship.Cargo, player.CurrentNanoHull,
                player.MaxNanoHull, player.Position.X,
                player.Position.Y,
                player.Spacemap.Id,
                (int) player.FactionId,
                player.Clan.Id,
                0,
                0,
                player.LaserCount(),
                player.Premium,
                player.Experience,
                player.Honor,
                player.Level.Id,
                player.Credits,
                player.Uridium,
                player.Jackpot,
                (int) player.RankId,
                player.Clan.Tag,
                0,
                true,
                false,
                new List<VisualModifierCommand>());
        }

        public static Command ShipCreateCommand(Character character, bool newClient = false)
        {
            if (!newClient)
            {
                if (character is Player)
                {
                    var player = (Player) character;

                    return new Command(commands.ShipCreateCommand.write(
                        player.Id,
                        player.Hangar.Ship.Id,
                        player.Hangar.Configurations[player.CurrentConfig].LaserCount,
                        player.Clan.Tag,
                        player.Name,
                        player.Position.X,
                        player.Position.Y,
                        (int) player.FactionId,
                        player.Clan.Id,
                        (int) player.RankId,
                        false, //warnIcon
                        new commands.ClanRelationModule(commands.ClanRelationModule.NONE),
                        player.Rings,
                        true,
                        false, //npc
                        false, //player.Controller.Invisible, //cloaked
                        0,
                        0, //idk
                        player.VisualModifier
                    ), false);
                }
                return new Command(commands.ShipCreateCommand.write(
                    character.Id,
                    character.Hangar.Ship.Id,
                    0, //idk
                    "", //clanTag
                    character.Name,
                    character.Position.X,
                    character.Position.Y,
                    (int) character.FactionId,
                    -1, //idk
                    0, //rankId
                    false, //warnIcon
                    new commands.ClanRelationModule(commands.ClanRelationModule.NONE),
                    0, //Rings
                    true,
                    true, //npc
                    false, //character.Controller.Invisible, //cloaked
                    0, //idk
                    0,
                    character.VisualModifier), false);
            }
            if (character is Player)
            {
                var player = (Player) character;

                return new Command(new newcommands.CreateShipCommand(
                    player.Id,
                    player.Hangar.Ship.ToStringLoot(),
                    player.LaserCount(), //idk
                    player.Clan.Tag,
                    player.Name,
                    player.Position.X,
                    player.Position.Y,
                    (int) player.FactionId,
                    player.Clan.Id, //idk
                    (int) player.RankId,
                    false, //warnIcon
                    new newcommands.ClanRelationModule(newcommands.ClanRelationModule.NONE),
                    player.Rings,
                    true,
                    false, //npc
                    false, //cloaked
                    0, //idk
                    0,
                    new List<newcommands.VisualModifierCommand>(),
                    new newcommands.commandK13(newcommands.commandK13.DEFAULT)
                ).getBytes(), true);

            }
            return new Command(new newcommands.CreateShipCommand(
                character.Id,
                character.Hangar.Ship.ToStringLoot(),
                0, //idk
                "",
                character.Name,
                character.Position.X,
                character.Position.Y,
                (int) character.FactionId,
                0, //idk
                0,
                false, //warnIcon
                new newcommands.ClanRelationModule(newcommands.ClanRelationModule.NONE),
                0,
                true,
                true, //npc
                false, //cloaked
                0, //idk
                0,
                new List<newcommands.VisualModifierCommand>(),
                new newcommands.commandK13(newcommands.commandK13.DEFAULT)
            ).getBytes(), true);
        }



        public static byte[] UserSettingsCommand(Player player, bool newClient = false)
        {
            if (newClient)
            {
                var qs = new newcommands.settingsModules.QualitySettingsModule(false, 3, 3, 3, false, 3, 3, 3, 3, 3, 3);
                var asm = new newcommands.settingsModules.AudioSettingsModule(false, 50, 0, 50, false);
                var ws = new newcommands.settingsModules.WindowSettingsModule(8, "23,1|24,1|25,1|27,1|26,1|100,1|34,1|36,1|33,1|35,1|39,1|38,1|37,1|32,1|", false);
                var gm = new newcommands.settingsModules.GameplaySettingsModule(false, false, false, false, false, true, false, false, false, false);
                var z9 = new newcommands.settingsModules.QuestSettingsModule(false, true, true, false, false, false);
                var ds = new newcommands.settingsModules.DisplaySettingsModule(true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, 3, 4, 4, 3, 3, 4, 3, 3, true, true, true, true);
                return new newcommands.UserSettingsCommand(qs, asm, ws, gm, z9, ds).getBytes();
            }
            return commands.UserSettingsCommand.write(player.Settings.QualitySettingsModule, player.Settings.DisplaySettingsModule, player.Settings.AudioSettingsModule,
                player.Settings.WindowSettingsModule, player.Settings.GameplaySettingsModule);
        }

        public static byte[] NewClientWindowsCommand()
        {
            var topLeftWindows = new List<ClientWindow>
            {
                new ClientWindow("cli", "title_cli", false, ClientWindow.RED),
                new ClientWindow("user", "title_user"),
                new ClientWindow("ship", "title_ship"),
                new ClientWindow("ship_warp", "ttip_shipWarp_btn"),
                new ClientWindow("chat", "title_chat"),
                new ClientWindow("group", "title_group"),
                new ClientWindow("minimap", "title_map"),
                new ClientWindow("spacemap", "title_spacemap"),
                new ClientWindow("quests", "title_quests"),
                new ClientWindow("refinement", "title_refinement", false),
                new ClientWindow("log", "title_log"),
                new ClientWindow("pet", "title_pet", false),
                new ClientWindow("contacts", "title_contacts", false),
                new ClientWindow("booster", "title_booster", false),
                new ClientWindow("spaceball", "title_spaceball", false),
                new ClientWindow("invasion", "title_invasion", false),
                new ClientWindow("ctb", "title_ctb", false),
                new ClientWindow("tdm", "title_tdm", false),
                new ClientWindow("ranked_hunt", "title_ranked_hunt", false),
                new ClientWindow("highscoregate", "title_highscoregate", false),
                new ClientWindow("scoreevent", "title_scoreevent", false),
                new ClientWindow("infiltration", "title_ifg", false),
                new ClientWindow("word_puzzle", "title_wordpuzzle", false),
                new ClientWindow("sectorcontrol", "title_sectorcontrol_ingame_status", false),
                new ClientWindow("jackpot_status_ui", "title_jackpot_status_ui", false),
                new ClientWindow("curcubitor", "httip_countdownHalloweenNPCs", false),
                new ClientWindow("influence", "httip_influence", false),
                new ClientWindow("traininggrounds", "title_traininggrounds", false),
                new ClientWindow("spaceplague", "title_spaceplague", false)
            };

            var topRightWindows = new List<ClientWindow>
            {
                new ClientWindow("shop", "title_shop", false),
                new ClientWindow("fullscreen", "ttip_fullscreen_btn"),
                new ClientWindow("settings", "title_settings"),
                new ClientWindow("help", "title_help", false),
                new ClientWindow("logout", "title_logout")
            };

            //Foreach window adds it to a list
            var topLeftButtons = topLeftWindows.Select(window => window.Window).ToList();
            var topRightButtons = topRightWindows.Select(window => window.Window).ToList();

            var slotbars = new List<newcommands.windowModules.commandKn>
            {
                new newcommands.windowModules.commandKn(newcommands.windowModules.commandKn.GAME_FEATURE_BAR, topLeftButtons, "0,0", "0"),
                new newcommands.windowModules.commandKn(newcommands.windowModules.commandKn.GENERIC_FEATURE_BAR, topRightButtons, "100,0", "0")
            };


            var command = new WindowsCommand(slotbars);
            command.write();

            return command.getBytes();
        }

        public static Command DronesCommand(Character character, bool newClient = false)
        {
            if (character.Hangar.Drones.Count <= 0) return LegacyModule("", newClient);

            var command = "0|n|d|" + character.Id + "|" + (int)character.Formation;

            foreach (var d in character.Hangar.Drones)
            {
                command += "|" + (int)d.DroneType + "|" + d.Level.Id + "|0";
            }
            return LegacyModule(command, newClient);
        }

        public static Command BigMessage(string message, bool newClient = false)
        {
            return LegacyModule("0|n|MSG|1|1|" + message + "|[{w:%MESSAGE%,v: " + message + "}]", newClient);
        }

        public static Command LegacyModule(string message, bool newClient = false)
        {
            if (newClient)
                return new Command(new newcommands.LegacyModule(message).getBytes(), true);
            return new Command(commands.LegacyModule.write(message), false);
        }

        public static byte[] SpeedUpdateCommand(int newSpeed)
        {
            return AttributeShipSpeedUpdateCommand.write(newSpeed);
        }

        public static byte[] ShieldUpdateCommand(int shieldNow, int shieldMax)
        {
            return AttributeShieldUpdateCommand.write(shieldNow, shieldMax);
        }

        public static Command MoveCommand(int entityId, Vector destination, int time, bool newClient = false)
        {
            if (!newClient)
                return new Command(commands.MoveCommand.write(
                    entityId,
                    destination.X,
                    destination.Y,
                    time
                ), false);
            return new Command(new newcommands.MovementCommand(entityId,
                    destination.X,
                    destination.Y,
                    time).getBytes(), true);


        }

        public static byte[] ShipRemoveCommand(int userId)
        {
            return commands.ShipRemoveCommand.write(userId);
        }

        public static byte[] ShipSelectionCommand(Character character)
        {
            if (!(character is Player || character is Npc || character is Pet))
            {
                return commands.ShipSelectionCommand.write(
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                false
                );
            }
            return commands.ShipSelectionCommand.write(
                character.Id,
                character.Hangar.Ship.Id, //idk probably shipId for group
                character.CurrentShield,
                character.MaxShield,
                character.CurrentHealth,
                character.MaxHealth,
                character.CurrentNanoHull,
                character.MaxNanoHull,
                false //bubleshield
                );
        }

        public static byte[] ShipDeselectionCommand()
        {
            return commands.ShipDeselectionCommand.write();
        }

        public static byte[] CreateAsset(Asset asset)
        {
            return commands.AssetCreateCommand.write(new AssetTypeModule(asset.Type), asset.Name, asset.FactionId,
                asset.ClanTag, asset.Id, asset.DesignId,
                asset.ExpansionStage, asset.Position.X, asset.Position.Y, asset.ClanId, asset.Invisible, asset.VisibleOnWarnRadar,
                asset.DetectedByWarnRadar,
                asset.ClanRelation, asset.Modifiers);
        }

        public static Command AttackLaserRunCommand(int attackerId, int targetId, int laserColor, bool isDiminishedBySkillShield, bool skilledLaser, bool newClient = false)
        {
            if (!newClient)
            {
                return new Command(commands.AttackLaserRunCommand.write(attackerId, targetId, laserColor, isDiminishedBySkillShield, skilledLaser), false);
            }
            return new Command(new newcommands.LaserCommand(attackerId, targetId, laserColor, isDiminishedBySkillShield, skilledLaser).getBytes(), true);
        }
    }
}
