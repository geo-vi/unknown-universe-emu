namespace NettyBaseReloaded.Chat.objects.chat
{
    class SupportMessage
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int SupportId { get; set; }

        public Game Game { get; set; }

        public Language Language { get; set; }

        public string Username { get; set; }

        public string Question { get; set; }

        public SupportMessage(int id, int senderId, int supportId, Game game, Language language, string username, string question)
        {
            Id = id;
            SenderId = senderId;
            SupportId = supportId;
            Game = game;
            Language = language;
            Username = username;
            Question = question;
        }
    }
}
