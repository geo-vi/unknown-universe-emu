using Server.Configurations;
using Server.Main.managers;
using System;
using System.IO;

namespace Server.Utils
{
    class ConfigFileReader
    {
        private static readonly string[] NON_READABLE_CHARS = { "#", ";" };

        public static void ReadConfigurations()
        {
            Console.WriteLine("Looking for configuration files");
            if (File.Exists(Directory.GetCurrentDirectory() + "/server.cfg"))
                ReadServerConfig();
            if (File.Exists(Directory.GetCurrentDirectory() + "/game.cfg"))
                ReadGameConfig();
            if (File.Exists(Directory.GetCurrentDirectory() + "/mysql.cfg"))
                ReadMySQLConfig();
        }

        public static void ReadServerConfig()
        {
            Console.WriteLine("Reading server.cfg");
            var cfgFile = Directory.GetCurrentDirectory() + "/server.cfg";
            var reader = new StreamReader(cfgFile);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!line.StartsWith(NON_READABLE_CHARS[0]) && !line.StartsWith(NON_READABLE_CHARS[1]))
                {
                    if (line.Contains("="))
                    {
                        var paramSplit = line.Split('=');
                        switch (paramSplit[0])
                        {
                            default:
                                //TODO: Settings yet to be added
                                break;
                        }
                    }
                    else
                    {
                        switch (line)
                        {
                            case "PRINTING_CONNECTIONS":
                                ServerConfiguration.PRINTING_CONNECTIONS = true;
                                Console.WriteLine("Every connection will be printed to console");
                                break;
                        }
                    }
                }
            }
            Out.WriteLog("Finished reading server.cfg");
        }

        public static void ReadGameConfig()
        {
            Console.WriteLine("Reading game.cfg");
            var cfgFile = Directory.GetCurrentDirectory() + "/game.cfg";
            var reader = new StreamReader(cfgFile);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!line.StartsWith(NON_READABLE_CHARS[0]) && !line.StartsWith(NON_READABLE_CHARS[1]))
                {
                    if (line.Contains("="))
                    {
                        var paramSplit = line.Split('=');
                        switch (paramSplit[0])
                        {
                            case "TICKS_PER_SEC":
                                //TickManager.TICKS_PER_SECOND = Convert.ToInt16(paramSplit[1]);
                                break;
                        }
                    }
                }
            }
            Out.WriteLog("Finished reading game.cfg");
        }

        public static void ReadMySQLConfig()
        {
            Console.WriteLine("Reading mysql.cfg");
            var cfgFile = Directory.GetCurrentDirectory() + "/mysql.cfg";
            var reader = new StreamReader(cfgFile);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!line.StartsWith(NON_READABLE_CHARS[0]) && !line.StartsWith(NON_READABLE_CHARS[1]))
                {
                    if (line.Contains("="))
                    {
                        var paramSplit = line.Split('=');
                        switch (paramSplit[0])
                        {
                            case "SERVER":
                                SqlDatabaseManager.SERVER = paramSplit[1];
                                Console.WriteLine("SERVER:" + paramSplit[1]);
                                break;
                            case "UID":
                                SqlDatabaseManager.UID = paramSplit[1];
                                Console.WriteLine("UID:" + paramSplit[1]);
                                break;
                            case "PWD":
                                SqlDatabaseManager.PWD = paramSplit[1];
                                Console.WriteLine("PWD:" + paramSplit[1]);
                                break;
                            case "DB":
                                SqlDatabaseManager.DB = paramSplit[1];
                                Console.WriteLine("DB:" + paramSplit[1]);
                                break;
                            case "DB_EXT":
                                SqlDatabaseManager.DB_EXT = paramSplit[1];
                                Console.WriteLine("DB_EXT:" + paramSplit[1]);
                                break;
                        }
                    }
                }
            }
            Console.WriteLine("Finished reading mysql.cfg");
        }
    }
}
