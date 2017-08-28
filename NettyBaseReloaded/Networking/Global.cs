using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.global_storage;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Main
{
    static class Global
    {
        public static QueryManager QueryManager = new QueryManager();
        public static TickManager TickManager = new TickManager();
        public static StorageManager StorageManager = new StorageManager();
        public static DebugLog Log = new DebugLog("global");

        public static State State = State.LOADING;

        public static void Start()
        {
            InitiateGlobalQueries();
            InitiatePolicy();
            InitiateChat();
            InitiateGame();
            //TODO -> ACP InitiateSocketty();
            State = State.READY;
            TickManager.Tick();
        }

        static void InitiateGlobalQueries()
        {
            QueryManager.Load();
            if (Properties.Server.DEBUG)           
                Log.Write("Global-Queries loaded!");
        }

        static void InitiateChat()
        {
            Chat.Chat.Init();
            new Server(Server.CHAT_PORT);

            Out.WriteLine("Chat-Server started successfully and DB loaded!", "SUCCESS", ConsoleColor.DarkGreen);
            if (Properties.Server.DEBUG)
                Log.Write("Chat-Server started.");
        }

        static void InitiatePolicy()
        {
            new Server(Server.POLICY_PORT);

            Out.WriteLine("Policy-Server started successfully!", "SUCCESS", ConsoleColor.DarkGreen);
            if (Properties.Server.DEBUG)
                Log.Write("Policy-Server started.");
        }
        
        static void InitiateGame()
        {
            World.InitiateManagers();
            new Server(Server.GAME_PORT);

            Out.WriteLine("Game-Server started successfully and DB loaded!", "SUCCESS", ConsoleColor.DarkGreen);
            if (Properties.Server.DEBUG)
                Log.Write("Game-Server started.");
        }

        public static void SaveAll()
        {
            QueryManager.SaveAll();
            World.DatabaseManager.SaveAll();
        }
    }
}
