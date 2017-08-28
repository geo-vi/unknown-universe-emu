using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Main.global.managers;
using NettyBaseReloadedController.Main.global.objects;

namespace NettyBaseReloadedController.Main.global
{
    class Global
    {
        public const int STATE_LOADING = 0;
        public const int STATE_LOADED = 1;

        public TickManager TickManager = new TickManager();
        public StorageManager StorageManager = new StorageManager();

        public int STATE = STATE_LOADING;

        public void AddMaps()
        {
            StorageManager.Spacemaps.Add(0, new Spacemap(0));
            StorageManager.Spacemaps.Add(1, new Spacemap(1));
            StorageManager.Spacemaps.Add(6, new Spacemap(6));
            StorageManager.Spacemaps.Add(9, new Spacemap(9));
        }
    }
}
