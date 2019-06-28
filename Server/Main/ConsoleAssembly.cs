using System;

namespace Server.Main
{
    class ConsoleAssembly
    {
        /// <summary>
        /// This will be called after everything is loaded
        /// </summary>
        public static void Initiate()
        {
            DisplayShortOutro();
            TitleUpdate();

            while (true)
            {
                ListenerForCommands();
            }
        }

        private static void ListenerForCommands()
        {
            var l = Console.ReadLine();
            if (l != "")
            {
                if (l.StartsWith("/"))
                    Global.CommandManager.HandleConsoleInput(l);
            }
        }

        private static void TitleUpdate()
        {
            Console.Title = "Unknown Universe 7.0";
        }

        private static void DisplayShortOutro()
        {
            Console.WriteLine("Everything finished, environment is ready.");
            Console.WriteLine("Starting command listener, to invoke type with /");
            Console.WriteLine("------------------------------------------------");
        }

        public static void Intro()
        {
            Console.WriteLine("Starting Unknown Universe 7.0");
            Console.WriteLine("Written on .NET Core 2.1");
            Console.WriteLine("Originally ported from .NET Framework 4.7");
            Console.WriteLine("Coded by Heisenberg");
            Console.WriteLine(">>> Starting...");
            Console.WriteLine();
            Console.WriteLine("Time of start: " + Program.ServerStartTime);
            Console.WriteLine();
        }
    }
}
