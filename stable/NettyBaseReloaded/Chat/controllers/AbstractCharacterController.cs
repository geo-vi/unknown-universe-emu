using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Chat.controllers
{
    class AbstractCharacterController
    {
        public Character Character { get; }

        public AbstractCharacterController(Character character)
        {
            Character = character;
        }
    }
}