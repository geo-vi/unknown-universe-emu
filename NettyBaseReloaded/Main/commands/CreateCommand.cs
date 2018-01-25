namespace NettyBaseReloaded.Main.commands
{
    class CreateCommand : Command
    {
        public CreateCommand() : base("createasset", "Creates an asset") { }

        public override void Execute(string[] args = null)
        {
            if (args != null && args.Length > 2)
            {
                var mapId = args[1];
                var assetType = args[2];
                var x = args[3];
                var y = args[4];
            }
        }
    }
}