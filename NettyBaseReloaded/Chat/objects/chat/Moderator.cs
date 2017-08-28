using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Moderator : Character
    {
        public enum Level
        {
            ADMIN = -1,
            MOD = 1,
            VIP = 2,
            SUPPORTER = 3
        }

        public Level AdminLevel { get; set; }

        /// <summary>
        /// Moderator controller
        /// </summary>
        public new ModeratorController Controller { get; set; }

        public Moderator(int id, string name, Clan clan, Level adminLevel) : base(id, name, clan)
        {
            AdminLevel = adminLevel;
        }
    }
}