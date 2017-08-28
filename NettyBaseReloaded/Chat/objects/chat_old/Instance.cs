namespace NettyBaseReloaded.Chat.objects.chat
{
    class Instance
    {
        public int Id { get; set; }

        public string Shortname { get; set; }

        public string Longname { get; set; }

        public int CreatorId { get; set; }

        public Instance(int id, string shortname, string longname, int creatorId)
        {
            Id = id;
            Shortname = shortname;
            Longname = longname;
            CreatorId = creatorId;
        }
    }
}
