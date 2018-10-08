using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Properties;

namespace NettyBaseReloaded.Utils
{
    class SessionDirCreator
    {
        public static readonly string PATH_LOG = Server.LOGGING_DIRECTORY + Program.SERVER_SESSION + "/log.txt";
        public static readonly string PATH_DBLOG = Server.LOGGING_DIRECTORY + Program.SERVER_SESSION + "/dblog.txt";
        public static readonly string PATH_PACT = Server.LOGGING_DIRECTORY + Program.SERVER_SESSION + "/pact.txt";

        public static void InitializeSession()
        {
            Directory.CreateDirectory(Server.LOGGING_DIRECTORY + Program.SERVER_SESSION);
            InitializeFile(PATH_LOG);
            InitializeFile(PATH_DBLOG);
            InitializeFile(PATH_PACT);
        }

        private static void InitializeFile(string path)
        {
            var writer = File.CreateText(path);
            writer.WriteLine("/*");
            writer.WriteLine(path);
            writer.WriteLine("Created @" + DateTime.Now + " by Emulator");
            writer.WriteLine("Emu Ver: " + Program.GetVersion());
            writer.WriteLine("*/");
            writer.Close();
        }

        public static void New(string filePath)
        {
            FileStream fileStream = null;
            if (!File.Exists(filePath))
                fileStream = File.Create(filePath);
            fileStream?.Dispose();
        }
    }
}
