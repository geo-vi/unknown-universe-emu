using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Logger.managers
{
    class FileManager
    {
        private string Directory { get; }

        private string SessionFile { get; set; }

        public FileManager(string directory)
        {
            Directory = InitiateDir(directory);
            CreateSessionFile();
        }

        private string InitiateDir(string directory)
        {
            if (!System.IO.Directory.Exists(directory + "\\" + DateTime.Now.Date.ToString("dd-MM-yy") + "\\"))
                System.IO.Directory.CreateDirectory(directory + "\\" + DateTime.Now.Date.ToString("dd-MM-yy") + "\\");

            return directory + DateTime.Now.Date.ToString("dd-MM-yy") + "\\";
        }

        private void CreateSessionFile()
        {
            var timeString = DateTime.Now.ToString();
            var name = Encode.MD5(timeString);

            SessionFile = name;

            if (File.Exists(Directory + "\\" + name)) return;
            var file = File.Create(Directory + "\\" + name);
            file.Close();
        }

        public string GetSessionFileDir()
        {
            if (Directory.StartsWith(".")) return Directory.Replace(".", System.IO.Directory.GetCurrentDirectory()) + SessionFile;
            return Directory + SessionFile;
        }
    }
}
