using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NettyStatusBot.modules;
using NettyStatusBot.Properties;

namespace NettyStatusBot.core
{
    class DiscordLogin
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task Run()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();
            new Updater(_client);

            await RegisterCommands();
            await _client.LoginAsync(TokenType.Bot, BotConfiguration.TOKEN);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task RegisterCommands()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            await _commands.AddModuleAsync<Ping>();
            await _commands.AddModuleAsync<Maintenance>();
            await _commands.AddModuleAsync<Refresh>();
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (arg is SocketUserMessage msg)
            {
                int argPos = 0;
                var context = new SocketCommandContext(_client, msg);
                if (context.IsPrivate && arg.Author != _client.CurrentUser)
                {
                    var result = await _commands.ExecuteAsync(context, argPos, _services);
                    if (!result.IsSuccess)
                    {
                        Console.WriteLine("Error ocurred \n" + result.Error + "\n" + result.ErrorReason);
                    }
                }
            }
        }
    }
}
