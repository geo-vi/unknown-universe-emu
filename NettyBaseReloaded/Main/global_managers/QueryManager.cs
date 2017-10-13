using System;
using System.Threading;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.global_managers
{
    class QueryManager
    {
        public void Load()
        {
            if (!CheckConnection())
            {
                throw new Exception("Couldn't establish MYSQL connection");
            }
            LoadClans();
        }

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
                SqlDatabaseManager.Log.Write("Successfully connected to database");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("MYSQL Connection failed.");
                SqlDatabaseManager.Log.Write("MySQL Connection failed");
                new ExceptionLog("mysql", "MYSQL Connection failed", e);
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

        public void LoadClans()
        {
            Global.StorageManager.Clans.Add(0, new Clan(0, "", ""));
            Global.StorageManager.Clans.Add(1, new Clan(1, "Administrators", "ADM"));
            Global.StorageManager.Clans.Add(2, new Clan(2, "Developers", "DEV"));
            foreach (var clan in Global.StorageManager.Clans)
                clan.Value.LoadDiplomacy();
        }

        public void SaveAll()
        {

        }
    }
}
