using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Logger.managers
{
    class ReadingManager
    {
        public string ReadAll()
        {
            var stream = new StreamReader(Logger.FileManager.GetSessionFileDir());
            var text = stream.ReadToEnd();
            stream.Close();
            return text;
        }

        public string Read(string content)
        {
            var stream = new StreamReader(Logger.FileManager.GetSessionFileDir());
            string line = "";
            string full = "";
            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                if (line.Contains(content))
                {
                    full += line + "\n";
                }
            }
            if (full != "") return full;
            return "Nothing found..";
        }

        public string ReadByTime(string time)
        {
            var stream = new StreamReader(Logger.FileManager.GetSessionFileDir());
            string line = "";
            string full = "";
            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                var splitTime = line.Split('-')[0];
                if (splitTime.Contains(time))
                {
                    full += line + "\n";
                }
            }
            if (full != "") return full;
            return "Nothing found..";
        }

        public string Read(int userId)
        {
            var stream = new StreamReader(Logger.FileManager.GetSessionFileDir());
            string line = "";
            string full = "";
            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                var splitTime = line.Split('-')[1];
                if (splitTime.Contains(userId.ToString()))
                {
                    full += line + "\n";
                }
            }
            if (full != "") return full;
            return "Nothing found..";
        }
    }
}
