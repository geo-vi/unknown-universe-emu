using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class GameBan
    {
        public int BanId { get; set; }

        public DateTime IssuedTime { get; set; }

        public string Reason { get; set; }

        public DateTime Expiry { get; set; }

        private int IssuedBy { get; set; } // Issued player id

        public Player GetBanAccountant() => World.DatabaseManager.GetAccount(IssuedBy);

        public GameBan(int id, DateTime issuedTime, string reason, DateTime expiry, int issuedBy)
        {
            BanId = id;
            IssuedTime = issuedTime;
            Reason = reason;
            Expiry = expiry;
            IssuedBy = issuedBy;
        }
    }
}
