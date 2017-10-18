using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map.zones;

namespace NettyBaseReloaded.Game.controllers.player
{
    class Range : IChecker
    {
        private PlayerController baseController;

        public Range(PlayerController controller)
        {
            baseController = controller;
        }

        public void Check()
        {
            LookupRangeZones();
        }

        private DateTime LastTimeCheckedZones = new DateTime();
        public void LookupRangeZones()
        {
            if (LastTimeCheckedZones.AddMilliseconds(250) > DateTime.Now) return;

            try
            {
                if (baseController.Player.RangeZones.Values.Count(x => x is DemiZone) > 0)
                {
                    if (!baseController.Player.State.InDemiZone && !baseController.Attack.Attacking)
                    {
                        baseController.Player.State.InDemiZone = true;
                        UpdatePlayer();
                    }
                }
                else
                {
                    if (baseController.Player.State.InDemiZone)
                    {
                        baseController.Player.State.InDemiZone = false;
                        UpdatePlayer();
                    }
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("player_range", "Range Zones", e);
            }
            LastTimeCheckedZones = DateTime.Now;
        }

        private DateTime LastTimeCheckedObjects = new DateTime();
        public void LookupRangeObjects()
        {
            //TODO
            LastTimeCheckedObjects = DateTime.Now;
        }

        public void UpdatePlayer()
        {
            var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
            Packet.Builder.BeaconCommand(gameSession);
        }
    }
}
