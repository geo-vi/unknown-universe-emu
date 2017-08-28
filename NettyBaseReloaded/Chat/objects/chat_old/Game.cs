namespace NettyBaseReloaded.Chat.objects.chat
{
    class Game
    {
        public int Id { get; set; }

        public string Longname { get; set; }

        public string Shortname { get; set; }

        public int CreatorId { get; set; }

        public Game(int id, string longname, string shortname, int creatorId)
        {
            Id = id;
            Longname = longname;
            Shortname = shortname;
            CreatorId = creatorId;
        }
    }
}
