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
        public const int MAX_COMBO = 10;
        public const int MAX_LIVES = 5;
        public const int MAX_COMBO_TIME = 60;

        public int Lives { get; set; }

        public int Combo { get; set; }

        public DateTime ComboTimeEnd = new DateTime();

        public ScoreMageddon():base(null, 0, -1) { }

        public ScoreMageddon(Player player, int id) : base(player, id, 500)
        {
            Lives = 5;
        }

        public ScoreMageddon(Player player, ScoreMageddon data) : this(player, data.Id)
        {
            Lives = data.Lives;
            Combo = data.Combo;
            ComboTimeEnd = data.ComboTimeEnd;
            Score = data.Score;
        }

        protected override void UpdatePlayer()
        {
            if (ComboTimeEnd < DateTime.Now && Combo > 0)
            {
                Combo -= 1;
                ComboTimeEnd = DateTime.Now.AddSeconds(MAX_COMBO_TIME);

                UpdateCombo();
                UpdateWindow();
            }
        }

        public override void Destroyed()
        {
            Lives -= 1;
            Combo = 0;
            ComboTimeEnd = DateTime.Now;
            World.DatabaseManager.UpdateEventForPlayer(this);
        }

        public override void Start()
        {
            UpdateCombo();
            UpdateWindow();
            World.DatabaseManager.UpdateEventForPlayer(this);
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

                if (Combo >= 3 && Combo < 5) Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|KSMSG|" + KEYS[0]);
                if (Combo >= 5 && Combo < 7) Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|KSMSG|" + KEYS[1]);
                if (Combo >= 7 && Combo < 9) Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|KSMSG|" + KEYS[2]);
                if (Combo >= 9) Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|KSMSG|" + KEYS[3]);

                var addedPoints = 0;
                if (Combo > 0)
                    addedPoints = 100 * Combo;
                Score += 100 + addedPoints;
                ComboTimeEnd = DateTime.Now.AddSeconds(MAX_COMBO_TIME);
            }
            else if (attackable is Character)
            {
                if (Combo < MAX_COMBO)
                    Combo++;

                if (Combo == 3) Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|KSMSG|" + KEYS[0]);
                if (Combo == 5) Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|KSMSG|" + KEYS[1]);
                if (Combo == 7) Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|KSMSG|" + KEYS[2]);
                if (Combo == 9) Packet.Builder.LegacyModule(Player.GetGameSession(), "0|n|KSMSG|" + KEYS[3]);

                var addedPoints = 0;
                if (Combo > 0)
                    addedPoints = 1 * Combo;
                Score += 1 + addedPoints;
                ComboTimeEnd = DateTime.Now.AddSeconds(MAX_COMBO_TIME);
            }

            UpdateCombo();
            UpdateWindow();
            World.DatabaseManager.UpdateEventForPlayer(this);
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
            return ComboTimeEnd > DateTime.Now ? Convert.ToInt32((ComboTimeEnd - DateTime.Now).TotalSeconds) : 0;
        }

        public int GetMaxCombo()
        {
            return Lives > 0 ? MAX_LIVES : 0;
        }

        private void UpdateCombo()
        {
            GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write($"0|n|SCEL|{Player.Id}|{Lives}|{MAX_LIVES}|{Combo}|{MAX_COMBO}"), true);
            GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write($"0|n|SCEL|{Player.Id}|{Lives}|{MAX_LIVES}|{Combo}|{MAX_COMBO}"), true);
        }

        private void UpdateWindow()
        {
            Packet.Builder.UpdateScoremageddonWindow(Player.GetGameSession(), this);
        }
    }
}
