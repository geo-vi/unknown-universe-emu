using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NettyStatusBot.core;

namespace NettyStatusBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new DiscordLogin().Run().GetAwaiter().GetResult();
        }
    }
}
