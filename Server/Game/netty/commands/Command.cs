namespace Server.Game.netty.commands
{
    class Command
    {
        public byte[] Bytes { get; }

        public bool IsNewClient { get; }

        public Command(byte[] bytes, bool isNewClient)
        {
            Bytes = bytes;
            IsNewClient = isNewClient;
        }
    }
}
