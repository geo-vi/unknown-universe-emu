using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.events;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.players.events
{
    class ScoreMageddon : PlayerEvent
    {
        public static string[] KEYS = { "epic_msg_killstreak_triplekill", "epic_msg_killstreak_unstoppable", "epic_msg_killstreak_godlike", "epic_msg_killstreak_likeaboss" };
        public const int MAX_COMBO = 5;
        public const int MAX_LIVES = 5;
        public const int MAX_COMBO_TIME = 60;

        public int Lives { get; set; }

        public int Combo { get; set; }

        public DateTime ComboTimeEnd = new DateTime();

        public ScoreMageddon(Player player, int id) : base(player, id, 500)
        {
        }

        private DateTime LastDbRefresh = new DateTime();
        protected override void UpdatePlayer()
        {
            if (ComboTimeEnd < DateTime.Now && Combo > 0)
            {
                Combo -= 1;
                ComboTimeEnd = DateTime.Now.AddSeconds(MAX_COMBO_TIME);
            }
            if (LastDbRefresh.AddSeconds(3) < DateTime.Now)
            {
                World.DatabaseManager.UpdateEventForPlayer(this);
                LastDbRefresh = DateTime.Now;
            }

            Packet.Builder.UpdateScoremageddonWindow(Player.GetGameSession(), this);
        }

        public override void Destroyed()
        {
            Lives -= 1;
            Combo = 0;
            ComboTimeEnd = DateTime.Now;
        }

        public override void Start()
        {
            Packet.Builder.UpdateScoremageddonWindow(Player.GetGameSession(), this);
            GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write($"0|n|SCEL|{Player.Id}|{Lives}|{MAX_LIVES}|{Combo}|{MAX_COMBO}"));
            GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write($"0|n|SCEL|{Player.Id}|{Lives}|{MAX_LIVES}|{Combo}|{MAX_COMBO}"));
        }

        public override void End()
        {
            Lives = 0;
            Combo = 0;
            Packet.Builder.UpdateScoremageddonWindow(Player.GetGameSession(), this);
        }

        public override void DestroyAttackable(IAttackable attackable)
        {
            if (attackable is Player)
            {
                if (Lives < MAX_LIVES)
                    Lives++;
                if (Combo < MAX_COMBO)
                    Combo++;

                if (Combo == 3) Packet.Builder.LegacyModule(Player.GetGameSession(), "");

                var addedPoints = 0;
                if (Combo > 0)
                    addedPoints = 100 * Combo;
                Score += 100 + addedPoints;
                ComboTimeEnd = DateTime.Now.AddSeconds(MAX_COMBO_TIME);
                GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write($"0|n|SCEL|{Player.Id}|{Lives}|{MAX_LIVES}|{Combo}|{MAX_COMBO}"));
                GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write($"0|n|SCEL|{Player.Id}|{Lives}|{MAX_LIVES}|{Combo}|{MAX_COMBO}"));
            }
        }
        
        // PACKETS -> 
        // 0|n|SCEL|userID|lives|maxlives|combo|maxcombo -> display combo
        // 0|n|KSMSG|key -> show big message
        // 0|A|SCE|lives|maxlives|combo|maxcombo|combotimer|combotimermax|points -> update window
        // 0|A|KSCE -> event finished / remove window
        public int GetMaxLives()
        {
            return Lives > 0 ? MAX_LIVES : 0;
        }

        public int GetComboTimeLeft()
        {
            return ComboTimeEnd > DateTime.Now ? (ComboTimeEnd - DateTime.Now).Seconds : 0;
        }

        public int GetMaxCombo()
        {
            return Lives > 0 ? MAX_LIVES : 0;
        }
    }
}
