using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Bot : Character
    {

        /// <summary>
        /// Bot controller
        /// </summary>
        public new BotController Controller { get; set; }

        public Bot(int id, string name) : base(id, name, "", new Clan(-1, "We are the bots", "BOT", -1))
        {

        }
    }
}