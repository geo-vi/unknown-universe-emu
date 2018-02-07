using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.global_managers
{
    class CronjobManager : ITick
    {
        public List<Cronjob> Cronjobs = new List<Cronjob>();

        public static void Initiate()
        {
            var cron = new CronjobManager
            {
                Cronjobs = Global.QueryManager.LoadCrons()
            };
            Global.TickManager.Add(cron);
        }
        /// <summary>
        /// Ticks the crons
        /// </summary>
        public void Tick()
        {
            foreach (var cron in Cronjobs)
            {
                cron.Tick();
            }
        }
    }
}
