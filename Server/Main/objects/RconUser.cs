using System;

namespace Server.Main.objects
{
    public class RconUser
    {
        /// <summary>
        /// Entry key used
        /// </summary>
        public string EntryKey { get; }
        
        /// <summary>
        /// IP of login
        /// </summary>
        public string IPAddress { get; }

        /// <summary>
        /// Time of login
        /// </summary>
        public DateTime LoginTime { get; }
        
        public RconUser(string entryKey, string ipAddress)
        {
            EntryKey = entryKey;
            IPAddress = ipAddress;
            LoginTime = DateTime.Now;
        }
    }
}