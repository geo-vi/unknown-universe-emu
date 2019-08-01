using Server.Game.objects.entities;
using Server.Game.objects.entities.ships;

namespace Server.Game.controllers.characters
{ 
    class ConfigurationController : AbstractedSubController
    {
        public override void OnAdded()
        {
            Character.Hangar.Configurations = Create();
        }

        protected virtual Configuration[] Create()
        {
            var firstConfiguration = new Configuration(1);
            firstConfiguration.TotalDamageCalculated = Character.Hangar.Ship.Damage;
            firstConfiguration.TotalShieldCalculated = Character.Hangar.Ship.Shield;
            firstConfiguration.TotalSpeedCalculated = Character.Hangar.Ship.Speed;
            var secondConfiguration = new Configuration(2);
            secondConfiguration.TotalDamageCalculated = Character.Hangar.Ship.Damage;
            secondConfiguration.TotalShieldCalculated = Character.Hangar.Ship.Shield;
            secondConfiguration.TotalSpeedCalculated = Character.Hangar.Ship.Speed;
            return new[] { firstConfiguration, secondConfiguration };
        }
    }
}