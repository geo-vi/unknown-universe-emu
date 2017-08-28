using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded
{
    static class Random
    {
        private static readonly System.Random _rnd = new System.Random();
        public static int Next(int n1, int n2)
        {
            return _rnd.Next(n1, n2);
        }

        public static int Next(int n)
        {
            return _rnd.Next(n);
        }
    }
}
