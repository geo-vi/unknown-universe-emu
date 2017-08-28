using System;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class BlockedUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public Game Game { get; set; }

        public string Username { get; set; }

        public string Tab { get; set; }

        public Instance Instance { get; set; }

        public Language Language { get; set; }

        public int RoomDbId { get; set; }

        public DateTime KickBegin { get; set; }

        public DateTime KickEnd { get; set; }

        public string BannedBy { get; set; }

        public string Comment { get; set; }

        public bool Permanent { get; set; }

        public string DebannedBy { get; set; }

        public DateTime DebanTime { get; set; }

        public BlockedUser(int id, int userId, Game game, string username, string tab, Instance instance,
            Language language, int roomDbId,
            DateTime kickBegin, DateTime kickEnd, string bannedBy, string comment, bool permanent, string debannedBy,
            DateTime debanTime)
        {
            Id = id;
            UserId = userId;
            Username = username;
            Tab = tab;
            Instance = instance;
            Language = language;
            RoomDbId = roomDbId;
            KickBegin = kickBegin;
            KickEnd = kickEnd;
            BannedBy = bannedBy;
            Comment = comment;
            Permanent = permanent;
            DebannedBy = debannedBy;
            DebanTime = debanTime;
        } 
    }
}
