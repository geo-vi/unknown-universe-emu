using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Main.commands
{
    class POICommand : Command
    {
        public POICommand() : base("poi", "POI command", false)
        {
        }

        public override void Execute(string[] args = null)
        {
            throw new NotImplementedException();
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            try
            {
                if (!session.Player.RCON) return;

                var equalSession = session.GetEquivilentGameSession();
                POI poiObject;
                switch (args[1])
                {
                    case "all":
                        foreach (var poi in equalSession.Player.Spacemap.POIs.OrderBy(x => x.Value.GetCenterVector().DistanceTo(equalSession.Player.Position)))
                        {
                            var center = poi.Value.GetCenterVector();
                            MessageController.System(session.Player, "POI: key-" + poi.Key + "; shape-" + poi.Value.Shape.ToString() + "; dist-" + equalSession.Player.Position.DistanceTo(center) + " meters away");
                        }
                        break;
                    case "hideall":
                        foreach (var poi in equalSession.Player.Spacemap.POIs)
                        {
                            Packet.Builder.MapRemovePOICommand(equalSession, poi.Value);
                        }
                        break;
                    case "showall":
                        foreach (var poi in equalSession.Player.Spacemap.POIs)
                        {
                            Packet.Builder.MapAddPOICommand(equalSession, poi.Value);
                        }
                        break;
                }
            }
            catch
            {

            }
        }
    }
}
