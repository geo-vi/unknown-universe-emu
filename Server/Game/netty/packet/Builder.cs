using System;
using System.Collections.Generic;
using Server.Game.netty.commands;
using Server.Game.objects;
using Server.Networking;

namespace Server.Game.netty.packet
{
    class Builder
    {
        public Dictionary<Commands, Action<GameClient, object[]>> OldCommands = new Dictionary<Commands, Action<GameClient, object[]>>();
        public Dictionary<Commands, Action<GameClient, object[]>> NewCommands = new Dictionary<Commands, Action<GameClient, object[]>>();

        public void AddCommands()
        {
            OldCommands.Add(Commands.LegacyModule, async(client, actionParams) => { await client.Send(commands.new_client.LegacyModule.write((string)actionParams[0]).Bytes); });
            NewCommands.Add(Commands.LegacyModule, (client, actionParams) => throw new NotImplementedException());
        }
        
        public void BuildCommand(GameClient client, Commands key, bool oldClient, params object[] parameters)
        {
            if (oldClient)
            {
                OldCommands[key].Invoke(client, parameters);
            }
            else
            {
                NewCommands[key].Invoke(client, parameters);
            }
        }

        public void BuildToRange(GameSession parent, Commands key, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void BuildToSpacemap(Spacemap map, Commands key, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void BuildToSelectedCharacter(GameSession parent, Commands key, bool selfSend, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}