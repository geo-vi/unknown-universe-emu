using DotNetty.Buffers;
using Server.Networking;
using Server.Utils;
using System;
using System.Collections.Generic;
using Server.Game.managers;
using Server.Game.netty.commands;
using Server.Game.netty.handlers;
using Server.Game.objects.entities;
using Server.Main.objects;
using Server.Networking.clients;

namespace Server.Game.netty.packet
{
    class Handler
    {
        /* *****
         * TODO: 
         *        Find command 18706
         *        Find why by default no keybindings show!!!
         *
         *
         *
         *
         *
         */
        
        
        
        public Dictionary<short, IHandler> OldClientCommands = new Dictionary<short, IHandler>();
        public Dictionary<short, IHandler> NewClientCommands = new Dictionary<short, IHandler>();
        public Dictionary<string, ILegacyHandler> LegacyCommands = new Dictionary<string, ILegacyHandler>();
        
        public void AddCommands()
        {
            OldClientCommands.Add(commands.old_client.requests.QualitySettingsRequest.ID, new QualitySettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.DisplaySettingsRequest.ID, new DisplaySettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.AudioSettingsRequest.ID, new AudioSettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.WindowSettingsRequest.ID, new WindowSettingsHandler());
            OldClientCommands.Add(commands.old_client.requests.ShipSettingsRequest.ID, new ShipSettingsHandler());
            Out.QuickLog($"Successfully added {OldClientCommands.Count} old client handlers to Handler");
            Out.QuickLog($"Successfully added {NewClientCommands.Count} new client handlers to Handler");
        }

        public void LookUp(IByteBuffer buffer, GameClient client)
        {
            try
            {
                Out.WriteLog("Packet received", "GAME");

                if (client.Initialized)
                {
                    var parser = new ByteParser(buffer.Copy());

                    var gameSession = GameStorageManager.Instance.GetGameSession(client.UserId);

                    if (gameSession != null)
                    {
                        if (gameSession.Player.UsingNewClient)
                        {
                            if (NewClientCommands.ContainsKey(parser.CommandId))
                                NewClientCommands[parser.CommandId].execute(gameSession, buffer);
                            else if (parser.CommandId != commands.new_client.LegacyModule.ID) Console.WriteLine("[3D] Received->{0} Instance: {1}", parser.CommandId, client.UserId);
                        }
                        else
                        {
                            if (OldClientCommands.ContainsKey(parser.CommandId))
                                OldClientCommands[parser.CommandId].execute(gameSession, buffer);
                            else if (parser.CommandId != commands.old_client.LegacyModule.ID) Console.WriteLine("[7.5.3] Received->{0} Instance: {1}", parser.CommandId, client.UserId);
                        }

                        if (parser.CommandId == commands.new_client.LegacyModule.ID ||
                            parser.CommandId == commands.old_client.LegacyModule.ID)
                        {
                            var packet = parser.readUTF();

                            if (packet.Contains('|'))
                            {
                                var splittedPacket = packet.Split('|');
                                if (LegacyCommands.ContainsKey(splittedPacket[0]))
                                    LegacyCommands[splittedPacket[0]].execute(gameSession, splittedPacket);
                                else Console.WriteLine("Received->{0} Instance: {1}", packet, client.UserId);
                            }
                            else
                            {
                                if (LegacyCommands.ContainsKey(packet))
                                    LegacyCommands[packet].execute(gameSession, new[] {packet});
                                else Console.WriteLine("Received->{0} Instance: {1}", packet, client.UserId);

                            }
                        }
                    }
                }
                else
                {
                    LoginSequence(client, buffer);
                }
            }
            catch (Exception exception)
            {
                Out.QuickLog("Exception found.. Disconnecting user", LogKeys.ERROR_LOG);
                Out.QuickLog("Exception: " + exception + ";" + exception.StackTrace + ";" + exception.Message, LogKeys.ERROR_LOG);
            }
        }

        private void LoginSequence(GameClient client, IByteBuffer buffer)
        {                
            var parser = new ByteParser(buffer.Copy());

            if (parser.CommandId == commands.old_client.requests.VersionRequest.ID)
            {
                var cmd = new commands.old_client.requests.VersionRequest();
                cmd.readCommand(buffer);
                var loginHandler = new ShipInitializationHandler(client, cmd.playerId, cmd.sessionId);
                Profiler.Profile("Player login time profile", 0, loginHandler.Execute);
//                loginHandler.Execute();
            }
            else if (parser.CommandId == commands.new_client.requests.LoginRequest.ID)
            {
                var cmd = new commands.new_client.requests.LoginRequest();
                cmd.readCommand(buffer);
                var loginHandler = new ShipInitializationHandler(client, cmd.playerId, cmd.sessionId, true);
                loginHandler.Execute();
            }
            else
            {
                Console.WriteLine("Unrecognised Login sequence, something is wrong. Recording IP and command datas..");

                Console.WriteLine("IP : " + client.IpEndPoint + "; Data command ID: " + parser.CommandId + ", Data command length: " + parser.Lenght);
            }
        }
    }
}
