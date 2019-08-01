using System;

namespace Server.Game.objects.implementable
{
    abstract class AbstractDbUtils
    {
        internal int intConv(object i)
        {
            if (i == DBNull.Value) return 0;
            return Convert.ToInt32(i);
        }

        internal double doubleConv(object i) => Convert.ToDouble(i);

        internal long longConv(object i) => Convert.ToInt64(i);

        internal string stringConv(object i) => Convert.ToString(i);
    }
}