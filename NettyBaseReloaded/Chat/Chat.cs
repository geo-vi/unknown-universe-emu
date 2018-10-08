using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.managers;

namespace NettyBaseReloaded.Chat
{
    class Chat
    {
        public static StorageManager StorageManager = new StorageManager();
        public static DatabaseManager DatabaseManager = new DatabaseManager();

        public static void Init()
        {
            InitiateManagers();
            LoadBots();
        }

        private static void InitiateManagers()
        {
            DatabaseManager.Initiate();
        }

        private static void LoadBots()
        {
            
        }

    }
}
