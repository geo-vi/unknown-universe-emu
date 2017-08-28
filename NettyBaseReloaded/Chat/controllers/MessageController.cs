using System;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.controllers
{
    class MessageController
    {
        public static void Send(Character character, int roomId, string message)
        {
            if (message.StartsWith("/"))
            {
                throw new NotImplementedException();
                //return;
            }
            Room(character, roomId, message);
        }

        public static void Room(Character character, int roomId, string message)
        {
            var room = Chat.StorageManager.Rooms[roomId];
            if (room == null) return;

            if(character is Moderator)
                ChatClient.SendToRoom(character, "j%" + roomId + "@" + character.Name + "@" + message + "@" + (int)((Moderator)character).AdminLevel + "#", room);
            if (character is Bot)
                throw new NotImplementedException();
            if (character is Player)
                ChatClient.SendToRoom(character, "a%" + roomId + "@" + character.Name + "@" + message + "#", room);
        }
    }
}