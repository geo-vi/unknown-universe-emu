using System.Globalization;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class QuestGiver : Asset, IClickable
    {
        public QuestGiver(int id, Faction faction, Vector pos) : base(id, "Nyx", AssetTypeModule.QUESTGIVER, (int)faction, "", id, 1, 0, pos, -1, false, false, false)
        {
            
        }

        public void click(Character character)
        {
            // TODO
        }
    }
}
