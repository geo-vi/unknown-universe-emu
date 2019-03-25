namespace NettyStatusBot.Networking.packets
{
    class PongPacket : PacketBase
    {
        public override string Header => "pong";

        public override void readPacket(string packet)
        {
        }
    }
}
