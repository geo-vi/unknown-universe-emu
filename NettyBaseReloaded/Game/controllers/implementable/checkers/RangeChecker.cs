namespace NettyBaseReloaded.Game.controllers.implementable.checkers
{
    abstract class RangeChecker
    {
        public AbstractCharacterController Controller;

        /// <summary>
        /// Abstracts
        /// </summary>
        public abstract void Tick();

        public abstract void Reset();
    }
}