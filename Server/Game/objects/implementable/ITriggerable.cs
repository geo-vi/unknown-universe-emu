namespace Server.Game.objects.implementable
{
    interface ITriggerable
    {
        Vector Position { get; set; }
        void Trigger();
    }
}
