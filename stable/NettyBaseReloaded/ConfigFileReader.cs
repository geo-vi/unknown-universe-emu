using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Main;
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
                                TickManager.TICKS_PER_SECOND = Convert.ToInt16(paramSplit[1]);
                                break;
                        }
                    }
                }
            }
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
                                MySQLManager.SERVER = paramSplit[1];
                                break;
                            case "UID":
                                MySQLManager.UID = paramSplit[1];
                                break;
                            case "PWD":
                                MySQLManager.PWD = paramSplit[1];
                                break;
                            case "DB":
                                MySQLManager.DB = paramSplit[1];
                                break;
                        }
                    }
                }
            }
        }
    }
}
