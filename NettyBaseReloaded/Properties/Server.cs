using System;

namespace NettyBaseReloaded.Properties
{
    public class Server
    {
        /// <summary>
        /// This should be for logging server info / crashes & cause of crash.
        /// </summary>
        public static bool LOGGING = true;

        public static string LOGGING_DIRECTORY = ".\\logs\\";

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

        /// <summary>
        /// Server is in DEBUG mode
        /// - will print out every possible shit
        /// </summary>
        public static bool DEBUG = false;

        /// <summary>
        /// End of public beta (only whitelisted can login)
        /// </summary>
        public static DateTime PUBLIC_BETA_END = new DateTime(2017, 6, 28, 11, 30, 0);

        public static DateTime RUNTIME = new DateTime(2017, 6, 28, 11, 30, 0);
    }
}