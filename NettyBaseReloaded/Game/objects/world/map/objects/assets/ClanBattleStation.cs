using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class ClanBattleStation : Asset, IClickable
    {
        public ClanBattleStation(int id, string name, Faction faction, Clan clan, Vector position, Spacemap map) : base(id, name, AssetTypes.BATTLESTATION, faction, clan, 0, 0, position, map, false, false, false)
        {
            
        }

        public void click(Character character)
        {
            
        }
    }
}
