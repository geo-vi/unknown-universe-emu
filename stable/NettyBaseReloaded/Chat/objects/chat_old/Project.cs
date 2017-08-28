namespace NettyBaseReloaded.Chat.objects.chat
{
    class Project
    {
        public int Id { get; set; }

        public Game Game { get; set; }

        public Instance Instance { get; set; }

        public Project(int id, Game game, Instance instance)
        {
            Id = id;
            Game = game;
            Instance = instance;
        }
    }
}
