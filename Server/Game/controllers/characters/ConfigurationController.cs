using System;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.characters
{ 
    class ConfigurationController : AbstractedSubController
    {
        public override void OnAdded()
        {
            Character.Hangar.Configurations = Create();
        }

        /// <summary>
        /// Creating the configurations
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Switching between configurations
        /// </summary>
        public virtual void Switch()
        {
            var newConfig = Character.CurrentConfig == 1 ? 2 : 1;
            if (Character.LastConfigurationChange.AddSeconds(3) > DateTime.Now)
            {
                Out.WriteLog("Trying to rapidly switch configuration, something is wrong", LogKeys.ALL_CHARACTER_LOG, Character.Id);
                throw new Exception("Trying to rapidly switch configuration");
            }
            Character.SetConfiguration(newConfig);
            Character.LastConfigurationChange = DateTime.Now;
        }
    }
}