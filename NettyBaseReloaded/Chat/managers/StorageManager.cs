using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Chat.managers
{
    class StorageManager
    {
        public Dictionary<int, Room> Rooms = new Dictionary<int, Room>();

        public Dictionary<int, ChatSession> ChatSessions = new Dictionary<int, ChatSession>();

        public Dictionary<int, Announcement> Announcements = new Dictionary<int, Announcement>();

        public Dictionary<int, Moderator> ChatModerators = new Dictionary<int, Moderator>();

        public ChatSession GetChatSession(int userId)
        {
            return ChatSessions.ContainsKey(userId) ? ChatSessions[userId] : null;
        }

        public void CreateRoomForGroup(Group group, string roomName)
        {
            //todo;
        }

        public Moderator GetChatModerator(int issuedBy)
        {
            if (ChatModerators.ContainsKey(issuedBy))
                return ChatModerators[issuedBy];
            return new Moderator(0, "", "", Global.StorageManager.GetClan(0), ModeratorLevelTypes.ADMIN);
        }

        public bool FindUserByName(string targetName, out Player chatUser)
        {
            var result = ChatSessions.Any(x => x.Value.Player.Name == targetName);
            chatUser = null;
            if (result)
            {
                chatUser = ChatSessions.FirstOrDefault(x => x.Value.Player.Name == targetName).Value.Player;
            }
            return result;
        }
    }
}
