using System;

namespace NettyStatusBot.Networking.packets
{
    class ChatLogPacket : PacketBase
    {
        public override string Header => "cl";

        public int RoomId;

        public int Id;

        public string Username;

        public string ChatRank;

        public string Message;

        public override void readPacket(string packet)
        {
            var split = packet.Split('|');
            RoomId = int.Parse(split[1]);
            Id = int.Parse(split[2]);
            Username = split[3];
            ChatRank = split[4];
            Message = split[5];
        }
    }
}
