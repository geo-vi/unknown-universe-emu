using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Logger.objects;

namespace NettyBaseReloaded.Logger
{
    class Logger
    {
        public static Logger _instance;

        public Dictionary<string,LogFile> LogFiles = new Dictionary<string, LogFile>();
 
        public Logger(Dictionary<string,string> paths)
        {
            foreach (var path in paths)
            {
                LogFiles.Add(path.Key, new LogFile(path.Value));
            }

            Task.Factory.StartNew(AsynchronousWriter);
        }

        private async void AsynchronousWriter()
        {
            while (true)
            {
                foreach (var logFile in LogFiles)
                {
                    logFile.Value.ProcessQueue();
                    await Task.Delay(1000);
                }
            }
        }

        public void Enqueue(string key, string text)
        {
            Debug.WriteLine("[" + key + "] " + text);
            if (LogFiles.ContainsKey(key))
            {
                LogFiles[key].QueuedTextWriter.Enqueue(text);
            }
        }
    }
}
