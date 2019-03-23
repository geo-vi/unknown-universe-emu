using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;

namespace NettyBaseReloaded.Helper.packets.handlers
{
    class DisconnectGameHandler : IHandler
    {
        public void Execute(HelperBrain brain, string[] packet)
        {
            SqlDatabaseManager.ConnectionString = "";
            SqlDatabaseManager.GlobalConnectionString = "";
        }
    }
}
