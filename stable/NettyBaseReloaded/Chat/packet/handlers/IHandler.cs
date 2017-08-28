using NettyBaseReloaded.Chat.objects;

namespace NettyBaseReloaded.Chat.packet.handlers
{
    interface IHandler
    {
        void execute(ChatSession session, string[] packet);
    }
}