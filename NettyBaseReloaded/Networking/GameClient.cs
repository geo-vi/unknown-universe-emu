using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.packet;
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

        public GameClient(XSocket gameSocket)
        {
            XSocket = gameSocket;
            XSocket.OnReceive += XSocketOnOnReceive;
            XSocket.ConnectionClosedEvent += XSocketOnConnectionClosedEvent;
            XSocket.Read();
        }

        private void XSocketOnConnectionClosedEvent(object sender, EventArgs eventArgs)
        {         
            //var gameSess = World.StorageManager.GetGameSession(UserId);
            //gameSess?.Disconnect();
        }

        private void XSocketOnOnReceive(object sender, EventArgs eventArgs)
        {
            var bytes = (ByteArrayArgs)eventArgs;
            var packet = Encoding.UTF8.GetString(bytes.ByteArray);

            if (packet.StartsWith("dialog"))
                Packet.Handler.DialogLookUp(packet);
            else
                Packet.Handler.LookUp(bytes.ByteArray, this);
        }

        public void Send(byte[] bytes)
        {
            try
            {
                if (World.StorageManager.GetGameSession(UserId) != null)
                {
                    if (World.StorageManager.GameSessions[UserId].Player.Controller != null)
                        World.StorageManager.GameSessions[UserId].LastActiveTime = DateTime.Now;
                }

                XSocket.Write(bytes);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Connected?");
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
            }
        }
        public void Send(string packet)
        {
            XSocket.Write(packet);
        }

        public static void SendRangePacket(Character character, Command command, bool sendCharacter = false)
        {
            if (character == null) return;
            try
            {
                foreach (var entry in character.Spacemap.Entities.ToList())
                {
                    var entity = entry.Value as Player;

                    if (character.InRange(entry.Value) && entity != null)
                    {
                        if (entity.UsingNewClient && command.IsNewClient)
                        {
                            World.StorageManager.GameSessions[entity.Id]?.Client.Send(command.Bytes);
                        }
                        if (!entity.UsingNewClient && !command.IsNewClient)
                        {
                            World.StorageManager.GameSessions[entity.Id]?.Client.Send(command.Bytes);
                        }
                    }
                }

                if (sendCharacter && character is Player)
                {
                    var player = (Player) character;
                    if (command.IsNewClient == player.UsingNewClient)
                        World.StorageManager.GameSessions[character.Id]?.Client.Send(command.Bytes);
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Something went wrong sending a range packet.", "ERROR", ConsoleColor.Red);
                Debug.WriteLine(e.Message, "Debug Error");
            }
        }

        public static void SendPacketSelected(Character character, Command command)
        {
            try
            {
                foreach (var entry in character.Spacemap.Entities.ToList())
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
            catch (Exception e)
            {
                Out.WriteLine("Something went wrong sending a range packet.", "ERROR", ConsoleColor.Red);
                Debug.WriteLine(e.Message, "Debug Error");
            }
        }

        public static void SendToSpacemap(Spacemap spacemap, Command command)
        {
            try
            {
                foreach (var entry in spacemap.Entities.ToList())
                {
                    var entity = entry.Value as Player;
                    if (entity != null)
                    {
                        if (entity.UsingNewClient && command.IsNewClient)
                        {
                            World.StorageManager.GameSessions[entity.Id]?.Client.Send(command.Bytes);
                        }
                        if (!entity.UsingNewClient && !command.IsNewClient)
                        {
                            World.StorageManager.GameSessions[entity.Id]?.Client.Send(command.Bytes);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Something went wrong sending a spacemap packet.", "ERROR", ConsoleColor.Red);
                Debug.WriteLine(e.Message, "Debug Error");

            }
        }

        public void Disconnect()
        {
            try
            {
                XSocket.Close();
            }
            catch (Exception)
            {
                Out.WriteLine("Error disconnecting user from Game", "GAME", ConsoleColor.DarkRed);
            }
        }

    }
}
