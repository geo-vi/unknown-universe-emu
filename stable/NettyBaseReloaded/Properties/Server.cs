using System;

namespace NettyBaseReloaded.Properties
{
    public class Server
    {
        /// <summary>
        /// This should be for logging server info / crashes & cause of crash.
        /// </summary>
        public static bool LOGGING = false;

        public static string LOGGING_DIRECTORY = ".\\logs\\";

        /// <summary>
        /// Is server running
        /// </summary>
        public static bool IS_READY = false;

        /// <summary>
        /// Is server locked
        /// </summary>
        public static bool LOCKED = false;

        /// <summary>
        /// Server password
        /// </summary>
        public static string PASSWORD = "";

        /// <summary>
        /// Server RCON passwrd
        /// </summary>
        public static string RCON_PW = "";

        public static int TOTAL_CONNECTED_PLAYERS = 0;

        public static bool DEBUG = false;

        public static DateTime PUBLIC_BETA_END = new DateTime(2017, 6, 28, 11, 30, 0);
    }
}