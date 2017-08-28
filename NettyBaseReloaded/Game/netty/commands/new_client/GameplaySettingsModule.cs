using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class GameplaySettingsModule
    {
        public const short ID = 28409;
        public bool O2W;
        public bool autoBoost;
        public bool autoRefinement;
        public bool D4d;
        public bool showBattlerayNotifications;
        public bool N2G;
        public bool N2e;
        public bool F3w;
        public bool HD;
        public bool D4P;

        public GameplaySettingsModule(bool o2W, bool autoBoost, bool autoRefinement, bool d4D, bool showBattlerayNotifications, bool n2G, bool n2E, bool f3W, bool hd, bool d4P)
        {
            O2W = o2W;
            this.autoBoost = autoBoost;
            this.autoRefinement = autoRefinement;
            D4d = d4D;
            this.showBattlerayNotifications = showBattlerayNotifications;
            N2G = n2G;
            N2e = n2E;
            F3w = f3W;
            HD = hd;
            D4P = d4P;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(this.O2W);
            cmd.Boolean(this.autoBoost);
            cmd.Boolean(this.autoRefinement);
            cmd.Boolean(this.D4d);
            cmd.Short(-9045);
            cmd.Boolean(this.showBattlerayNotifications);
            cmd.Boolean(this.N2G);
            cmd.Boolean(this.N2e);
            cmd.Boolean(this.F3w);
            cmd.Boolean(this.HD);
            cmd.Boolean(this.D4P);
            return cmd.Message.ToArray();
        }
    }
}
