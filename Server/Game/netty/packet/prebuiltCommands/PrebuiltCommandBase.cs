namespace Server.Game.netty.packet.prebuiltCommands
{
    abstract class PrebuiltCommandBase
    {
        public abstract void AddCommands();
        
        protected void ArgumentFixer(object[] args, int expectedLength, out object[] newArgs)
        {
            newArgs = args;
            if (args.Length >= expectedLength) return;
            
            newArgs = new object[expectedLength];
            var i = 0;
            while (i != expectedLength - 1)
            {
                if (i > args.Length - 1)
                {
                    newArgs[i] = 0;
                }
                else
                {
                    newArgs[i] = args[i];
                }
                i++;
            }
        }
    }
}