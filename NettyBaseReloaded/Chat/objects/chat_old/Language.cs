namespace NettyBaseReloaded.Chat.objects.chat
{
    class Language
    {
        public int Id { get; set; }

        public string Longname { get; set; }

        public string Shortname { get; set; }

        public int CreatorId { get; set; }

        public string[] RoomNames { get; set; }

        public Language(int id, string longname, string shortname, int creatorId, string[] roomNames)
        {
            Id = id;
            Longname = longname;
            Shortname = shortname;
            CreatorId = creatorId;
            RoomNames = roomNames;
        }
    }
}
