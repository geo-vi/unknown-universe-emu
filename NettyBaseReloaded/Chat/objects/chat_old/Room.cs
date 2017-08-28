using System;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Chat.objects.chat
{
    enum RoomType
    {
        NORMAL_ROOM,
        PRIVATE_ROOM,
        CLAN_ROOM,
        SUPPORT_ROOM,
        GROUP_ROOM,
        DYNAMIC_ROOM,
        SCALABLE_ROOM_PARENT,
        SCALABLE_ROOM_CHILD,
        SECTOR_ROOM
    }

    class Room
    {
        public int Id { get; set; }

        public int RoomnameId { get; set; }

        public Game Game { get; set; }

        public Instance Instance { get; set; }

        public Language Language { get; set; }

        public int CompanyId { get; set; }

        public int TabOrder { get; set; }

        public RoomType RoomType { get; set; }

        public bool NewComerRoom { get; set; }

        public bool MultilanguageRoom { get; set; }

        public int SectorId { get; set; }

        public int ParentId { get; set; }

        public int MaxUsersPerChild { get; set; }
        
        public int MaxAvgRoomUsers { get; set; }

        public Room(int id, int roomnameId, Game game, Instance instance, Language language, int companyId, int tabOrder,
            RoomType roomType, bool newComerRoom, bool multilanguageRoom,
            int sectorId, int parentId, int maxUsersPerChild, int maxAvgRoomUsers)
        {
            Id = id;
            RoomnameId = roomnameId;
            Game = game;
            Instance = instance;
            Language = language;
            CompanyId = companyId;
            TabOrder = tabOrder;
            RoomType = roomType;
            NewComerRoom = newComerRoom;
            MultilanguageRoom = multilanguageRoom;
            SectorId = sectorId;
            ParentId = parentId;
            MaxUsersPerChild = maxUsersPerChild;
            MaxAvgRoomUsers = maxAvgRoomUsers;
        }

        public override string ToString()
        {
            return Id + "|" + Language.RoomNames[RoomnameId] + "|" + TabOrder + "|" + CompanyId + "|" +
                   (int) RoomType + "|" + Convert.ToInt32(NewComerRoom);
        }
    }
}
