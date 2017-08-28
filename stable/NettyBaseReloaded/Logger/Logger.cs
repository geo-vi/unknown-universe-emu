using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Logger.managers;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Logger
{
    static class Logger
    {
        public static FileManager FileManager;
        public static ReadingManager ReadingManager = new ReadingManager();
        public static WritingManager WritingManager = new WritingManager();

        public static void Start()
        {
            FileManager = new FileManager(GetActualDirectory());
            Out.WriteLine("Logger Started", "SUCCESS", ConsoleColor.DarkGreen);
        }

        private static string GetActualDirectory()
        {
            var directory = Properties.Server.LOGGING_DIRECTORY;
            if (directory.StartsWith("./"))
                directory = Directory.GetCurrentDirectory() + "/" + Properties.Server.LOGGING_DIRECTORY.Replace("./", "");

            return directory;
        }
    }
}
