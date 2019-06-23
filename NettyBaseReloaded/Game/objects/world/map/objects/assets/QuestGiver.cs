using System;
using System.Globalization;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class QuestGiver : Asset, IClickable
    {
        public int QuestGiverId;

        public QuestGiver(int id, int questGiverId, Faction faction, Vector pos, Spacemap map) : base(id, "Nyx", AssetTypes.QUESTGIVER, faction, Global.StorageManager.Clans[0], 2, 0, pos, map, false, false, false)
        {
            QuestGiverId = questGiverId;
        }

        public override void execute(Character character)
        {
            var player = character as Player;
            if (player != null && !player.UsingNewClient)
            {
                click(character);
            }
        }

        public void click(Character character)
        {
            if (character is Player player && player.FactionId == Faction)
            {
                var session = player.GetGameSession();
                if (session != null)
                {
                    Packet.Builder.QuestGiversAvailableCommand(session, this);
                }
            }
        }
    }
}
