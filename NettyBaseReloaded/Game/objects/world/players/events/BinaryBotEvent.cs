using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.npcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.events
{
    class BinaryBotEvent : PlayerEvent
    {
        public BinaryBotEvent(Player player, int id) : base(player, id, 3000)
        {
        }

        private Spacemap AnnouncedSpacemap;
        
        public override void Tick()
        {
            if (Player.Spacemap == AnnouncedSpacemap && Player.Spacemap.Entities.Any(x => x.Value is EventNpc))
            {
                var eventNpc = Player.Spacemap.Entities.FirstOrDefault(x => x.Value is EventNpc).Value as EventNpc;
                eventNpc.PrivateAnnouncement(Player.GetGameSession());
                AnnouncedSpacemap = Player.Spacemap;
            }
        }

        public override void End()
        {
        }

        public override void Start()
        {
            Packet.Builder.LegacyModule(Player.GetGameSession(), "0|A|STD|Binary Bots Event started!");
        }
    }
}
