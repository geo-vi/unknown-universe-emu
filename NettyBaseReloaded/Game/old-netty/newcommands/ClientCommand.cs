namespace NettyBaseReloaded.Game.netty.newcommands
{
    /// <summary>
    /// This class will be used as template for all the ClientCommands
    /// </summary>
    abstract class ClientCommand : SimpleCommand
    {
        public int id { get; set; }
        protected SimpleCommand SimpleCommand { get; set; }

        protected ClientCommand(int id, SimpleCommand simpleCommand) : base(simpleCommand)
        {
            this.id = id;
            SimpleCommand = simpleCommand;
        }

        public abstract void readCommand();
    }
}