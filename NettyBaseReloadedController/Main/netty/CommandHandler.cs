using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Main.netty.commands;
using NettyBaseReloadedController.Main.netty.handlers;
using NettyBaseReloadedController.Networking;
using NettyBaseReloadedController.Utils;

namespace NettyBaseReloadedController.Main.netty
{
    class CommandHandler
    {
        public static Dictionary<short, IHandler> HandledCommands = new Dictionary<short, IHandler>();

        public static void AddCommands()
        {
            HandledCommands.Add(LoginCommand.ID, new LoginCommandHandler());
            HandledCommands.Add(CharacterInfoCommand.ID, new CharacterInfoCommandHandler());
            HandledCommands.Add(PlayerInfoCommand.ID, new PlayerDataCommandHandler());
        }

        public static void Handle(byte[] bytes, ControllerClient client)
        {
            var parser = new ByteParser(bytes);
            
            Debug.WriteLine(parser.CMD_ID);

            if (HandledCommands.ContainsKey(parser.CMD_ID))
                HandledCommands[parser.CMD_ID].execute(client, parser);
        }

    }
}
