using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.packet;

namespace NettyBaseReloaded.Game
{
    class World
    {    
        public static managers.StorageManager StorageManager = new StorageManager();
        public static managers.DatabaseManager DatabaseManager = new DatabaseManager();

        public static void InitiateManagers()
        {
            Packet.Handler.AddCommands();
            DatabaseManager.BasicLoads();
        }
    }
}
