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
        }

        public void Sync(DateTime premEnd)
        {
            ExpiryDate = premEnd;
        }
    }
}
