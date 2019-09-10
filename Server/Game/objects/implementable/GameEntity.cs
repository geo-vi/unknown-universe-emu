
namespace Server.Game.objects.implementable
{
    interface IGameEntity
    {
        int Id { get; }
        Vector Position { get; set; }
        Spacemap Spacemap { get; set; }
    }
}