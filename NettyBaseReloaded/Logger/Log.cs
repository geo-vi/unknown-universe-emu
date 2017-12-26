using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Logger
{
    abstract class Log
    {
        public const string BASE_DIR = "/logs";

        protected Writer Writer { get; set; }

        public static DateTime LastLogTime { get; set; }

        public abstract void Initialize(string fileName = "");
    }
}
