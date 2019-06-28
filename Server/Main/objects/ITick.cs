namespace Server.Main.objects
{
    interface ITick
    {
        int TickId { get; set; }
        void Tick();
    }
}
