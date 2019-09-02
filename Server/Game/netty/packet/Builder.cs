using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using Server.Game.objects.implementable;
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
            PrebuiltLegacyCommands.Instance.AddCommands();
            PrebuiltPlayerCommands.Instance.AddCommands();
            PrebuiltRangeCommands.Instance.AddCommands();
            PrebuiltCombatCommands.Instance.AddCommands();
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

            BuildCommand(client, Commands.LEGACY_MODULE, usingNewClient, packetBuilder.ToString());
        }

        public void BuildToRange(AbstractAttackable parent, Commands key, object[] oldClientParameters, object[] newClientParameters)
        {
            foreach (var character in parent.Spacemap.Entities)
            {
                if (!(character.Value is Player player)) continue;
                
                var session = GameStorageManager.Instance.FindSession(player);
                if (player.UsingNewClient)
                {
                    BuildCommand(session.GameClient, key, true, newClientParameters);
                }
                else
                {
                    BuildCommand(session.GameClient, key, false, oldClientParameters);
                }
            }
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