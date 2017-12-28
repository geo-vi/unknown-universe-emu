using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class StationModule : Object, IClickable
    {
        public short Type { get; private set; }

        public StationModule(int id, Vector pos, short type) : base(id, pos)
        {
            Type = type;
        }
        public override void execute(Character character)
        {
        }

        public void click(Character character)
        {
            switch (Type)
            {
                case AssetTypeModule.REPAIR_DOCK:
                    character.Controller.Damage?.Area(character.MaxHealth, Id);
                    break;
            }
        }
    }
}
