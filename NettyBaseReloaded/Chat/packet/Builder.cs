using System;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;

namespace NettyBaseReloaded.Chat.packet
{
    class Builder
    {
        #region Legacy
        public void Legacy(ChatSession chatSession, string packet)
        {
            chatSession.Client.Send(packet + "#");
        }
        #endregion
        #region System Message

        public void SystemMessage(ChatSession chatSession, string msg)
        {
            Legacy(chatSession, "dq%" + msg);
        }
        #endregion

        #region Message

        public void Message(ChatSession chatSession, Room room, string msg)
        {

        }
        #endregion

        #region Send Rooms

        public void SendRooms(ChatSession chatSession)
        {
            string packet = Constants.CMD_SET_USER_ROOMLIST + "%";
            foreach (var roomConnected in chatSession.Character.ConnectedRooms.Values)
            {
                packet += roomConnected.ToString();
            }
            Legacy(chatSession, packet);
        }
        #endregion
    }
}