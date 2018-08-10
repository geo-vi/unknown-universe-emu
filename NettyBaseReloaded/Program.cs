using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using NettyBaseReloaded.Logger;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Properties;
using NettyBaseReloaded.Utils;
using Newtonsoft.Json;

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

        public static string SERVER_SESSION = "";

        public static DebugLog Log;

        public static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            Application.ThreadException += ApplicationOnThreadException;
            Application.CurrentCulture = new CultureInfo("en-US");

            InitiateConsole();
            //InitiatePanel();
        }


        private static void InitiateConsole()
        {
            DisableSizing();

            //Console.SetOut(new Out());
            Console.CursorVisible = false;

            Draw.Logo();

            //TODO: Add QuestBuilder();
            //RewardBuilder();
            InitiateSession();
            ConsoleUpdater();
            ConsoleCommands.Add();
            KeepAlive();
        }

        private static void InitiatePanel()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Controller());
        }

        public static void RewardBuilder()
        {
            Console.WriteLine("Entered reward builder mode..");
            Console.WriteLine("Setup adding items");
            var itemDictionary = new List<Tuple<string, int>>(); // lootid - amount
            while (true)
            {
                var l = Console.ReadLine();
                if (l != "")
                {
                    // ADD ITEM
                    var itemId = l;
                    int amount = 0;
                    if (l.Contains(' ') && int.TryParse(l.Split(' ')[1], out amount))
                    {
                        itemId = l.Split(' ')[0];
                    }
                    else amount = int.Parse(Console.ReadLine());
                    Console.WriteLine("ItemId: " + itemId);
                    Console.WriteLine("Amount: " + amount);
                    Console.WriteLine("Add?");
                    var readKey = Console.ReadKey();
                    if (readKey.Key == ConsoleKey.Enter)
                    {
                        itemDictionary.Add(new Tuple<string, int>(itemId, amount));
                        Console.WriteLine("Added item to dictionary");
                    }
                }
                else
                {
                    Console.WriteLine("Finished?");
                    var readKey = Console.ReadKey();
                    if (readKey.Key == ConsoleKey.Enter)
                    {
                        var result = JsonConvert.SerializeObject(itemDictionary);
                        Console.WriteLine("Result:\n" + result);
                        Console.WriteLine("Copy to clipboard?");
                        readKey = Console.ReadKey();
                        if (readKey.Key == ConsoleKey.Enter)
                        {
                            Clipboard.SetText(result);
                        }
                        Environment.Exit(0);
                    }
                }
            }
        }

        /// <summary>
        /// If Exception wasn't handled it will just run this
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            new ExceptionLog("unhandled", $"Unhandled exception trapped and logged\nProgram terminated {e.IsTerminating}", e.ExceptionObject as Exception);
            Environment.Exit(0);
            // TODO: Save everything and then fuck up
        }


        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            new ExceptionLog("thread_exception", $"Unhandled thread exception trapped and logged", threadExceptionEventArgs.Exception);
            Environment.Exit(0);
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
            Server.RUNTIME = DateTime.Now;

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
            Log.Write("Looking for config files");
            if (File.Exists(Directory.GetCurrentDirectory() + "/server.cfg")) ConfigFileReader.ReadServerConfig();
            if (File.Exists(Directory.GetCurrentDirectory() + "/game.cfg")) ConfigFileReader.ReadGameConfig();
            if (File.Exists(Directory.GetCurrentDirectory() + "/mysql.cfg")) ConfigFileReader.ReadMySQLConfig();
        }

        public static void InitiateSession()
        {
            SERVER_SESSION = Encode.MD5(DateTime.Now.ToLongTimeString());
            InitiateServer();
        }

        /// <summary>
        /// Starting the server up
        /// </summary>
        public static bool ServerUp = false;
        static void InitiateServer()
        {
            if (ServerUp) return;
            Console.WriteLine("Initiating..");
            LoadLogger();
            LookForConfigFiles();
            Global.Start();
            ServerUp = true;
            Server.RUNTIME = DateTime.Now;
        }

        public static void CloseForMaintenance()
        {
            Console.WriteLine("Entering critical mode (Over 100 exceptions recorded)");
            Properties.Server.DEBUG = true;
        }

        static void LoadLogger()
        {
            if (!Server.LOGGING) return;

            Creator.InitializeSession();
            Log = new DebugLog("core");
            Log.Write("Logger succesfully loaded.");
            Log.Write("Testing... 1 2 3");
        }

        static void ParseXML()
        {
            var xml = XDocument.Load("http://univ3rse.com/flashinput/translationTitles.php");
            foreach (var element in xml.Descendants())
            {
                var titleId = element.Attribute("name")?.Value;
                var id = titleId?.Replace("title_", "");
                Console.WriteLine($"INSERT INTO server_titles VALUES ('{id}', '{titleId}','{element.Value}','0','#FFF');");
            }
            Console.Read();
            //INSERT INTO server_titles (KEY,TITLE_NAME,TITLE_COLOR_HEX) VALUES ("", "","#FFF");
        }

        public static void Exit()
        {
            Environment.Exit(0);
        }
    }
}
