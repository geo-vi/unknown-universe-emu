using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace NettyBaseReloaded.Logger.objects
{
    class LogFile
    {
        /// <summary>
        /// Path of file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Queue for writing
        /// </summary>
        public ConcurrentQueue<string> QueuedTextWriter = new ConcurrentQueue<string>();

        public bool IsOpen = false;

        private StreamWriter Writer;

        public LogFile(string path)
        {
            Path = path;
        }

        public void ProcessQueue()
        {
            using (FileStream fs = new FileStream(@Path
                , FileMode.Append
                , FileAccess.Write))
            {
                StreamWriter tw = new StreamWriter(fs);

                IsOpen = true;
                string txtRemoved;
                foreach (var queuedLog in QueuedTextWriter)
                {
                    tw.WriteLine(queuedLog);
                    QueuedTextWriter.TryDequeue(out txtRemoved);
                }
                tw.Flush();
                IsOpen = false;
            }
        }
    }
}
