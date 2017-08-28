namespace NettyBaseReloaded.Game.netty.newcommands
{
    abstract class IServerCommand : SimpleCommand
    {
        public abstract void write();
    }
}
