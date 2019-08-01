using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Server.Game.managers;
using Server.Game.netty.commands;
using Server.Game.netty.commands.old_client;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Networking;
using Server.Networking.clients;
using Server.Utils;

namespace Server.Game.netty.packet
{
    class Builder
    {
        public Dictionary<Commands, Action<GameClient, object[]>> OldCommands = new Dictionary<Commands, Action<GameClient, object[]>>();
        public Dictionary<Commands, Action<GameClient, object[]>> NewCommands = new Dictionary<Commands, Action<GameClient, object[]>>();
        
        public void AddCommands()
        {
            OldCommands.Add(Commands.LEGACY_MODULE,
                async (client, actionParams) =>
                {
                    Console.WriteLine(actionParams[0]);
                    await client.Send(commands.old_client.LegacyModule.write((string) actionParams[0]).Bytes);
                });
            NewCommands.Add(Commands.LEGACY_MODULE, (client, actionParams) => throw new NotImplementedException());

            
            OldCommands.Add(Commands.SHIP_INITIALIZATION_COMMAND, async (client, actionParams) =>
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

            OldCommands.Add(Commands.USER_SETTINGS_COMMAND, async (client, actionParams) =>
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

            OldCommands.Add(Commands.HOTKEYS_COMMAND, async (client, actionParams) =>
                {
                    await client.Send(new commands.old_client.UserKeyBindingsUpdate(new List<UserKeyBindingsModule>(), false).write());
                });
            
            OldCommands.Add(Commands.SHIP_SETTINGS_COMMAND, async (client, actionParams) =>
                {
                    ArgumentFixer(actionParams, 5, out actionParams);

                    await client.Send(new commands.old_client.ShipSettingsCommand(Convert.ToString(actionParams[0]),
                        Convert.ToString(actionParams[1]), Convert.ToInt32(actionParams[2]),
                        Convert.ToInt32(actionParams[3]), Convert.ToInt32(actionParams[4])).write().Bytes);
                });
            
            Out.QuickLog($"Successfully added {OldCommands.Count} old client commands to Builder");
            Out.QuickLog($"Successfully added {NewCommands.Count} new client commands to Builder");
        }
        
        public void BuildCommand(GameClient client, Commands key, bool usingNewClient, params object[] parameters)
        {
            var player = client.AttachedPlayer;
            if (player == null)
            {
                Out.WriteLog("Invalid GameSession, client userid is invalid", LogKeys.ERROR_LOG);
                throw new ConstraintException("No valid GameSession found, client is attached to nothing, trying to send packets to nowhere");   
            }

            if (CharacterStateManager.Instance.IsInState(player, CharacterStates.NO_CLIENT_CONNECTED))
            {
                Out.WriteLog("Nothing to send");
                return;
            }
            
            if (!usingNewClient)
            {
                //OLD COMMAND
                if (OldCommands.ContainsKey(key))
                {
                    OldCommands[key].Invoke(client, parameters);
                }
                else
                {
                    Out.WriteLog("Command key " + key + " not found on the given instance of Builder", LogKeys.ERROR_LOG);
                }
            }
            else
            {
                // NEW COMMAND
                NewCommands[key].Invoke(client, parameters);
            }
        }

        public void BuildLegacyCommand(GameClient client, bool usingNewClient, params object[] parameters)
        {
            var packetBuilder = new StringBuilder();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                if (parameter is int || parameter is double || parameter is float ||
                    parameter is string || parameter is char || parameter is long ||
                    parameter is short)
                {
                    Console.WriteLine("Build parameter: " + parameter);
                    if (i == parameters.Length - 1)
                    {
                        packetBuilder.Append(parameter);
                    }
                    else
                    {
                        packetBuilder.Append(parameter).Append("|");
                    }
                }
                else throw new ArgumentException("Not allowed argument");
            }

            Console.WriteLine(packetBuilder);
            BuildCommand(client, Commands.LEGACY_MODULE, usingNewClient, packetBuilder.ToString());
        }

        public void BuildToRange(GameSession parent, Commands key, object[] oldClientParameters, object[] newClientParameters)
        {
            throw new NotImplementedException();
        }

        public void BuildToSpacemap(Spacemap map, Commands key, object[] oldClientParameters, object[] newClientParameters)
        {
            throw new NotImplementedException();
        }

        public void BuildToSelectedCharacter(GameSession parent, Commands key, bool selfSend, object[] oldClientParameters, object[] newClientParameters)
        {
            throw new NotImplementedException();
        }

        public void BuildToAllConnections(Commands key, object[] oldClientParameters, object[] newClientParameters)
        {
            
        }
    }
}