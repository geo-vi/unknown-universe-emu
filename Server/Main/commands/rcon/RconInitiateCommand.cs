using System;

namespace Server.Main.commands.rcon
{
    class RconInitiateCommand
    {
        public void Execute(string[] args = null)
        {
            var consoleUser = Global.StorageManager.ConsoleUser;
            switch (args[1])
            {
                case "login":
                    if (consoleUser == null)
                    {
                        Console.WriteLine("Console user should be logged in by default and if it isn't, error occured");
                    }
                    else
                    {
                        Console.WriteLine("Already logged in with entry key: " + consoleUser.EntryKey);
                    }
                    break;
                case "logout":
                    if (consoleUser != null)
                    {
                        Console.WriteLine("No access to logout");
                    }
                    break;
            }
        }
    }
}
