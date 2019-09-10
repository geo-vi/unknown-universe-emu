using DotNetty.Buffers;
using Server.Game.managers;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.enums;

namespace Server.Game.netty.handlers
{
    class HellstormLaunchHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            CombatManager.Instance.CreateCombat(gameSession.Player, gameSession.Player.Selected, AttackTypes.ROCKET_LAUNCHER, 
                gameSession.Player.Settings.GetSettings<SlotbarSettings>().SelectedHellstormRocketAmmo);
        }
    }
}
