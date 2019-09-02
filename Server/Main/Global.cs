using System.Threading;
using System.Threading.Tasks;
using Server.Game;
using Server.Game.netty;
using Server.Main.managers;
using Server.Main.objects;
using Server.Networking;
using Server.Networking.servers;
using Server.Utils;

namespace Server.Main
{
    class Global
    {
        public static readonly CommandManager CommandManager = new CommandManager();
        public static readonly TickManager TickManager = new TickManager();
        public static readonly QueryManager QueryManager = new QueryManager();
        public static readonly StorageManager StorageManager = new StorageManager();
        public static readonly ServerManager ServerManager = new ServerManager();
        public static readonly ConsoleStat ConsoleStatistics = new ConsoleStat();
        
        public static void Initiate()
        {
            InitiateManagers();
            InitiateHelper();
            InitiatePolicy();
            InitiateGame();
            InitiateChat();
            InitiateWebCommunicator();
            
            //Create all servers, allow connection
            ServerManager.InitiateAll();
        }

        private static void InitiateManagers()
        {
            CommandManager.CreateCommands();
            QueryManager.Initiate();
            TickManager.Initialize();
        }

        private static void InitiateHelper()
        {

        }

        private static void InitiatePolicy()
        {
            var policyServer = new PolicyServer();
            ServerManager.Create(policyServer);
            
            Out.WriteLog("Policy-Server created successfully!", LogKeys.INFO);
        }
        
        private static void InitiateGame()
        {
            World.InitiateManagers();
            Packet.Handler.AddCommands();
            Packet.Builder.AddCommands();
            var gameServer = new GameServer();
            ServerManager.Create(gameServer);

            Out.WriteLog("Game-Server created successfully and DB loaded!", LogKeys.INFO);
        }

        private static void InitiateChat()
        {

        }

        private static void InitiateWebCommunicator()
        {

        }
    }
}
