using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class WizardEffect : Cooldown
    {
        private static readonly int[] POSSIBLE_SHIPS = new[] {120, 121, 51, 83, 32, 73, 75, 79, 78, 76, 77, 71, 50, 9, 3, 7, 6, 5, 4, 2, 1, 8, 52, 53, 10};

        public WizardEffect() : base(DateTime.Now, DateTime.Now.AddSeconds(300))
        {
        }

        public override void OnStart(Character character)
        {
            //character.Hangar.ShipDesign = World.StorageManager.Ships[POSSIBLE_SHIPS[Random.Next(0, POSSIBLE_SHIPS.Length - 1)]];
            //character.UpdateShip();
        }

        public override void OnFinish(Character character)
        {
            //character.Hangar.ShipDesign = character.Hangar.Ship;
            //character.UpdateShip();
            //character.Update();
        }

        public override void Send(GameSession gameSession)
        {
        }
    }
}
