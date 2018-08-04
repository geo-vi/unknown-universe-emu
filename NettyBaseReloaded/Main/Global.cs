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
using NettyBaseReloaded.Utils;
using NettyBaseReloaded.WebSocks;

namespace NettyBaseReloaded.Main
{
    static class Global
    {
        public static QueryManager QueryManager = new QueryManager();
        public static TickManager TickManager = new TickManager();
        public static StorageManager StorageManager = new StorageManager();
        public static CronjobManager CronjobManager = new CronjobManager();
        public static DebugLog Log = new DebugLog("global");

        public static State State = State.LOADING;

        public static void Start()
        {
            InitiateGlobalQueries();
            InitiatePolicy();
            InitiateChat();
            InitiateGame();
            InitiateDiscord();
            InitiateWebSocks();
            InitiateRandomResetTimer();
            //TODO -> ACP InitiateSocketty();
            State = State.READY;
            CronjobManager.Initiate();
            var task = new Task(() => TickManager.Tick(), TaskCreationOptions.LongRunning);
            task.Start();
            Out.WriteLine("Ready for work.", "GLOBAL");
        }

        static void InitiateGlobalQueries()
        {
            QueryManager.Load();
            Log.Write("Global-Queries loaded!");
        }

        static void InitiateChat()
        {
            Chat.Chat.Init();
            new Server(Server.CHAT_PORT);

            Log.Write("Chat-Server started.");
        }

        static void InitiatePolicy()
        {
            new Server(Server.POLICY_PORT);

            Log.Write("Policy-Server started.");
        }
        
        static void InitiateGame()
        {
            World.InitiateManagers();
            new Server(Server.GAME_PORT);

            Log.Write("Game-Server started.");
        }

        private static void InitiateWebSocks()
        {
            WebSocks.packets.Handler.AddHandlers();
            WebSocketListener.InitiateListener();

            Log.Write("WebSocks started.");
        }

        static void InitiateDiscord()
        {
            new Server(Server.DISCORD_PORT);

            Log.Write("Discord-Server started.");
        }

        static void InitiateRandomResetTimer()
        {
            TickManager.Add(RandomInstance.getInstance(new object()));
        }

        public static void SaveAll()
        {
            QueryManager.SaveAll();
        }
    }
}
