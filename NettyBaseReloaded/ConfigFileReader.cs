using System;
using System.IO;
using NettyBaseReloaded.Main.global_managers;

namespace NettyBaseReloaded
{
    static class ConfigFileReader
    {
        private static readonly string[] NON_READABLE_CHARS = { "#", ";" };

        public static void ReadServerConfig()
        {
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
                            case "LOGGING_DIRECTORY":
                                Properties.Server.LOGGING_DIRECTORY = paramSplit[1];
                                break;
                            case "SERVER_PW":
                                Properties.Server.PASSWORD = paramSplit[1];
                                break;
                            case "SERVER_RCON_PW":
                                Properties.Server.RCON_PW = paramSplit[1];
                                break;
                        }
                    }
                    else
                    {
                        switch (line)
                        {
                            case "LOGGING":
                                Properties.Server.LOGGING = true;
                                break;
                            case "SERVER_DEBUG":
                                Properties.Server.DEBUG = true;
                                break;
                            case "SERVER_LOCKED":
                                Properties.Server.LOCKED = true;
                                break;
                        }
                    }
                }
            }
            Out.WriteLog("Finished reading server.cfg");
        }

        public static void ReadGameConfig()
        {
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
