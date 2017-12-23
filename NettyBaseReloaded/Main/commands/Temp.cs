using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Main.commands
{
    class Temp : Command
    {
        public Temp() : base("temp", ""){ }

        public override async void Execute(string[] args = null)
        {
            for (int i = 0; i < 105; i++)
            {
                new ExceptionLog("temp", "Just a test", new WarningException("Just a working warning exception"));
                await Task.Delay(150);
            }
        }
    }
}