using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static State State = State.LOADING;

        private static GameServer GameServer;

        public static void Start()
        {
            //InitiateStatusUpdater();
            InitiateGlobalQueries();
            InitiatePolicy();
            InitiateChat();
            InitiateGame();
            InitiateDiscord();
            InitiateWebSocks();
            InitiateRandomResetTimer();
            //TODO -> ACP InitiateSocketty();
            State = State.READY;
            Task.Factory.StartNew(() => {
                TickManager.Tick();
            }, TaskCreationOptions.LongRunning);
        }

        private static void InitiateStatusUpdater()
        {
            Cachet.handlers.CachetAuth.InitiateAuth();
        }

        static void InitiateGlobalQueries()
        {
            QueryManager.Load();
            Out.WriteLog("Global-Queries loaded!");
        }

        static void InitiateChat()
        {
            Chat.Chat.Init();
            new Server(Server.CHAT_PORT);

            Out.WriteLog("Chat-Server started successfully and DB loaded!", "SUCCESS");
            Out.WriteLog("Chat-Server started.");
        }

        static void InitiatePolicy()
        {
            new Server(Server.POLICY_PORT);

            Out.WriteLog("Policy-Server started successfully!", "SUCCESS");
            Out.WriteLog("Policy-Server started.");
        }
        
        static void InitiateGame()
        {
            World.InitiateManagers();
            GameServer = new GameServer(8080, 10);
            GameServer.StartAsync();

            Out.WriteLog("Game-Server started successfully and DB loaded!", "SUCCESS");
            Out.WriteLog("Game-Server started.");
        }

        private static void InitiateWebSocks()
        {
            WebSocks.packets.Handler.AddHandlers();
            WebSocketListener.InitiateListener();

            Out.WriteLog("WebSocks - ready to listen!", "SUCCESS");
            Out.WriteLog("WebSocks started.");
        }

        static void InitiateDiscord()
        {
            new Server(Server.DISCORD_PORT);

            Out.WriteLog("Discord-Server started successfully and DB loaded!", "SUCCESS");
            Out.WriteLog("Discord-Server started.");
        }

        static void InitiateRandomResetTimer()
        {
            int randomId = 0;
            TickManager.Add(RandomInstance.getInstance(new object()), out randomId);
        }

        public static void SaveAll()
        {
            QueryManager.SaveAll();
        }

        public static void Close()
        {
            foreach (var gameSession in World.StorageManager.GameSessions)
            {
                gameSession.Value.Kick();
            }
            QueryManager.SaveAll();
            Program.Exit();
        }
    }
}
