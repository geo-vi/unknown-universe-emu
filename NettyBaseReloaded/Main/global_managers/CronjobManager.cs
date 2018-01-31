using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.global_managers
{
    class CronjobManager : ITick
    {
        public List<Cronjob> Cronjobs = new List<Cronjob>();

        public void GetAllCrons()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "/crons.xml")) return;
            var xml = XDocument.Load(Directory.GetCurrentDirectory() + "/crons.xml");
            foreach (var cronjob in xml.Descendants())
            {
                
            }
        }

        public void Tick()
        {
            foreach (var cron in Cronjobs)
            {
                cron.Tick();
            }
        }
    }
}
