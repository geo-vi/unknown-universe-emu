using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedBrowser.Game.netty.commands;
using NettyBaseReloadedBrowser.Game.netty.handlers;
using NettyBaseReloadedBrowser.Game.netty.requests;
using NettyBaseReloadedBrowser.Game.netty.clientResponses;
using NettyBaseReloadedBrowser.Utils;

namespace NettyBaseReloadedBrowser.Game.netty
{
    class CommandHandler
    {
        public static Dictionary<int, IHandler> HandledCommands = new Dictionary<int, IHandler>();
        public static Dictionary<int, IResponse> HandledResponses = new Dictionary<int, IResponse>();

        public static void AddCommands()
        {
            HandledCommands.Add(ShipInitializationRequest.ID, new ShipInitializationHandler());
            HandledResponses.Add(clientResponses.ShipInitializationCommand.ID, new clientResponses.ShipInitializationCommand());
        }

        public static void Handle(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            if (HandledCommands.ContainsKey(parser.CMD_ID))
                HandledCommands[parser.CMD_ID].execute(parser);
            else Debug.WriteLine("{0} - Undefined command [ID: {1}]", DateTime.Now, parser.CMD_ID);
        }

        public static void HandleResponse(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            if (HandledResponses.ContainsKey(parser.CMD_ID))
                HandledResponses[parser.CMD_ID].execute();
            else Debug.WriteLine("{0} - Undefined response [ID: {1}]", DateTime.Now, parser.CMD_ID);
        }
    }
}
