using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.packet;
using Character = NettyBaseReloaded.Chat.objects.chat.Character;

namespace NettyBaseReloaded.Networking
{
    class ChatClient
    {
        private XSocket XSocket;
        public int UserId;

        public ChatClient(XSocket chatSocket)
        {
            XSocket = chatSocket;
            XSocket.OnReceive += XSocketOnReceive;
            XSocket.ConnectionClosedEvent += XSocketOnConnectionClosedEvent;
            XSocket.Read(true);
        }

        private void XSocketOnConnectionClosedEvent(object sender, EventArgs eventArgs)
        {
            Chat.Chat.StorageManager.GetChatSession(UserId)?.Close();
        }

        private void XSocketOnReceive(object sender, EventArgs eventArgs)
        {
            var stringArgs = (StringArgs)eventArgs;
            var packet = stringArgs.Packet;

            const string policyPacket = "<?xml version=\"1.0\"?>\r\n" +
                            "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                            "<cross-domain-policy>\r\n" +
                            "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n" +
                            "</cross-domain-policy>";

            if (packet.StartsWith("<policy-file-request/>"))
                XSocket.Write(policyPacket);
            else
                Packet.Handler.Handle(this, packet);
        }

        public void Send(string packet)
        {
            try
            {
                XSocket.Write(packet);
            }
            catch (Exception)
            {
                Debug.WriteLine("Chat: Connected?");
            }
        }

        public static void SendToAll(Character character, string packet, bool sendToSelf = true)
        {
            foreach (var user in Chat.Chat.StorageManager.ChatSessions.Values)
            {
                if (user.Player.Id == character.Id && !sendToSelf) return;

                user.Client.Send(packet);
            }
        }

        public static void SendToRoom(string packet, Room targetRoom)
        {
            foreach (var user in targetRoom.ConnectedUsers.Values)
            {
                if (user is Player)
                {
                    var chatSession = Chat.Chat.StorageManager.GetChatSession(user.Id);
                    chatSession.Client.Send(packet);
                }
            }
        }

        public void Disconnect()
        {
            if (Chat.Chat.StorageManager.ChatSessions.ContainsKey(UserId))
                Chat.Chat.StorageManager.ChatSessions.Remove(UserId);

            try
            {
                XSocket.Close();
            }
            catch (Exception)
            {
                Out.WriteLog("Error disconnecting user from Chat", "CHAT");
            }
        }
    }
}
