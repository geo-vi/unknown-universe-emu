using Server.Utils;
using System;
using System.Diagnostics;
using System.Threading;

namespace Server.Main.managers
{
    class QueryManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns>If connection is established</returns>
        public bool CheckConnection()
        {
            int tries = 0;
        TRY:
            try
            {
                SqlDatabaseManager.Initialize();
                Out.WriteDbLog("Successfully connected to database");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("MySql Error: " + e.Message);
                Console.WriteLine("MYSQL Connection failed.");
                Out.WriteDbLog("MySQL Connection failed");
                if (tries < 6)
                {
                    Console.WriteLine("Trying to reconnect in .. " + tries + " seconds.");
                    Thread.Sleep(tries * 1000);
                    tries++;
                    goto TRY;
                }
            }
            return false;
        }
    }
}
