using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;

namespace NettyBaseReloaded.Main.global_managers
{
    class PerformanceManager
    {
        public enum State
        {
            MAX_PERFORMANCE,
            HIGH_PERFORMANCE_PLUS,
            HIGH_PERFORMANCE,
            HIGH_PERFORMANCE_MINUS,
            MEDIUM_PERFORMANCE_PLUS,
            MEDIUM_PERFORMANCE,
            MEDIUM_PERFORMANCE_MINUS,
            ECO_PLUS,
            ECO,
            ECO_MINUS
        }

        /// <summary>
        /// Based on server performance state it will add a thread delay
        /// </summary>
        public static double THREAD_DELAY = 0;
        public static State RunningState { get; set; }

        public static void MesaureLoad()
        {
            //TODO: Finish this
            //World.GetTotalEntries();
        }
    }
}
