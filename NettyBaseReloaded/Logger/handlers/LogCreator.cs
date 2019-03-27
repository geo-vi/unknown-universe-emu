using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Properties;

namespace NettyBaseReloaded.Logger.handlers
{
    class LogCreator
    {
        public static readonly string PATH_LOG = Server.LOGGING_DIRECTORY + Program.SERVER_SESSION + "/log.txt";
        public static readonly string PATH_DBLOG = Server.LOGGING_DIRECTORY + Program.SERVER_SESSION + "/dblog.txt";
        public static readonly string PATH_PACT = Server.LOGGING_DIRECTORY + Program.SERVER_SESSION + "/pact.txt";
        public static readonly string PATH_ANTICHEAT = Server.LOGGING_DIRECTORY + Program.SERVER_SESSION + "/anticheat.txt";

        public static void Initialize()
        {
            Directory.CreateDirectory(Server.LOGGING_DIRECTORY + Program.SERVER_SESSION);
            New(PATH_LOG);
            New(PATH_DBLOG);
            New(PATH_PACT);
            New(PATH_ANTICHEAT);
            Logger._instance = new Logger(new Dictionary<string, string>
            {
                {"log", PATH_LOG },
                {"dblog", PATH_DBLOG },
                {"pact", PATH_PACT },
                {"anticheat", PATH_ANTICHEAT }
            });
            foreach (var logfile in Logger._instance.LogFiles)
            {
                var builder = new StringBuilder();
                builder.AppendLine("/*");
                builder.AppendLine(logfile.Value.Path);
                builder.AppendLine("Created @" + DateTime.Now + " by Emulator");
                builder.AppendLine("Emu Ver: " + Program.GetVersion());
                builder.AppendLine("*/");
                Logger._instance.Enqueue(logfile.Key, builder.ToString());
            }
        }

        private static void New(string filePath)
        {
            FileStream fileStream = null;
            if (!File.Exists(filePath))
                fileStream = File.Create(filePath);
            fileStream?.Dispose();
        }
    }
}
