using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Moderator : Player
    {
        public ModeratorLevelTypes AdminLevel { get; set; }

        /// <summary>
        /// Moderator controller
        /// </summary>
        public new ModeratorController Controller { get; set; }

        public Moderator(int id, string name, string sessionId, Clan clan, ModeratorLevelTypes adminLevel) : base(id, name, sessionId, clan)
        {
            AdminLevel = adminLevel;
        }
    }
}