using Server.Main;
using Server.Utils;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Server.Game.netty;
using Server.Game.netty.handlers;
using Server.Game.objects.entities;
using Server.Networking;

[assembly: InternalsVisibleTo("Server.Tests")]
namespace Server
{
    class Program
    {
        public static readonly DateTime ServerStartTime = DateTime.Now;

        static void Main(string[] args)
        {
            
            ConsoleAssembly.Intro();
            InitiateServer();
//            var task = Task.Factory.StartNew(InitiateServer);
//            task.Wait();
        }

        static void InitiateServer()
        {
            ConfigFileReader.ReadConfigurations();
            Global.Initiate();
            ConsoleAssembly.Initiate();
        }
    }
}
