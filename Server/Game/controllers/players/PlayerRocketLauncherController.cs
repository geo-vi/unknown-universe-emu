using System;
using Server.Game.controllers.characters;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.entities.ships.items;

namespace Server.Game.controllers.players
{
    class PlayerRocketLauncherController : RocketLauncherController
    {
        /// <summary>
        /// Returning Player and converting it from Character
        /// </summary>
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }
        
        private bool _isLoading;

        /// <summary>
        /// Plug event
        /// </summary>
        public override void OnAdded()
        {
            Player.RocketLauncher.SetLoad(Player.Settings.GetSettings<SlotbarSettings>().SelectedHellstormRocketAmmo);
                
            Player.RocketLauncher.OnLoadChange += OnLoadChange;
            Player.RocketLauncher.OnRocketsLaunched += OnRocketsLaunched;
        }
        
        /// <summary>
        /// Unplug event
        /// </summary>
        public override void OnRemoved()
        {
            Player.RocketLauncher.OnLoadChange -= OnLoadChange;
            Player.RocketLauncher.OnRocketsLaunched -= OnRocketsLaunched;
        }
        
        public override void OnTick()
        {
            if (_isLoading)
            {
                Reload();
            }
        }

        /// <summary>
        /// Starting the loading process
        /// </summary>
        public void StartLoading()
        {
            _isLoading = true;
        }
        
        /// <summary>
        /// Once new rocket is added, update
        /// </summary>
        protected override void OnAddRocket()
        {
            base.OnAddRocket();
            PrebuiltPlayerCommands.Instance.RocketLauncherStatus(Player);
        }
        
        /// <summary>
        /// Once rocket launcher load changes, update
        /// </summary>
        /// <param name="sender">Rocket Launcher</param>
        /// <param name="e">New loot ID</param>
        private void OnLoadChange(object sender, string e)
        {
            _isLoading = false;
            PrebuiltPlayerCommands.Instance.RocketLauncherStatus(Player);
        }
        
        /// <summary>
        /// Once rocket launcher launches the rockets, stop the loading process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRocketsLaunched(object sender, EventArgs e)
        {
            _isLoading = false;
            PrebuiltPlayerCommands.Instance.RocketLauncherStatus(Player);
        }
    }
}