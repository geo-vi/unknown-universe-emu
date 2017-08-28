using System.Collections.Generic;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Main.commands
{
    class Temp : Command
    {
        public Temp() : base("temp", ""){ }

        public override void Execute(string[] args = null)
        {
            foreach (var user in World.StorageManager.GameSessions.Values)
            {
                for (int i = 0; i <= 250; i++)
                {
                    var id = 1000 + i;
                    var pos = Vector.Random(0, 41600, 0, 25600, "416x256");

                    user.Client.Send(MoveCommand.write(id, pos.X, pos.Y, 16000).Bytes);
                }
            }
        }
    }
}