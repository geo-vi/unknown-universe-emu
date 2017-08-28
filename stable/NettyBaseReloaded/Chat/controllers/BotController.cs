using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Chat.controllers
{
    class BotController : AbstractCharacterController, ITick
    {
        public BotController(Character character) : base(character)
        {

        }

        public void Tick()
        {

        }
    }
}