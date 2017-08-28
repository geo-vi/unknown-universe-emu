using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;

namespace NettyBaseReloaded.Chat.managers
{
    class StorageManager
    {

        public Dictionary<int, Moderator> Moderators = new Dictionary<int, Moderator>();

        public Dictionary<int, BannedCharacter> GlobalBans = new Dictionary<int, BannedCharacter>();

        public Dictionary<int, Room> Rooms = new Dictionary<int, Room>();

        public Dictionary<int, ChatSession> ChatSessions = new Dictionary<int, ChatSession>();

        public ChatSession GetChatSession(int userId)
        {
            return ChatSessions.ContainsKey(userId) ? ChatSessions[userId] : null;
        }
    }
}
