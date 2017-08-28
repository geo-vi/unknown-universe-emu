using System;
using System.Text;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Main.commands
{
    class Help : Command
    {
        public Help() : base("help", "Lists all the commands", false)
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args != null)
                Args = args;

            if (Args == null)
            {
                Print();
            }
            else
            {
                foreach (var arg in Args)
                {
                    try
                    {
                        PrintPage(int.Parse(arg));
                    }
                    catch (Exception)
                    {
                        if (arg != "help")
                        {
                            PrintCmd(arg);
                            return;
                        }
                    }
                }
            }

            Args = null;
        }

        public void Print()
        {
            Console.WriteLine(" -- - HELP - -- ");
            Console.WriteLine();

            StringBuilder finalPrint = new StringBuilder();

            int i = 0;
            foreach (var cmd in ConsoleCommands.Commands)
            {
                if (cmd.Value.Display)
                    finalPrint.Append("/" + cmd.Key + " ");

                if (i > 5)
                {
                    Console.WriteLine(finalPrint);
                    finalPrint.Clear();
                }

                i++;
            }

            Console.WriteLine(finalPrint);
            Console.WriteLine(Environment.NewLine + "For looking up a command /help {command}");
        }

        public void PrintCmd(string cmd)
        {
            var command = ConsoleCommands.Commands[cmd];
            if (command == null) return;

            Console.WriteLine("{0} - {1}", command.Name, command.Description);
            if (command.HelpingParams != null)
            {
                foreach (var helpingParam in command.HelpingParams)
                {
                    if (helpingParam.Display)
                        Console.WriteLine("> {0} - {1}", helpingParam.Name, helpingParam.Desc);
                }
            }
        }

        public void PrintPage(int page)
        {

        }
    }
}