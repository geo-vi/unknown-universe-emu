using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    interface ILegacyHandler
    {
        void execute(GameSession gameSession, string[] param);
    }
}
