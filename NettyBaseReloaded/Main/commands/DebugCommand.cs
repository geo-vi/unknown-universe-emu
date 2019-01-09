﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Main.commands
{
    class DebugCommand : Command
    {
        public DebugCommand() : base("debug", "Debug command")
        {
            
        }

        public override void Execute(string[] args = null)
        {
            if (args == null || args.Length < 2)
            {
                if (Properties.Server.DEBUG)
                {
                    Properties.Server.DEBUG = false;
                    Properties.Game.PRINTING_COMMANDS = false;
                    Properties.Game.PRINTING_LEGACY_COMMANDS = false;
                    Properties.Game.DEBUG_ENTITIES = false;
                    Properties.Game.PRINTING_CONNECTIONS = false;
                    Console.WriteLine("Debug::Session ended");
                }
                else
                {
                    Properties.Server.DEBUG = true;
                    Console.WriteLine("Debug::Session started");
                }
                return;
            }
            if (!Properties.Server.DEBUG)
            {
                Console.WriteLine("Access Denied!");
            }

            var playerId = 0;
            if (args.Length > 2)
                playerId = int.Parse(args[2]);

            switch (args[1])
            {
                case "commands":
                case "printcmd":
                case "printcmds":
                    if (Properties.Game.PRINTING_COMMANDS)
                    {
                        Properties.Game.PRINTING_COMMANDS = false;
                        Console.WriteLine("Debug::Stopped printing commands");
                        break;
                    }

                    Properties.Game.PRINTING_COMMANDS = true;
                    Console.WriteLine("Debug::Commands should now print");
                    break;
                case "packets":
                case "printpacket":
                case "printpackets":
                    if (Properties.Game.PRINTING_LEGACY_COMMANDS)
                    {
                        Properties.Game.PRINTING_LEGACY_COMMANDS = false;
                        Console.WriteLine("Debug::Stopped printing legacy commands");
                        break;
                    }

                    Properties.Game.PRINTING_LEGACY_COMMANDS = true;
                    Console.WriteLine("Debug::Legacy commands should now print");
                    break;
                case "range":
                case "entities":
                    if (Properties.Game.DEBUG_ENTITIES)
                    {
                        Properties.Game.DEBUG_ENTITIES = false;
                        Console.WriteLine("Debug::Stopped printing range entities");
                        break;
                    }

                    Properties.Game.DEBUG_ENTITIES = true;
                    Console.WriteLine("Debug::Range entities should now print");
                    break;
                case "connections":
                    if (Properties.Game.PRINTING_CONNECTIONS)
                    {
                        Properties.Game.PRINTING_CONNECTIONS = false;
                        Console.WriteLine("Debug::Stopped printing connections");
                        break;
                    }

                    Properties.Game.PRINTING_CONNECTIONS = true;
                    Console.WriteLine("Debug::Player connections are now printing");
                    break;
                case "send":
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(playerId), args[3]);
                    break;
                case "listen":
                    var session = World.StorageManager.GetGameSession(playerId);
                    session.Client.Listening = !session.Client.Listening;
                    break;
            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            var id = session.Player.Id;
            var sessionId = session.Player.SessionId;
            var worldSession = World.StorageManager.GetGameSession(id);
            if (worldSession != null && worldSession.Player.Id == id && worldSession.Player.SessionId == sessionId &&
                worldSession.Player.RankId == Rank.ADMINISTRATOR)
            {
                Execute(args);
            }

        }
    }
}
