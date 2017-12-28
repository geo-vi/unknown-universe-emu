using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.handlers;
using NettyBaseReloaded.Networking;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.packet
{
    class Handler
    {
        public Dictionary<short, IHandler> OldClientCommands = new Dictionary<short, IHandler>();
        public Dictionary<short, IHandler> NewClientCommands = new Dictionary<short, IHandler>();
        public Dictionary<string, ILegacyHandler> LegacyCommands = new Dictionary<string, ILegacyHandler>();

        public void AddCommands()
        {
            NewClientCommands.Add(commands.new_client.requests.MoveRequest.ID, new MoveHandler());
            OldClientCommands.Add(commands.old_client.requests.MoveRequest.ID, new MoveHandler());
            NewClientCommands.Add(commands.new_client.requests.UIOpenRequest.ID, new UIOpenHandler());
            OldClientCommands.Add(commands.old_client.requests.ShipSelectionRequest.ID, new ShipSelectionHandler());
            NewClientCommands.Add(commands.new_client.requests.ShipSelectionRequest.ID, new ShipSelectionHandler());
            NewClientCommands.Add(commands.new_client.requests.ItemSelectionRequest.ID, new ItemSelectionHandler());
            OldClientCommands.Add(commands.old_client.requests.SelectBatteryRequest.ID, new ItemSelectionHandler());
            OldClientCommands.Add(commands.old_client.requests.SelectRocketRequest.ID, new ItemSelectionHandler());
            NewClientCommands.Add(commands.new_client.requests.AttackLaserRequest.ID, new AttackLaserHandler());
            OldClientCommands.Add(commands.old_client.requests.AttackLaserRequest.ID, new AttackLaserHandler());
            NewClientCommands.Add(commands.new_client.requests.AttackAbortLaserRequest.ID, new AttackAbortLaserHandler());
            LegacyCommands.Add(ClientCommands.LASER_STOP, new AttackAbortLaserHandler());
            NewClientCommands.Add(commands.new_client.requests.PetRequest.ID, new PetHandler());
            OldClientCommands.Add(commands.old_client.requests.PetRequest.ID, new PetHandler());
            NewClientCommands.Add(commands.new_client.requests.AttackRocketRequest.ID, new AttackRocketHandler());
            NewClientCommands.Add(commands.old_client.requests.AttackRocketRequest.ID, new AttackRocketHandler());
            LegacyCommands.Add(ClientCommands.SELECT, new LegacySelectHandler());
            LegacyCommands.Add(ServerCommands.REQUEST_SHIP, new ForceInitHandler());
            NewClientCommands.Add(commands.new_client.requests.ClickableRequest.ID, new ClickableHandler());
            NewClientCommands.Add(commands.new_client.requests.commandHF.ID, new command42JHandler());
            OldClientCommands.Add(commands.old_client.requests.CollectBoxRequest.ID, new CollectBoxHandler());
            NewClientCommands.Add(commands.new_client.requests.CollectBoxRequest.ID, new CollectBoxHandler());
            LegacyCommands.Add(ClientCommands.PORTAL_JUMP, new JumpRequestHandler());
            LegacyCommands.Add(ServerCommands.ROCKET_ATTACK, new AttackRocketLegacyHandler());
            OldClientCommands.Add(commands.old_client.requests.ShipWarpWindowRequest.ID, new ShipWarpWindowHandler()); 
            OldClientCommands.Add(commands.old_client.requests.PetGearActivationRequest.ID, new PetGearActivationHandler());
            OldClientCommands.Add(commands.old_client.requests.WindowSettingsRequest.ID, new WindowSettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.HellstormLoadRequest.ID, new HellstormLoadHandler());
            OldClientCommands.Add(commands.old_client.requests.HellstormLaunchRequest.ID, new HellstormLaunchHandler());
            OldClientCommands.Add(commands.old_client.requests.HellstormSelectRocketRequest.ID, new HellstormSelectRocketHandler());
            OldClientCommands.Add(commands.old_client.requests.ShipSettingsRequest.ID, new ShipSettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.DisplaySettingsRequest.ID, new DisplaySettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.QualitySettingsRequest.ID, new QualitySettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.AudioSettingsRequest.ID, new AudioSettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.GameplaySettingsRequest.ID, new GameplaySettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.LogoutRequest.ID, new LogoutHandler());
            OldClientCommands.Add(commands.old_client.requests.ClientResolutionChangeRequest.ID, new ClientResolutionChangeHandler());
        }

        public void LookUp(byte[] bytes, GameClient client)
        {
            var parser = new ByteParser(bytes);

            if (Properties.Game.PRINTING_COMMANDS)
                Console.WriteLine("Received->{0} Instance: {1}", parser.CMD_ID, client.UserId);

            if (parser.CMD_ID == commands.old_client.requests.VersionRequest.ID)
            {
                var cmd = new commands.old_client.requests.VersionRequest();
                cmd.readCommand(bytes);
                new ShipInitalizationHandler(client, cmd.playerId, cmd.sessionId);
                return;
            }

            if (parser.CMD_ID == commands.new_client.requests.LoginRequest.ID)
            {
                var cmd = new commands.new_client.requests.LoginRequest();
                cmd.readCommand(bytes);
                new ShipInitalizationHandler(client, cmd.playerId, cmd.sessionId, true);
                return;
            }

            var gameSession = World.StorageManager.GetGameSession(client.UserId);

            if (gameSession != null)
            {
                if (gameSession.Player.UsingNewClient)
                {
                    if (NewClientCommands.ContainsKey(parser.CMD_ID))
                        NewClientCommands[parser.CMD_ID].execute(gameSession, bytes);
                }
                else
                {
                    if (OldClientCommands.ContainsKey(parser.CMD_ID))
                        OldClientCommands[parser.CMD_ID].execute(gameSession, bytes);
                }

                if (parser.CMD_ID == commands.new_client.LegacyModule.ID || parser.CMD_ID == commands.old_client.LegacyModule.ID)
                {
                    var packet = parser.readUTF();
                    if (Properties.Game.PRINTING_LEGACY_COMMANDS)
                        Console.WriteLine("Received->{0} Instance: {1}", packet, client.UserId);

                    if (packet.Contains('|'))
                    {
                        var splittedPacket = packet.Split('|');
                        if (LegacyCommands.ContainsKey(splittedPacket[0]))
                            LegacyCommands[splittedPacket[0]].execute(gameSession, splittedPacket);
                    }
                    else
                    {
                        if (LegacyCommands.ContainsKey(packet))
                            LegacyCommands[packet].execute(gameSession, new[] { packet });
                    }
                }
            }
        }

        public void DialogLookUp(string packet)
        {
            
        }
    }
}
