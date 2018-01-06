using System;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.implementable.attack
{
    class Attacker
    {
        public Player Player { get; set; }

        public int TotalDamage { get; private set; }

        public DateTime LastRefresh { get; private set; }

        public bool FadedToGray { get; set; }

        public Attacker(Player player)
        {
            Player = player;
            LastRefresh = DateTime.Now;
        }

        public void Damage(int dmg)
        {
            TotalDamage += dmg;
        }

        public void Refresh()
        {
            LastRefresh = DateTime.Now;
        }
    }
}
