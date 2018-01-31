using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NettyBaseReloaded.Main.objects
{
    class Cronjob
    {
        public string Name { get; set; }

        public DateTime ExecutionTime { get; set; }

        public bool RepeatedTask { get; set; }

        public int Intervals { get; set; }

        public string ExecuteStr { get; set; }

        public void Execute()
        {
            if (ExecuteStr.StartsWith("run"))
            {
                var path = ExecuteStr.Split('-', '>');
                Console.WriteLine(path[1]);
            }

            if (!RepeatedTask)
            {
                Remove();
            }
        }

        public static Cronjob ParseCronjob(XmlDocument xml)
        {
            throw new NotImplementedException();
        }

        public void Tick()
        {
            if (ExecutionTime.AddMinutes(Intervals) < DateTime.Now) Execute();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
