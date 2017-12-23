using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.settings
{
    class Hotkey
    {
        public const short JUMP = 0;

        public const short CHANGE_CONFIG = 1;

        public const short ACTIVATE_LASER = 2;

        public const short LAUNCH_ROCKET = 3;

        public const short PET_ACTIVATE = 4;

        public const short PET_GUARD_MODE = 5;

        public const short LOGOUT = 6;

        public const short QUICKSLOT = 7;

        public const short QUICKSLOT_PREMIUM = 8;

        public const short TOGGLE_WINDOWS = 9;

        public const short PERFORMANCE_MONITORING = 10;

        public const short ZOOM_IN = 11;

        public const short ZOOM_OUT = 12;

        public const short PET_REPAIR_SHIP = 13;

        public object Object { get; set; }
        public Hotkey(short action, int keyCode, bool newClient) : this(action, keyCode, 0, newClient) { }

        public Hotkey(short action, int keyCode, int parameter, bool newClient)
        {
            if (newClient)
                Object = new netty.commands.new_client.UserKeyBindingsModule(action, new List<int> { keyCode }, parameter, 0);
            else Object = new netty.commands.old_client.UserKeyBindingsModule(action, new List<int> { keyCode }, parameter, 0);
        }

    }
}
