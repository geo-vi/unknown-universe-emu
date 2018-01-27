using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class Premium
    {
        public DateTime ExpiryDate { get; set; }

        public bool Active => ExpiryDate > DateTime.Now;

        public void Login(GameSession gameSession)
        {
            if (Active)
            {
                Packet.Builder.LegacyModule(gameSession,
                    $"0|A|STD|Premium active until {ExpiryDate.ToLongDateString()}.\nThanks for supporting the server!");
            }
        }

        public void Update(Player player)
        {
            if (player != null)
            {
                var gameSession = player.GetGameSession();
                if (gameSession == null) return;
                Packet.Builder.QuickSlotPremiumCommand(player.GetGameSession());
                if (Active)
                    Packet.Builder.LegacyModule(player.GetGameSession(),
                        $"0|A|STD|Premium active until {ExpiryDate.ToLongDateString()}.\nThanks for supporting the server!");
                else
                    Packet.Builder.LegacyModule(player.GetGameSession(),
                        $"0|A|STD|Your Premium expired on {ExpiryDate.ToLongDateString()}.\nIf you wish to support the server once again, head over to the Premium page.");
            }
        }
    }
}
