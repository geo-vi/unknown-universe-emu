using System.Globalization;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class QuestGiver : Asset, IClickable
    {
        public QuestGiver(int id, Faction faction, Vector pos, Spacemap map) : base(id, "Nyx", AssetTypes.QUESTGIVER, faction, Global.StorageManager.Clans[0], 1, 0, pos, map, false, false, false)
        {
            
        }

        public void click(Character character)
        {
            // TODO
        }
    }
}
