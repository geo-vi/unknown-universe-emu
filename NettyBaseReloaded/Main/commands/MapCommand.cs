using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Main.commands
{
    class MapCommand : Command
    {
        public MapCommand() : base("map", "Displaying map info", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            try
            {
                var mapId = Convert.ToInt32(args[1]);
                var map = World.StorageManager.Spacemaps[mapId];
                Console.WriteLine("Selected map ID: " + args[1]);
                switch (args[2])
                {
                    case "entities":
                        break;
                    case "collectables":
                        foreach (var entry in map.Objects.Where(x => x.Value is Collectable))
                        {
                            var collectable = entry.Value as Collectable;
                            Console.WriteLine(collectable.Hash + ";" + collectable.Type + ";" + collectable.Position + ";HoneyBox=" + collectable.HoneyBox);
                        }
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            throw new NotImplementedException();
        }
    }
}
