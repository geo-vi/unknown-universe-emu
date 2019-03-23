namespace NettyStatusBot.Networking.packets
{
    abstract class PacketBase
    {
        public abstract string Header { get; }

        public abstract void readPacket(string packet);

        public static string GetBase(string s)
        {
            if (s.Contains("|"))
            {
                return s.Split('|')[0];
            }

            return s;
        }
    }
}
