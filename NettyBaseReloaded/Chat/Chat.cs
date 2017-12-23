using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.commands;
using NettyBaseReloaded.Chat.managers;

namespace NettyBaseReloaded.Chat
{
    class Chat
    {
        public static StorageManager StorageManager = new StorageManager();
        public static DatabaseManager DatabaseManager = new DatabaseManager();

        public static Dictionary<string, Command> HandledCommands = new Dictionary<string, Command>();

        public static void Init()
        {
            AddCommands();
            InitiateManagers();
        }

        private static void AddCommands()
        {

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
