using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects.chat.players;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Player : Character
    {
        /// <summary>
        /// Player controller
        /// </summary>
        public new PlayerController Controller { get; set; }

        public Dictionary<int, ChatIssue> Issues;

        public bool IsMute =>
            Issues.Any(x => x.Value.IssueType == ChatIssueTypes.MUTE && x.Value.Expiry > DateTime.Now);

        public Player(int id, string name, string sessionId, Clan clan) : base(id, name, sessionId, clan)
        {
            Issues = Chat.DatabaseManager.LoadChatIssues(this);
        }

        /// <summary>
        /// returns session
        /// </summary>
        /// <returns></returns>
        public ChatSession GetSession()
        {
            return Chat.StorageManager.GetChatSession(Id);
        }

        public void DisconnectRooms()
        {
            foreach (var room in ConnectedRooms)
            {
                room.Value.Kick(this);
            }
        }
    }
}