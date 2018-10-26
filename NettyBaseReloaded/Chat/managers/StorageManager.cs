using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Chat.managers
{
    class StorageManager
    {
        public Dictionary<int, Banned> GlobalBans = new Dictionary<int, Banned>();

        public Dictionary<int, Room> Rooms = new Dictionary<int, Room>();

        public Dictionary<int, ChatSession> ChatSessions = new Dictionary<int, ChatSession>();

        public Dictionary<int, Announcement> Announcements = new Dictionary<int, Announcement>();

        public ChatSession GetChatSession(int userId)
        {
            return ChatSessions.ContainsKey(userId) ? ChatSessions[userId] : null;
        }

        public void CreateRoomForGroup(Group group, string roomName)
        {
            //todo;
        }
    }
}
