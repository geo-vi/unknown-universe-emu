using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;

namespace NettyBaseReloaded.Game.objects.world.map.objects.stations
{
    class StationModule : Object, IClickable
    {
        public AssetTypes Type { get; private set; }

        public StationModule(int id, Vector pos, Spacemap map, AssetTypes type) : base(id, pos, map)
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
                    character.Controller.Damage?.Area(character.MaxHealth, Damage.Types.KAMIKAZE);
                    break;
            }
        }
    }
}
