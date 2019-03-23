using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class Credits : BaseInfo
    {
        public Credits(Player player) : base(player)
        {
            SqlName = "CREDITS";
        }

        public override void Refresh()
        {
            bool upd = false;
            World.DatabaseManager.LoadInfo(Player, this);
            if (Value != SyncedValue)
                upd = true;
            Value = SyncedValue;
            if (upd) Update();
        }

        public override void Add(double amount)
        {
            World.DatabaseManager.UpdateInfo(Player, this, amount);
            Value = SyncedValue;
        }

        public override void Remove(double amount)
        {
            World.DatabaseManager.UpdateInfo(Player, this, -amount);
            Value = SyncedValue;
        }

        public override void Set(double value)
        {
            World.DatabaseManager.SetInfo(Player, this, value);
            Value = SyncedValue;
            Update();
        }

        public override void Update()
        {
            var gameSession = Player.GetGameSession();
            if (gameSession != null)
                Packet.Builder.AttributeCurrencyCommand(gameSession);
        }
    }
}
