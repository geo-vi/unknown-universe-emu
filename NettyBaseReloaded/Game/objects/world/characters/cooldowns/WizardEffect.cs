using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class WizardEffect : Cooldown
    {
        private static readonly int[] POSSIBLE_SHIPS = { 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 52, 53, 50, 57, 71, 77, 76, 121, 73, 75, 83, 81 };

        public WizardEffect() : base(DateTime.Now, DateTime.Now.AddSeconds(300))
        {
        }

        public override void OnStart(Character character)
        {
            base.OnStart(character);

            var visual = new VisualEffect(character, ShipVisuals.WIZARD_ATTACK, EndTime) {Attribute = RandomShip()};
            visual.Start();
            //character.Hangar.ShipDesign = World.StorageManager.Ships[POSSIBLE_SHIPS[Random.Next(0, POSSIBLE_SHIPS.Length - 1)]];
            //character.UpdateShip();
        }

        private int RandomShip()
        {
            var random = RandomInstance.getInstance(this);
            return POSSIBLE_SHIPS[random.Next(0, POSSIBLE_SHIPS.Length - 1)];
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
