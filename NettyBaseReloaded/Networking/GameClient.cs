using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Networking
{
    class GameClient
    {
        private XSocket XSocket;

        public int UserId { get; set; }

        public IPAddress IPAddress => XSocket.RemoteHost;


        /* debug only */

        public int SentPackets;
        public bool Listening = true;

        /* -end debug only- */

        public GameClient(XSocket gameSocket)
        {
            XSocket = gameSocket;
            XSocket.OnReceive += XSocketOnOnReceive;
            XSocket.ConnectionClosedEvent += XSocketOnConnectionClosedEvent;
            XSocket.Read();
        }

        private void XSocketOnConnectionClosedEvent(object sender, EventArgs eventArgs)
        {
        }

        private void XSocketOnOnReceive(object sender, EventArgs eventArgs)
        {
            var bytes = (ByteArrayArgs)eventArgs;
            Packet.Handler.LookUp(bytes.ByteArray, this);
        }

        public void Send(byte[] bytes)
        {
            try
            {
                XSocket.Write(bytes);
                //SentPackets++;
                //if (Listening)
                //{
                //    var caller = Out.GetCaller();
                //    Console.WriteLine($"Listener ({UserId})::" + caller);
                //    if (caller.EndsWith("LegacyModule"))
                //    {
                //        var legacy = new LegacyModule();
                //        legacy.readCommand(bytes);
                //        Console.WriteLine("[" + legacy.message + "]");
                //    }
                //}
            }
            catch
            {
                World.StorageManager.GetGameSession(UserId)?.KillSession();
                Debug.WriteLine("->" + Out.GetCaller());
            }
        }

        public static void SendRangePacket(Character character, Command command, bool sendCharacter = false)
        {
            if (character == null) return;
            try
            {
                foreach (var entry in character.Spacemap.Entities)
                {
                    var entity = entry.Value as Player;
                    if (entity == null) continue;

                    if (entity.InRange(character, character.RenderRange) && entity != character || entity.Pet == character)
                    {
                        if (entity.UsingNewClient && command.IsNewClient)
                        {
                            entity.GetGameSession()?.Client.Send(command.Bytes);
                        }
                        if (!entity.UsingNewClient && !command.IsNewClient)
                        {
                            entity.GetGameSession()?.Client.Send(command.Bytes);
                        }
                    }
                }

                if (sendCharacter && character is Player)
                {
                    var player = (Player) character;
                    if (command.IsNewClient == player.UsingNewClient)
                        player.GetGameSession()?.Client.Send(command.Bytes);
                }
            }
            catch
            {
                Out.WriteLog("Something went wrong sending a range packet.", "ERROR");
            }
        }

        public static void SendToPlayerView(Character character, Command command, bool sendCharacter = false) =>
            SendRangePacket(character, command, sendCharacter);

        public static void SendPacketSelected(Character character, Command command)
        {
            try
            {
                foreach (var entry in character.Spacemap.Entities)
                {
                    var entity = entry.Value;

                    if (entity is Player && entity.Selected != null)
                    {
                        if (entity.Selected.Id == character.Id)
                        {
                            var entitySession = World.StorageManager.GetGameSession(entity.Id);
                            if (entitySession != null && entitySession.Player.UsingNewClient == command.IsNewClient)
                                entitySession.Client.Send(command.Bytes);
                        }
                    }
                }
            }
            catch
            {
                Out.WriteLog("Something went wrong sending a range packet.", "ERROR");
            }
        }

        public static void SendToSpacemap(Spacemap spacemap, Command command)
        {
            try
            {
                foreach (var entry in spacemap.Entities)
                {
                    var entity = entry.Value as Player;
                    if (entity != null && (entity.Spacemap != null || entity.Position != null))
                    {
                        var session = World.StorageManager.GetGameSession(entity.Id);
                        if (session == null || !session.Active) return;
                        if (entity.UsingNewClient && command.IsNewClient)
                        {
                            session?.Client.Send(command.Bytes);
                        }
                        if (!entity.UsingNewClient && !command.IsNewClient)
                        {
                            session?.Client.Send(command.Bytes);
                        }
                    }
                }
            }
            catch
            {
                Out.WriteLog("Something went wrong sending a spacemap packet.", "ERROR");
            }
        }


        public void Disconnect()
        {
            try
            {
                XSocket.Close();
            }
            catch
            {
                Out.WriteLog("Error disconnecting user from Game", "GAME");
            }
        }

    }
}
