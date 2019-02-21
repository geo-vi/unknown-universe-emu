namespace NettyStatusBot.network.packets
{
    abstract class PacketBase
    {
        public abstract string Header { get; }

        public abstract void readPacket(string packet);
    }
}
