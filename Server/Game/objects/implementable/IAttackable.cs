namespace Server.Game.objects.implementable
{
    interface IAttackable
    {
        int CurrentHealth { get; set; }
        int MaxHealth { get; set; }
        int CurrentShield { get; set; }
        int MaxShield { get; set; }
    }
}
