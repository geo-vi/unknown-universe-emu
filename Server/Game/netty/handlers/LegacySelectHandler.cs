using System;
using NettyBaseReloaded.Game.netty.commands;
using Server.Game.controllers.characters;
using Server.Game.controllers.players;
using Server.Game.managers;
using Server.Game.netty.commands;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.enums;
using Server.Game.objects.errors;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class LegacySelectHandler : ILegacyHandler
    {
        private Player _player;

        public void Execute(GameSession gameSession, string[] packet)
        {
            _player = gameSession.Player;
            
            switch (packet[0])
            {
                case ServerCommands.TECHS:
                    if (!int.TryParse(packet[1], out var techId) || techId == 0)
                    {
                        Out.WriteLog("Using non-client parameters", LogKeys.PLAYER_LOG, _player.Id);
                        break;
                    }

                    SelectTech((Techs) techId);
                    break;
                default:
                    switch (packet[1])
                    {
                        case ServerCommands.EMP:
                            GameItemManager.Instance.Use(_player, "ammunition_specialammo_emp-01");
                            break;
                        case ServerCommands.SMARTBOMB:
                            GameItemManager.Instance.Use(_player, "ammunition_mine_smb-01");
                            break;
                        case ServerCommands.INSTASHIELD:
                            GameItemManager.Instance.Use(_player, "equipment_extra_cpu_ish-01");
                            break;
                        case ClientCommands.ROBOT:
                            GameItemManager.Instance.ActivateRobot(_player);
                            break;
                        case ClientCommands.SELECT_CLOAK:
                            GameItemManager.Instance.ActivateCloak(_player);
                            break;
                        case ClientCommands.AROL:
                            GameItemManager.Instance.Activate(_player, "equipment_extra_cpu_arol-x");
                            break;
                        case ClientCommands.RLLB:
                            GameItemManager.Instance.Activate(_player, "equipment_extra_cpu_rllb-x");
                            break;
                        case ServerCommands.JUMP_CPU:
                            switch (packet[2])
                            {
                                case "1":
                                    GameItemManager.Instance.Activate(_player, "equipment_extra_cpu_jp-01");
                                    break;
                                case "2":
                                    GameItemManager.Instance.Activate(_player, "equipment_extra_cpu_jp-02");
                                    break;
                            }

                            break;
                        case ClientCommands.CONFIGURATION:
                            SwitchConfiguration();
                            break;
                        case ClientCommands.MINE:
                            switch (packet[2])
                            {
                                case ServerCommands.MINE_ACM:
                                    GameItemManager.Instance.Use(_player, "ammunition_mine_acm-01");
                                    break;
                                case ServerCommands.MINE_SAB:
                                    GameItemManager.Instance.Use(_player, "ammunition_mine_sabm-01");
                                    break;
                                case ServerCommands.MINE_EMP:
                                    GameItemManager.Instance.Use(_player, "ammunition_mine_empm-01");
                                    break;
                                case ServerCommands.MINE_DDM:
                                    GameItemManager.Instance.Use(_player, "ammunition_mine_ddm-01");
                                    break;
                                case ServerCommands.MINE_SLM:
                                    GameItemManager.Instance.Use(_player, "ammunition_mine_slm-01");
                                    break;
                            }

                            break;
                        case ServerCommands.FIREWORKS:
                            switch (packet[2])
                            {
                                case "1":
                                    GameItemManager.Instance.Use(_player, "ammunition_firework_fwx-s");
                                    break;
                                case "2":
                                    GameItemManager.Instance.Use(_player, "ammunition_firework_fwx-m");
                                    break;
                                case "3":
                                    GameItemManager.Instance.Use(_player, "ammunition_firework_fwx-l");
                                    break;
                            }

                            break;
                        case ServerCommands.FIREWORKS_IGNITE:
                            GameItemManager.Instance.ActivateFireworks(_player);
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// Switching configurations
        /// </summary>
        private void SwitchConfiguration()
        {
            if (_player.LastConfigurationChange.AddSeconds(3) > DateTime.Now)
            {
                if (_player.Settings.GetSettings<GameplaySettings>().DisplayConfigurationChanges)
                {
                    PrebuiltLegacyCommands.Instance.ServerMessage(_player,
                        "Time left for configuration switch: " +
                        Convert.ToInt32(3000 + (_player.LastConfigurationChange - DateTime.Now).TotalMilliseconds) + "ms");
                }

                return;
            }
            
            _player.Controller.GetInstance<PlayerConfigurationController>().Switch();
        }

        private void SelectTech(Techs tech)
        {
            //todo: create techs
        }
    }
}