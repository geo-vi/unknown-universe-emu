using System;
using NettyBaseReloaded.Game.netty;

namespace Server.Game.netty.handlers
{
    class JumpCPUHandler: ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            switch (param[1])
            {
                case "S"://select
                    var mapId = int.Parse(param[2]);
                    var price = World.PortalSystemManager.CalculateDistance(mapId, gameSession.Player.Spacemap.Id) * 50;
                    bool jumpPossibility = !(mapId == gameSession.Player.Spacemap.Id) || !World.PortalSystemManager.GetOpenMapsList(gameSession.Player).Contains(mapId);
                    if (jumpPossibility) gameSession.Player.Controller.CPUs.SelectedMapId = mapId;
                    gameSession.Player.Controller.CPUs.JumpSequenceActive = false;
                    Packet.Builder.JumpCPUFeedbackCommand(gameSession, mapId, price, jumpPossibility);
                    break;
                case "J":
                    //Packet.Builder.LegacyModule(gameSession, "0|A|STD|TODO");
                    Packet.Builder.LegacyModule(gameSession, "0|A|JCPU|S|10|1");
                    gameSession.Player.Controller.CPUs.JumpSequenceEnd = DateTime.Now.AddSeconds(10);
                    gameSession.Player.Controller.CPUs.JumpSequenceActive = true;
                    break;
            }
        }
    }
}
