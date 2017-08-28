using System.Collections.Generic;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects.storages;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Character
    {
        /// <summary>
     /// Chat client's ID
     /// </summary>

        public int Id { get; set; }

        /// <summary>
        /// Chat client's name
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// Chat client's running language (Currently supported: 'en')
        /// </summary>

        public string Language { get; set; }

        public Dictionary<int, Room> ConnectedRooms = new Dictionary<int, Room>();

        public Clan Clan { get; set; }

        public bool Rcon { get; set; }

        public virtual AbstractCharacterController Controller
        {
            get
            {
                if (this is Moderator)
                {
                    var temp = (Moderator) this;
                    return temp.Controller;
                }
                return new PlayerController(this);
            }
        }

        public RoomsStorage RoomsStorage = new RoomsStorage();

        public Character(int id, string name, Clan clan, string lang)
        {
            Id = id;
            Name = name;
            Language = lang;
            Clan = clan;
        }

        public void ConnectToRoom(Room room)
        {
            if (ConnectedRooms.ContainsValue(room)) return;
            ConnectedRooms.Add(room.Id, room);
        }

        public bool IsRcon()
        {
            return Rcon;
        }
    }
}
