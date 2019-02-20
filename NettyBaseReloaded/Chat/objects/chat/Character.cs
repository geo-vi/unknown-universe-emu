using System.Collections.Generic;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.packet;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Character
    {
        /// <summary>
        /// Character ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Character name
        /// </summary>
        public string Name { get; set; }

        public string SessionId { get; set; }

        public Clan Clan { get; set; }

        public virtual AbstractCharacterController Controller
        {
            get
            {
                if (this is Player)
                {
                    var temp = (Player) this;
                    return temp.Controller;
                }
                if (this is Moderator)
                {
                    var temp = (Moderator) this;
                    return temp.Controller;
                }
                if (this is Bot)
                {
                    var temp = (Bot) this;
                    return temp.Controller;
                }

                return null;
            }
        }


        /// <summary>
        /// Rooms character is currently connected in
        /// </summary>
        public Dictionary<int, Room> ConnectedRooms = new Dictionary<int, Room>();

        /// <summary>
        /// If character is RCON
        /// </summary>
        public bool RCON { get; set; }

        public Character(int id, string name, string sessionId, Clan clan)
        {
            Id = id;
            Name = name;
            SessionId = sessionId;
            Clan = clan;
        }

        public bool ConnectedToRoom(int roomId)
        {
            return ConnectedRooms.ContainsKey(roomId);
        }

        public void ConnectToRoom(int roomId)
        {
            var room = Chat.StorageManager.Rooms[roomId];
            if (room == null) return;

            if (room.ConnectedUsers.Count > room.MaxUsers)
            {
                if (this is Player p)
                    Packet.Builder.SystemMessage(p.GetSession(), "Maximal amount of users connected. Please try again later.");
                return;
            }

            if (room.ConnectedUsers.ContainsKey(Id))
            {
                room.ConnectedUsers[Id] = this;
            }
            else room.ConnectedUsers.Add(Id, this);

            if (!ConnectedRooms.ContainsKey(roomId))
                ConnectedRooms.Add(roomId, room);
            else ConnectedRooms[roomId] = room;
        }
    }
}