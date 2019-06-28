using Server.Game.netty;
using Server.Main.managers;
using Server.Networking;
using Server.Utils;

namespace Server.Main
{
    class Global
    {
        public static CommandManager CommandManager = new CommandManager();
        public static TickManager TickManager = new TickManager();
        public static StorageManager StorageManager = new StorageManager();

        private static GameServer GameServer;

        public static void Initiate()
        {
            InitiateManagers();
            InitiateHelper();
            InitiateGame();
            InitiateChat();
            InitiateWebCommunicator();
        }

        private static void InitiateManagers()
        {
            CommandManager.CreateCommands();
        }

        private static void InitiateHelper()
        {

        }

        private static void InitiateGame()
        {
            Packet.Handler.AddCommands();
            GameServer = new GameServer(8080, 10);
            GameServer.StartAsync();
            Out.WriteLog("Game-Server started successfully and DB loaded!", "SUCCESS");
            Out.WriteLog("Server started.", "GAME");

        }

        private static void InitiateChat()
        {

        }

        private static void InitiateWebCommunicator()
        {

        }
    }
}
