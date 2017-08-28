using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.handlers;
using NettyBaseReloaded.Game.netty.newcommands;
using NettyBaseReloaded.Networking;
using NettyBaseReloaded.Utils;
using LegacyModule = NettyBaseReloaded.Game.netty.commands.LegacyModule;

namespace NettyBaseReloaded.Game.netty
{
    class CommandHandler
    {
        public static Dictionary<short, IHandler> HandledCommands = new Dictionary<short, IHandler>();
        public static Dictionary<short, IHandler> HandledNewClientCommands = new Dictionary<short, IHandler>();
        public static Dictionary<string, ILegacyHandler> HandledLegacyCommands = new Dictionary<string, ILegacyHandler>();

        public static void AddCommands()
        {
            HandledCommands.Add(MoveRequest.ID, new MoveHandler());
            HandledCommands.Add(ShipSelectRequest.ID, new ShipSelectionHandler());
            HandledCommands.Add(SelectBatteryRequest.ID, new SelectBatteryHandler());
            HandledCommands.Add(SelectRocketRequest.ID, new SelectRocketHandler());
            HandledCommands.Add(AttackLaserRequest.ID, new AttackLaserRunHandler());
            HandledCommands.Add(DroneFormationChangeRequest.ID, new DroneFormationChangeHandler());
            HandledNewClientCommands.Add(MovementRequest.ID, new MoveHandler());
            HandledLegacyCommands.Add(ClientCommands.LASER_STOP, new AttackAbortLaserHandler());
            HandledLegacyCommands.Add(ServerCommands.ROCKET_ATTACK, new AttackRocketHandler());
            HandledLegacyCommands.Add(ClientCommands.PORTAL_JUMP, new JumpGateHandler());
            HandledLegacyCommands.Add(ClientCommands.SELECT, new SelectHandler());
        }

        public static void Handle(byte[] bytes, GameClient client)
        {
            var parser = new ByteParser(bytes);

            if (parser.CMD_ID == 666)
            {
                new ShipInitalizationHandler(client, parser.Int(), parser.UTF());
                return;
            }

            if (parser.CMD_ID == newcommands.ShipInitializationRequest.ID)
            {
                // Command received => trying to read = #bug 
                var simpleCmd = new SimpleCommand(bytes); 
                var cmd = new newcommands.ShipInitializationRequest(simpleCmd);
                cmd.readShort();
                cmd.readCommand();
                new ShipInitalizationHandler(client, cmd.playerId, cmd.sessionId, true);
                return;
            }

            var gameSession = World.StorageManager.GetGameSession(client.UserId);

            if (gameSession != null)
            {
                if (gameSession.Player.UsingNewClient)
                {
                    if (HandledNewClientCommands.ContainsKey(parser.CMD_ID))
                        HandledNewClientCommands[parser.CMD_ID].execute(gameSession, bytes);
                }
                else
                {
                    if (HandledCommands.ContainsKey(parser.CMD_ID))
                        HandledCommands[parser.CMD_ID].execute(gameSession, bytes);
                }

                if (parser.CMD_ID == LegacyModule.ID || parser.CMD_ID == newcommands.LegacyModule.ID)
                {
                    var packet = parser.UTF();
                    if (packet.Contains('|'))
                    {
                        var splittedPacket = packet.Split('|');
                        if (HandledLegacyCommands.ContainsKey(splittedPacket[0]))
                            HandledLegacyCommands[splittedPacket[0]].execute(gameSession, splittedPacket);
                    }
                    else
                    {
                        if (HandledLegacyCommands.ContainsKey(packet))
                            HandledLegacyCommands[packet].execute(gameSession, new []{packet});
                    }
                }
            }
        }
    }
}
