using System;
using System.Diagnostics;
using System.IO;
using NettyBaseReloaded.Chat.packet;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.controllers
{
    class MessagesController
    {
        public static void Send(Character character, int roomId, string message)
        {
            if (ServerManager.RCON_LOGIN_ONLY && !character.IsRcon())
            {
                if (message == ServerManager.RCON_PW)
                    character.Rcon = true;

                if (character.IsRcon())
                {
                    Send(character, "Authoized to enter.\nReloading...");
                    LoginController.ExecuteLogin(ServerManager.GetChatSession(character.Id));
                    Game.controllers.LoginController.ExecuteLogin(ServerManager.GetGameSession(character.Id));
                }
                return;
            }

            if (message.StartsWith("/"))
            {
                //Console.WriteLine("IM ABOUT TO HANDLE THIS SHIT STRONK");
                MessageHandler(character, roomId, message);
            }
            else ChatClient.SendToAll(character, PacketBuilder.SendMessage(character, roomId, message));
        }

        private static void ModeratorMessage(Character character, int roomId, string message)
        {

        }

        private static void MessageHandler(Character character, int roomId, string message)
        {
            //if (character is Moderator) ModeratorMessage(character,roomId, message);
            var session = ServerManager.GetGameSession(character.Id);
            if (message.Contains(' '))
            {
                var splitMessage = message.Split(' ');
                switch (splitMessage[0])
                {
                    case "/cam":
                        if (session == null) return;
                        switch (splitMessage[1])
                        {
                            case "cords":
                                session.Client.Send(CameraLockToCoordinatesCommand.write(int.Parse(splitMessage[2]) * 100, int.Parse(splitMessage[3]) * 100, 1));
                                break;
                            case "selected":
                                session.Client.Send(CameraLockToShipCommand.write(session.Player.Selected.Id, 1, 1));
                                break;
                            case "hero":
                                session.Client.Send(CameraLockToHeroCommand.write());
                                break;
                            default:
                                Send(character, "cords: x/y; selected; hero");
                                break;
                        }
                        break;
                    case "/send":
                        if (session == null) return;

                        var replaced = message.Replace("ATRIBUTE_SEPERATOR", "|");

                        session.Client.Send(LegacyModule.write(replaced));
                        break;
                    case "/global":
                        if (character.IsRcon())
                        {
                            byte[] packet;
                            string cutMessage;
                            switch (splitMessage[1])
                            {
                                case "big":
                                    cutMessage = message.Replace("/global big ", "");
                                    packet = Net.netty.PacketBuilder.BigMessage(cutMessage);
                                    break;
                                default:
                                    cutMessage = message.Replace("/global ", "");
                                    packet = Net.netty.PacketBuilder.LegacyModule("0|A|STD|" + cutMessage);
                                    break;
                            }
                            foreach (var gameClient in ServerManager.GameSessions.Values)
                            {
                                gameClient.Client.Send(packet);
                            }
                        }
                        break;
                    case "/rcon":
                        switch (splitMessage[1])
                        {
                            case "login":
                                if (splitMessage[2] != ServerManager.RCON_PW)
                                    break;

                                character.Rcon = true;
                                MessagesController.Send(character, "Logged in as RCON");
                                break;
                            case "auth":
                                if (character.IsRcon())
                                {
                                    if (splitMessage[2] == null)
                                    {
                                        Send(character, "/rcon auth [edit/delete]");
                                        break;
                                    }
                                    switch (splitMessage[2])
                                    {
                                        case "edit":
                                            Send(character, "Not done yet.");
                                            break;
                                        case "delete":
                                            File.Delete(Directory.GetCurrentDirectory() + "/p.auth");
                                            File.Delete(Directory.GetCurrentDirectory() + "/k.auth");
                                            Send(character, "Successfully deleted auth files.");
                                            break;
                                    }
                                }
                                break;
                            case "lock":
                                if (character.IsRcon())
                                {
                                    if (!ServerManager.RCON_LOGIN_ONLY)
                                    {
                                        Send(character, "Locking the server to RCONs only.");
                                        ServerManager.RCON_LOGIN_ONLY = true;
                                        break;
                                    }

                                    Send(character, "Unlocking..");
                                    ServerManager.RCON_LOGIN_ONLY = false;

                                    foreach (var entry in ServerManager.GameSessions)
                                    {
                                        var chatEntry = ServerManager.GetChatSession(entry.Key);
                                        if (chatEntry != null)
                                            if (chatEntry.Player.Rcon)
                                                return;

                                        entry.Value.CloseSession();
                                    }
                                }
                                break;
                            case "restart":
                                if (character.IsRcon())
                                {
                                    Send(character, "Server is restarting...");
                                    Process.Start(Directory.GetCurrentDirectory() + "/NettyRestarter.exe");
                                }
                                break;
                            case "exit":
                                if (character.IsRcon())
                                    Environment.Exit(0);
                                break;
                        }
                        break;
                    default:
                        //Console.WriteLine(splitMessage[0]);
                        break;
                }

            }
            else
            {
                switch (message)
                {
                    case "/online":
                        Send(character, "Currently there are " + ServerManager.ChatSessions.Count + " users connected to chat.");
                        break;
                }
            }
        }

        public static void Send(Character character, string message)
        {
            if (character == null) return;

            Packet.Builder.Legacy(Chat.StorageManager.GetChatSession(character.Id), "dq%" + message););
        }

        public static void SendGlobal(string message, int roomId = 0)
        {
            if (roomId != 0) /* Sends to room */ return;

            foreach (var user in Chat.StorageManager.ChatSessions.Values)
            {
                Packet.Builder.Legacy(user, "dq%" + message));
            }
        }
    }
}