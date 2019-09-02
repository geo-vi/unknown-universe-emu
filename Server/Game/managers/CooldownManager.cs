using System;
using Server.Game.controllers;
using Server.Game.controllers.implementable;
using Server.Game.controllers.server;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.managers
{
    class CooldownManager
    {
        public static CooldownManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CooldownManager();
                }

                return _instance;
            }
        }

        private static CooldownManager _instance;

        /// <summary>
        /// Creating and invoking a start for the cooldown
        /// </summary>
        /// <param name="cooldown">Cooldown</param>
        /// <exception cref="ArgumentException">Cooldown is null</exception>
        public void CreateCooldown(Cooldown cooldown)
        {
            if (cooldown == null)
            {
                Out.QuickLog("Something went wrong with creating cooldown", LogKeys.ERROR_LOG);
                throw new ArgumentException("Cannot add cooldown, Cooldown is null, error in manager");
            }
            
            ServerController.Get<CooldownController>().AddCooldown(cooldown);
            cooldown.Start();
        }

        public bool Exists(AbstractAttacker sender, CooldownTypes cooldownType)
        {
            if (sender == null)
            {
                return false;
            }

            return ServerController.Get<CooldownController>().Exists(sender, cooldownType);
        }

        /// <summary>
        /// Getting the cooldown
        /// </summary>
        /// <param name="sender">Person who needs to have the cooldown</param>
        /// <param name="type">Type of cooldown</param>
        /// <returns>Cooldown</returns>
        /// <exception cref="Exception">Sender is invalid</exception>
        /// <exception cref="NullReferenceException">Cooldown is not found</exception>
        public Cooldown Get(AbstractAttacker sender, CooldownTypes type)
        {
            if (sender == null)
            {
                Out.QuickLog("Something is wrong with CooldownManager's sender on Get", LogKeys.ERROR_LOG);
                throw new Exception("Something is wrong with sender");
            }

            var cooldown = ServerController.Get<CooldownController>().Get(sender, type);
            if (cooldown == null)
            {
                Out.QuickLog("Failed getting a cooldown from type " + type, LogKeys.ERROR_LOG);
                throw new NullReferenceException("Cooldown is null, failed getting");
            }
            return cooldown;
        }
    }
}