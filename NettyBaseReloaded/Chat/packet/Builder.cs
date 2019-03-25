using System;
using System.Linq;
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
            foreach (var roomConnected in chatSession.Player.ConnectedRooms.Values)
            {
                packet += roomConnected.ToString();
                if (roomConnected != chatSession.Player.ConnectedRooms.Values.Last())
                    packet += "}";
            }

            Legacy(chatSession, packet);
        }
        #endregion

        #region Whisper
        public void Whisper(ChatSession session, string from, string msg)
        {
            Legacy(session, $"cv%{from}@{msg}");
        }

        public void WhisperTo(ChatSession session, string to, string msg)
        {
            Legacy(session, $"cw%{to}@{msg}");
        }
        #endregion

        #region User Not Exist

        public void UserNotExist(ChatSession session)
        {
            Legacy(session, "ct");
        }
        #endregion
    }
}