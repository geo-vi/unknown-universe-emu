namespace NettyBaseReloaded.Chat.objects.chat
{
    class TextModule
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Text { get; set; }

        public TextModule(int id, string description, string text)
        {
            Id = id;
            Description = description;
            Text = text;
        }
    }
}
