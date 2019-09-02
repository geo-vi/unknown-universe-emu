using System;
using Server.Main;
using Server.Main.commands.server;
using Xunit;

namespace Server.Tests.console
{
    public class ConsoleCommandTests
    {
        [Fact]
        public void ConsoleCommandHandler_Test()
        {
            Global.CommandManager.CreateCommands();
            foreach (var command in Global.CommandManager.RegisteredConsoleCommands)
            {
                Global.CommandManager.HandleConsoleInput("/" + command.Key);
            }
        }

        [Fact]
        public void RuntimeCommand_Test()
        {
            var runtime = new RuntimeCommand();
            runtime.Execute();
        }

        [Fact]
        public void HelpCommand_Test()
        {
            var help = new HelpCommand();
            help.Execute(new []{ "runtime"});
        }
    }
}