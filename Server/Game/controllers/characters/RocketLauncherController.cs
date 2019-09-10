using System;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.characters
{
    class RocketLauncherController : AbstractedSubController
    {
        private DateTime _lastRocketLauncherReload;

        public override void OnTick()
        {
            Reload();
        }
        
        /// <summary>
        /// Reloading the Rocket Launcher 
        /// </summary>
        protected void Reload()
        {
            if (Character.RocketLauncher.LoadedRockets > Character.RocketLauncher.MaximalRocketLoad)
            {
                Out.QuickLog("Loaded too many rocket launcher rockets", LogKeys.ERROR_LOG);
                throw new Exception("Loaded rocket launcher rockets over load limit");
            }

            if (Character.RocketLauncher.LoadLootId == "")
            {
                Out.QuickLog("Invalid rocket launcher load loot id",LogKeys.ERROR_LOG);
                throw new Exception("Invalid rocket launcher load loot Id");
            }

            if (_lastRocketLauncherReload.AddSeconds(1) > DateTime.Now) return;
            
            OnAddRocket();
            _lastRocketLauncherReload = DateTime.Now;
        }

        /// <summary>
        /// Adding a rocket to rocket launcher
        /// </summary>
        protected virtual void OnAddRocket()
        {
            Character.RocketLauncher.LoadedRockets++;
        }
    }
}