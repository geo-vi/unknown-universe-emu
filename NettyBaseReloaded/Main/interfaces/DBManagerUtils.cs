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
        internal int intConv(object i)
        {
            if (i == DBNull.Value) return 0;
            return Convert.ToInt32(i);
        }

        internal double doubleConv(object i) => Convert.ToDouble(i);

        internal long longConv(object i) => Convert.ToInt64(i);

        internal string stringConv(object i) => Convert.ToString(i);

        public abstract void Initiate();
    }
}
