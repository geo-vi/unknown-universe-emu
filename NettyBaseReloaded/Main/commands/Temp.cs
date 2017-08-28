using System;
using System.Collections.Generic;
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

        public override void Execute(string[] args = null)
        {
            for (int i = 0; i < 10; i++)
            {
                World.StorageManager.Spacemaps[12].CreateOre(OreTypes.DURANIUM, Vector.Random(16000, 20000, 1000, 5000));                
                World.StorageManager.Spacemaps[12].CreateBox(Types.BONUS_BOX, Vector.Random(16000, 20000, 1000, 5000));
            }
        }
    }
}