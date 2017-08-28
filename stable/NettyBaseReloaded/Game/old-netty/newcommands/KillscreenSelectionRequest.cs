namespace NettyBaseReloaded.Game.netty.newcommands
{
    class KillscreenSelectionRequest : ClientCommand
    {
        public static int ID = 16044;

        public KillscreenButtonRequest selection;
        public ShipInitializationRequest ShipInitializationRequest;

        public KillscreenSelectionRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            //selection = (KillscreenButtonRequest) CommandHandler.getCommand(SimpleCommand);
            //SimpleCommand.readInt();
            //ShipInitializationRequest = (ShipInitializationRequest)CommandHandler.getCommand(SimpleCommand);
        }
    }
}