namespace NettyBaseReloaded.Chat.objects.chat
{
    class BadWord
    {
        public int Id { get; set; }

        public string Word { get; set; }

        public int Level { get; set; }

        public Language Language { get; set; }

        public int CreatorId { get; set; }

        public string Creator { get; set; }

        public int[] GameIDs { get; set; }

        public BadWord(int id, string word, int level, Language language, int creatorId, string creator, int[] gameIds)
        {
            Id = id;
            Word = word;
            Level = level;
            Language = language;
            CreatorId = creatorId;
            Creator = creator;
            GameIDs = gameIds;
        }
    }
}
