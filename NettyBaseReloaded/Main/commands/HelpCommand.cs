using System;
using System.Text;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Main.commands
{
    class HelpCommand : Command
    {
        public HelpCommand() : base("commands", "Lists all the commands", false)
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args != null)
                Args = args;

            if (Args == null)
            {
                Console.WriteLine(Print());
            }
            else
            {
                foreach (var arg in Args)
                {
                    try
                    {
                        Console.WriteLine(PrintPage(int.Parse(arg)));
                    }
                    catch (Exception)
                    {
                        if (arg != "help")
                        {
                            Console.WriteLine(PrintCmd(arg));
                            return;
                        }
                    }
                }
            }

            Args = null;
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            if (args != null)
                Args = args;

            if (Args == null)
            {
                MessageController.System(session.Player, Print().ToString());
            }
            else
            {
                foreach (var arg in Args)
                {
                    try
                    {
                        MessageController.System(session.Player, PrintPage(int.Parse(arg)).ToString());
                    }
                    catch (Exception)
                    {
                        if (arg != "commands")
                        {
                            MessageController.System(session.Player, PrintCmd(arg).ToString());
                            return;
                        }
                    }
                }
            }

            Args = null;
        }


        public StringBuilder Print()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(" -- - HELP - -- ");
            builder.AppendLine();

            StringBuilder finalPrint = new StringBuilder();

            int i = 0;
            foreach (var cmd in ConsoleCommands.Commands)
            {
                if (cmd.Value.Display)
                    finalPrint.Append("/" + cmd.Key + " ");

                if (i > 5)
                {
                    builder.AppendLine(finalPrint.ToString());
                    finalPrint.Clear();
                }

                i++;
            }

            builder.AppendLine(finalPrint.ToString());
            builder.AppendLine(Environment.NewLine + "For looking up a command /help {command}");
            return builder;
        }

        public StringBuilder PrintCmd(string cmd)
        {
            StringBuilder builder = new StringBuilder();
            var command = ConsoleCommands.Commands[cmd];
            if (command == null) return builder;

            builder.AppendLine($"{command.Name} - {command.Description}");
            if (command.HelpingParams != null)
            {
                foreach (var helpingParam in command.HelpingParams)
                {
                    if (helpingParam.Display)
                        builder.AppendLine($"> {helpingParam.Name} - {helpingParam.Desc}");
                }
            }

            return builder;
        }

        public StringBuilder PrintPage(int page)
        {
            return null;
        }
    }
}