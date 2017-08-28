namespace NettyBaseReloaded.Game.netty.newcommands
{
    class ShipInitializationRequest : ClientCommand
    {
        public static int ID = 21821;

        public int factionID;
        public string version;
        public int instanceId;
        public int playerId;
        public string sessionId;

        public ShipInitializationRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            readShort();
            this.instanceId = readInt();
            this.instanceId = (int)(((uint)this.instanceId >> 6) | ((uint)this.instanceId << 26));
            this.factionID = readShort();
            this.playerId = readInt();
            this.playerId = (int)(((uint)this.playerId << 3) | ((uint)this.playerId >> 29));
            this.version = readUTF();
            this.sessionId = readUTF();
        }
    }
}