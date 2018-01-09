using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class StationModule : Object, IClickable
    {
        public AssetTypes Type { get; private set; }

        public StationModule(int id, Vector pos, AssetTypes type) : base(id, pos)
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
                case AssetTypes.REPAIR_DOCK:
                    character.Controller.Damage?.Area(character.MaxHealth, Id);
                    break;
            }
        }
    }
}
