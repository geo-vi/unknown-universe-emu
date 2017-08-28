using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded
{
    class Program
    {
        #region Sizing
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        public static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            DisableSizing();

            Console.SetOut(new Out());
            Console.CursorVisible = false;
            Console.Title = "Unknown Universe / " + GetVersion();

            Draw.Logo();
            InitiateServer();
            //ConsoleUpdater();
            ConsoleCommands.Add();
            KeepAlive();
            
        }

        /// <summary>
        /// If Exception wasn't handled it will just run this
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            // TODO: Save everything and then fuck up
        }

        /// <summary>
        /// This is disabling the Console from being resized
        /// * When Console gets resized it bugs *
        /// </summary>
        public static void DisableSizing()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
        }

        private static async void ConsoleUpdater()
        {
            while (true)
            {
                ConsoleMonitor.Check();
                ConsoleMonitor.Update();
                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Console commands handler
        /// </summary>
        private static void KeepAlive()
        {
            while (true)
            {
                var l = Console.ReadLine();
                if (l != "")
                {
                    if (l.StartsWith("/")) ConsoleCommands.Handle(l);
                }
            }
        }


        /// <summary>
        /// Version of the program
        /// </summary>
        /// <returns>Version</returns>
        public static string GetVersion()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            return string.Format(CultureInfo.InvariantCulture, @"{0}.{1}.{2}-R{3}", v.Major, v.Minor, v.Build, v.Revision);
        }

        /// <summary>
        /// This will look up if there are config files in the folder of the server
        /// </summary>
        static void LookForConfigFiles()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "/server.cfg")) ConfigFileReader.ReadServerConfig();
            if (File.Exists(Directory.GetCurrentDirectory() + "/game.cfg")) ConfigFileReader.ReadGameConfig();
            if (File.Exists(Directory.GetCurrentDirectory() + "/mysql.cfg")) ConfigFileReader.ReadMySQLConfig();
        }

        /// <summary>
        /// Starting the server up
        /// </summary>
        private static bool ServerUp = false;
        static void InitiateServer()
        {
            if (ServerUp) return;
            Console.WriteLine("Initiating..");
            LookForConfigFiles();
            Global.Start();
            ServerUp = true;
        }
    }
}
