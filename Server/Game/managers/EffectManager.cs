using Server.Game.objects.entities;
using Server.Game.objects.implementable;

namespace Server.Game.managers
{
    class EffectManager
    {
        public void LaunchEffect(Player player, string effect)
        {
            switch (effect)
            {
                case "EMP":
                    //AmmunitionManager.Instance.TryReduceAmmunition(effect, 1);
                    //todo: remove 1 emp
                    break;
                case "ISH":
                    player.Ammunition.RemoveAmmunition("min01", 10);
                    //todo: remove 10 mines
                    break;
                case "SMB":
                    player.Ammunition.RemoveAmmunition("min01", 10);
                    //todo: remove 10 mines
                    break;
            }
        }

        public void LaunchEffect(AbstractAttacker attacker, string effect)
        {
            
        }
    }
}