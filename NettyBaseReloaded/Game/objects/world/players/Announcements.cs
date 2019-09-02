using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Announcements : PlayerBaseClass
    {
        private static string[] BANNER_PAYMENT_IDS = new []
        {
            "tradeDrohne", "bigboy_defensive", "bigboy_offensive", "premium_plus", "logfile_advantage",
            "shipdesign_glory", "booster_bonus_box", "booster_quest_reward", "buyPet"
        };

        private static string[] TEXT_ANNOUCNEMENTS = new[]
        {
            "If any bugs occur please contact support.univ3rse.com",
            "Server is still incomplete but developers are working hard to get it all perfectly done",
            "Follow the server's status @status.univ3rse.com",
            "Join our server's Discord\nLink can be found on the startpage of play.univ3rse.com",
            "Server has currently " + World.StorageManager.GameSessions.Count + " connected players.",
            "Restarts generally occur when server is unstable. We're working on fixing it.",
            "Please be patient, server devs are constantly working on the server",
            "A statistical breakup: \nCreated Quests: " + World.StorageManager.Quests.Count + "\nEvents: " + World.StorageManager.Events.Count + "\nClan Battle Stations: " + World.StorageManager.ClanBattleStations.Count,
            "Want to contribute? Buy Uridium or Premium."
        };

        public Announcements(Player player) : base(player)
        {
        }

        public int BannerDelay = 500;
        public int TextDelay = 250;

        public void Tick()
        {
            if (LastAnnouncedBannerTime.AddSeconds(BannerDelay) < DateTime.Now)
            {
                //todo:
                //BannerAnnouncement();
            }
            else if (LastAnnouncedTextTime.AddSeconds(TextDelay) < DateTime.Now)
            {
                TextAnnouncement();
            }
        }

        private DateTime LastAnnouncedBannerTime;
        private int LastAnnouncedBannerIndex = 0;
        public void BannerAnnouncement()
        {
            if (LastAnnouncedBannerIndex == BANNER_PAYMENT_IDS.Length - 1)
            {
                LastAnnouncedBannerIndex = 0;
            }
            else LastAnnouncedBannerIndex++;
            var banner = BANNER_PAYMENT_IDS[LastAnnouncedBannerIndex];
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.CreateBannerAd(session, banner, "c");
            }

            LastAnnouncedBannerTime = DateTime.Now;
        }

        private DateTime LastAnnouncedTextTime;
        private int LastAnnouncedTextIndex = 0;
        public void TextAnnouncement()
        {
            if (LastAnnouncedTextIndex == TEXT_ANNOUCNEMENTS.Length - 1)
            {
                LastAnnouncedTextIndex = 0;
            }
            else LastAnnouncedTextIndex++;
            var text = TEXT_ANNOUCNEMENTS[LastAnnouncedTextIndex];
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|A|STD|" + text);
                Packet.Builder.LegacyModule(session, "0|A|STD|" + (121 -(DateTime.Now - Properties.Server.RUNTIME).Minutes) + " minutes left until automatic restart!");
            }

            LastAnnouncedTextTime = DateTime.Now;
        }
    }
}
