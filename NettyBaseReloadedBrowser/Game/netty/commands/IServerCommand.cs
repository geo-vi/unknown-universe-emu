namespace NettyBaseReloadedBrowser.Game.netty.commands
{
    abstract class IServerCommand : SimpleCommand
    {
        public abstract void write();
    }
}
