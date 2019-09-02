using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    interface ILegacyHandler
    {
        void Execute(GameSession gameSession, string[] param);
    }
}
