using System;
using System.Linq;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;

namespace Server.Game.objects.entities.ships.equipment
{
    class RocketLauncher
    {
        public int LoadedRockets { get; set; }

        public string LoadLootId { get; private set; }
        
        public RocketLaunchers[] Launchers { get; private set; }
        
        public int MaximalRocketLoad
        {
            get
            {
                return Launchers.Count(x => x == RocketLaunchers.HST_02) * 5 +
                       Launchers.Count(x => x == RocketLaunchers.HST_01) * 3;
            }
        }

        public bool IsFullyLoaded => MaximalRocketLoad == LoadedRockets;
        
        public event EventHandler<string> OnLoadChange;

        public event EventHandler OnRocketsLaunched;

        public RocketLauncher(RocketLaunchers[] launchers)
        {
            Launchers = launchers;
        }

        public RocketLauncher(RocketLaunchers[] launchers, string loadLootId) : this(launchers)
        {
            LoadLootId = loadLootId;
        }

        /// <summary>
        /// Setting the load
        /// </summary>
        /// <param name="lootId"></param>
        public void SetLoad(string lootId)
        {
            LoadedRockets = 0;
            LoadLootId = lootId;
            OnLoadChange?.Invoke(this, lootId);
        }

        /// <summary>
        /// Emptying the rocket launcher
        /// </summary>
        /// <param name="rockets"></param>
        public void OnRocketsLaunch()
        {
            LoadedRockets = 0;
            OnRocketsLaunched?.Invoke(this, EventArgs.Empty);
        }
    }
}