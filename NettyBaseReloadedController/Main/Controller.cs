using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::NettyBaseReloadedController.Main.global;
using NettyBaseReloadedController.Main.netty;

namespace NettyBaseReloadedController.Main
{
    class Controller
    {
        public static Global Global = new Global();
        public static void Initiate()
        {
            Global.AddMaps();
            Global.TickManager.Tick();
            CommandHandler.AddCommands();

        }
    }
}
