using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects;

namespace NettyBaseReloaded.Chat.packet.handlers
{
    class UserMessageHandler : IHandler
    {
        public void execute(ChatSession chatSession, string[] param)
        {
            var roomId = int.Parse(param[1]);
            var message = param[2];

            if (chatSession.Player.ConnectedToRoom(roomId))
                MessageController.Send(chatSession.Player, roomId, message);
        }
    }
}