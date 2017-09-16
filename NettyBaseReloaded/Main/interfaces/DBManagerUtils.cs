using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main.global_managers;

namespace NettyBaseReloaded.Main.interfaces
{
    abstract class DBManagerUtils
    {
        internal DebugLog Log => SqlDatabaseManager.Log;

        internal int intConv(object i) => Convert.ToInt32(i);

        internal double doubleConv(object i) => Convert.ToDouble(i);

        internal long longConv(object i) => Convert.ToInt64(i);

        internal string stringConv(object i) => Convert.ToString(i);

        public abstract void Initiate();
    }
}
