using System.Collections.Generic;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Game.objects.world.storages.playerStorages
{
    class SettingsStorage
    {
        /// <summary>
        /// If beta countdown will show
        /// </summary>
        public bool BetaCountdown { get; set; }

        public SettingsStorage()
        {
            BetaCountdown = true;
        }
    }
}